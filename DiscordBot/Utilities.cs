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
        
        public static void Log(String Message, IChannel Channel) =>
            Vars.Logging.SendMessageAsync($"{DateTime.Now} : [ {Channel.Name} ] {Message}");
        
        public static async Task<Action<EmbedFieldBuilder>> CreateEmbedField(String Name, object Value)
        {
            return F =>
            {
                F.IsInline = true;
                F.Name = Name;
                F.Value = Value;
            };
        }

        public static async Task<Embed> CreateEmbed(String Title, Color? Color, String ThumbnailURL, String URL, Action<EmbedFieldBuilder>[] Fields = null)
        {
            EmbedBuilder Embed = new EmbedBuilder
            {
                Title = Title,
                Color = Color,
                ThumbnailUrl = ThumbnailURL,
                Url = URL
            };

            if (Fields == null) return Embed.Build();
            
            for (int i = 0; i < Fields.Length; i++)
                Embed.AddField(Fields[i]);

            return Embed.Build();
        }
    }
}