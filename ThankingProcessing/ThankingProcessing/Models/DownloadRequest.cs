using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThankingProcessing.Models
{
    public class DownloadRequest
    {
        public string stage { get; set; }
        public string hwid { get; set; }
        public string steamname { get; set; }
        public string steam64 { get; set; }
    }
}
