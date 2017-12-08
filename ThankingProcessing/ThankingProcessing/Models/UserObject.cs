using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThankingProcessing.Models
{
    public class UserObject
    {
        public UserObject()
        {

        }

        [Key]
        public int Id { get; set; } = 0;
        public string hwid { get; set; } = "";
        public string ip { get; set; } = "";
        public bool premium { get; set; } = false;
        public bool blacklisted { get; set; } = false;
        public string steamname { get; set; } = "";
        public Int64 steam64 { get; set; } = 0;
        public DateTime lastuse { get; set; } = DateTime.Now;
        public string forum { get; set; } = "";
    }
}
