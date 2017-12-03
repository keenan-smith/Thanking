using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;

namespace ThankingProcessing.Models
{
    public class UsersContext : DbContext
    {
        public DbSet<UserObject> users { get; set; }

        public UsersContext(DbContextOptions<UsersContext> options)
        : base(options)
        { }

    }

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
                            User.hwid = HWID;
                            User.ip = Reader.GetString(2);
                            User.blacklisted = Reader.GetBoolean(3);
                            User.premium = Reader.GetBoolean(4);
                            User.steam64 = Reader.GetInt64(5);
                            User.steamname = Reader.GetString(6);
                            User.lastuse = Reader.GetDateTime(7);
                        }
                    }
                }
            }

            return User;
        }
    }
}
