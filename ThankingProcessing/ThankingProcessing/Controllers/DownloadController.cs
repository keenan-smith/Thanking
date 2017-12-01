using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using ThankingProcessing.Models;

namespace ThankingProcessing.Controllers
{
    public class HomeController : Controller
    {
        private string GetDocumentContents(Microsoft.AspNetCore.Http.HttpRequest Request)
        {
            string documentContents;
            using (Stream receiveStream = Request.Body)
            {
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    documentContents = readStream.ReadToEnd();
                }
            }
            return documentContents;
        }

        public IActionResult Index()
        {
            ViewBag.Message = "Error: No Data Input.";
            return View();
        }

        [HttpPost]
        [Route("download")]
        public string download(DownloadRequest content)
        {
            return content.stage;
        }
    }
}