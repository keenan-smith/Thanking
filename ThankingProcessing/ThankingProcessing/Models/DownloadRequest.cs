using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThankingProcessing.Models
{
    public class DownloadRequest
    {
        public string Stage { get; set; }
        public string Hwid { get; set; }
        public string Steam_name { get; set; }
        public string Steam_64 { get; set; }
    }
}
