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

namespace ThankingProcessing.Controllers
{

    public class HomeController : Controller
    {
        private async Task<string> Encrypt(string toEncrypt)
        {
            char[] buffer = toEncrypt.ToCharArray();
            char[] tmp = toEncrypt.ToCharArray();

            Parallel.For(0, toEncrypt.Length, i =>
            {
                buffer[i] = (char)(tmp[i] + 1);
            });

            return new string(buffer);
        }

        private async Task<string> EncryptFile(string path)
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
                return Encrypted;
            }
            catch
            {
                return "Error";
            }
        }

        private string SendError(int ErrorId, string ErrorMessage)
        {
            dynamic jsonObject = new JObject();
            jsonObject.success = false;
            dynamic jsonErrorObject = new JObject();
            jsonErrorObject.error_ID = ErrorId;
            jsonErrorObject.error_message = ErrorMessage;
            jsonObject.data = jsonErrorObject;
            return jsonObject.ToString(Formatting.None);
        }

        private string SendData(JObject data)
        {
            dynamic jsonObject = new JObject();
            jsonObject.success = true;
            jsonObject.data = data;
            return jsonObject.ToString(Formatting.None);
        }

        public IActionResult Index()
        {
            ViewBag.Message = "Error: No Data Input.";
            return View();
        }

        [HttpPost]
        [Route("download")]
        public async Task<string> download(DownloadRequest content)
        {
            if (content == null) return SendError(10, "Missing data or malformed request!");
            if (content.Stage == null || content.Hwid == null) return SendError(10, "Missing data or malformed request!");

            int stage = 0;
            if (!int.TryParse(content.Stage, out stage))
                return SendError(11, "Stage is not of type int!");

            switch(stage)
            {
                case 1:
                    dynamic injection = new JObject();
                    injection.injection = await EncryptFile("files/ironic-loader.dll");
                    return SendData(injection);
                    break;
                case 2:
                    dynamic hack = new JObject();
                    hack.hack = await EncryptFile("files/ironic-payload-loader.dll");
                    return SendData(hack);
                    break;
                case 3:
                    dynamic payload = new JObject();
                    payload.payload = await EncryptFile("files/Thanking.dll");
                    return SendData(payload);
                    break;
                case 4:
                    break;
            }

            return SendError(12, "Unknown Stage!");
        }
    }
}