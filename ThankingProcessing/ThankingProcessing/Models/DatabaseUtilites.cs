using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace ThankingProcessing.Models
{
    public class DatabaseUtilites
    {
        public async Task<UserObject> GetUser(string HWID)
        {
            using (var db = new AppDb(Startup.database))
            {

            }
        }

        public async Task<List<UserObject>> ReadAllAsync(DbDataReader reader)
        {
            var Users = new List<UserObject>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var user = new 
                }
            }
        }
    }
}
