using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ThankingProcessing.Models
{
    public class DatabaseUtilites
    {
        public async Task<UserObject> GetUser(string HWID)
        {
            UserObject User = new UserObject();
            
            using (AppDb db = new AppDb(Startup.database))
            {
                using (MySqlCommand Command = db.Connection.CreateCommand())
                {
                    Command.Parameters.AddWithValue("HWID", HWID);
                    Command.CommandText = "SELECT * FROM Users WHERE HWID = @HWID";

                    await db.Connection.OpenAsync();

                    using (MySqlDataReader Reader = Command.ExecuteReader())
                    {
                        while (await Reader.ReadAsync())
                        {
                            User.Hwid = HWID;
                            User.Ip = Reader.GetString(2);
                            User.IsBlacklisted = Reader.GetBoolean(3);
                            User.IsPremium = Reader.GetBoolean(4);
                            User.Steam64 = Reader.GetInt64(5);
                            User.SteamName = Reader.GetString(6);
                            User.LastUse = Reader.GetDateTime(7);
                        }
                    }
                }
            }

            return User;
        }

        public async Task<UserObject[]> ReadAllAsync(DbDataReader reader)
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
