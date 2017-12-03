using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThankingProcessing.Models
{
    public class UserObject
    {
        public UserObject()
        {

        }
        
        public long Id = 0;
        public string Ip = "";
        public string Hwid = "";
        public string SteamName = "";
        public long Steam64 = 0;
        public bool IsPremium = false;
        public bool IsBlacklisted = false;
        public DateTime LastUse = DateTime.Now;
    }
}
