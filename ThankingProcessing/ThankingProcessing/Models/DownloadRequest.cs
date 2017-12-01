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
        public string Steamname { get; set; }
        public string Steam64 { get; set; }
    }
}
