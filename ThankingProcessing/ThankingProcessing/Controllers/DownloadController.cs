using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using ThankingProcessing.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace ThankingProcessing.Controllers
{
    public class EncryptReturn //Object to return data from a Task
    {
        public EncryptReturn(bool Success, string data)
        {
            this.Success = Success;
            this.data = data;
        }

        public bool Success = false;
        public string data = null;
    }

    public class HomeController : Controller
    {
        private UsersContext context;
        public HomeController(UsersContext context)
        {
            this.context = context;
        }

        private async Task<string> Encrypt(string toEncrypt) //gigantic encryption method here
        {
            char[] buffer = toEncrypt.ToCharArray();
            char[] tmp = toEncrypt.ToCharArray();

            Parallel.For(0, toEncrypt.Length, i =>
            {
                buffer[i] = (char)(tmp[i] + 1);
            });

            return new string(buffer);
        }

        private async Task<EncryptReturn> EncryptFile(string path) //Gets a file at the path passed, and encrypts it in the proper format. Returns a string
        {
            try
            {
                byte[] RawBytes = await System.IO.File.ReadAllBytesAsync(path);
                string RawBase64 = Convert.ToBase64String(RawBytes);
                string RawBase64Encrypted = await Encrypt(RawBase64);
                string RawBase64EncryptedReversed = new string(RawBase64Encrypted.ToCharArray().Reverse().ToArray());
                byte[] ProcessedBytes = Encoding.UTF8.GetBytes(RawBase64EncryptedReversed);
                string ProcessedBase64 = Convert.ToBase64String(ProcessedBytes);
                string Encrypted = new string(ProcessedBase64.ToCharArray().Reverse().ToArray());
                return new EncryptReturn(true, Encrypted);
            }
            catch
            {
                return new EncryptReturn(false, "");
            }
        }

        private string SendError(int ErrorId, string ErrorMessage) //Send errors in the proper format (return SendError(int ID, string message);)
        {
            dynamic jsonObject = new JObject();
            jsonObject.success = false;
            dynamic jsonErrorObject = new JObject();
            jsonErrorObject.error_ID = ErrorId;
            jsonErrorObject.error_message = ErrorMessage;
            jsonObject.data = jsonErrorObject;
            return jsonObject.ToString(Formatting.None);
        }

        private string SendData(JObject data) //Send data in the proper format (return SendData(JObject object);)
        {
            dynamic jsonObject = new JObject();
            jsonObject.success = true;
            jsonObject.data = data;
            return jsonObject.ToString(Formatting.None);
        }

        public async Task<IActionResult> Index() //Basic index task (Nothing needed here)
        {
            ViewBag.Message += "Error: No Data Input. | " + Request.Headers["CF-Connecting-IP"];
            return View();
        }

        [HttpPost]
        [Route("download")]
        public async Task<string> download(DownloadRequest content) //Download API endpoint
        {
            //Grabs IP from cloudflare header
            string IpAddress = Request.Headers["CF-Connecting-IP"];

            if (content == null) return SendError(10, "Missing data or malformed request!"); //Bad POST request
            if (content.Stage == null || content.Hwid == null) return SendError(10, "Missing data or malformed request!"); //No stage or HWID

            //Get user record from SQL server
            UserObject User = await DBUtils.GetUser(context, content.Hwid);

            if (User == null) //Record doesnt exist, create one
            {
                User = new UserObject
                {
                    hwid = content.Hwid,
                    ip = IpAddress,
                    lastuse = DateTime.Now
                };

                await DBUtils.AddUser(context, User);
            }

            //Update records
            User.ip = IpAddress;
            User.lastuse = DateTime.Now;
            await context.SaveChangesAsync();

            //Check if premium
            if (!User.premium)
                return SendError(-2, "Error parsing your POST data!");
            
            //Parse stage
            if (!int.TryParse(content.Stage, out int stage))
                return SendError(11, "Stage is not of type int!");

            switch(stage)
            {
                case 1: //Injector
                    dynamic injection = new JObject();
                    EncryptReturn injectionReturn = await EncryptFile("files/ironic-loader.dll");
                    if (!injectionReturn.Success)
                        return SendError(9, "Internal Server Error");
                    injection.injection = injectionReturn.data;
                    return SendData(injection);
                case 2: //Native DLL
                    dynamic hack = new JObject();
                    EncryptReturn hackReturn = await EncryptFile("files/ironic-payload-loader.dll");
                    if (!hackReturn.Success)
                        return SendError(9, "Internal Server Error");
                    hack.hack = hackReturn.data;
                    return SendData(hack);
                case 3: //C# Assembly
                    dynamic payload = new JObject();
                    EncryptReturn payloadReturn = await EncryptFile("files/Thanking.dll");
                    if (!payloadReturn.Success)
                        return SendError(9, "Internal Server Error");
                    payload.payload = payloadReturn.data;
                    return SendData(payload);
                case 4: //Heartbeat
                    long Steam64 = 0;
                    if (!Int64.TryParse((content.Steam_64 == null ? "0" : content.Steam_name), out Steam64))
                        Steam64 = 0;

                    User.steam64 = (Steam64 == 0 ? User.steam64 : Steam64);
                    User.steamname = (content.Steam_name == null ? User.steamname : content.Steam_name);
                    await context.SaveChangesAsync();

                    //Returned string (Client checks if it contains a certain string)
                    return "cLNX4sftHUD03zGVamceM1pWeqlhmUhCjTkrw74UeyFxDOSaPnNWu5mU3bGK9bJMbvOB+mbC5S5Sz7ekahgTyqkeF0GBXBBUPCUtqwaZa4m65c9tTgZPMs8k5I2I709rQUZGfvmJuGCJUAtzMPmTJEUIHbcJtTVITplQ";
            }

            //No other possiblities
            return SendError(12, "Unknown Stage!");
        }
    }
}