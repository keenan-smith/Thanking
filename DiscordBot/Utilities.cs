using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot
{
    public static class Utilities
    {
        public static async Task<bool> IsAdministrator(this IUser User)
        {
            ulong[] IDs = (await Vars.Main.GetUserAsync(User.Id)).RoleIds.ToArray();

            for (Byte i = 0; i < IDs.Length; i++)
            {
                if (IDs[i] == 369662625790820352)
                    return true;
            }

            return false;
        }
        
        public static void Log(String Message, IChannel Channel)
        {
            Vars.Logging.SendMessageAsync($"{DateTime.Now} : [ {Channel.Name} ] {Message}");
        }
    }
}