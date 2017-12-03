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

    public static class DBUtils
    {
        public static async Task<UserObject> GetUser(UsersContext context, string HWID)
        {
            var Users = await (from b in context.users
                               where b.hwid == HWID
                               select b).ToListAsync();

            return (Users.Count<1?null:Users[0]);
        }

        public static async Task AddUser(UsersContext context, UserObject user)
        {
            context.users.Add(user);
            await context.SaveChangesAsync();
        }
    }
}
