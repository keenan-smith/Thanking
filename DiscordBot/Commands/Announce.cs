using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordBot.Commands
{
    public class Announce : ModuleBase
    {
        [Command("Announce"), Summary("Announces a message to designated channel")]
        public async Task AnnounceCMD([Summary("ID of channel to announce to")] ulong ChannelID, [Summary("Message to announce")] String Message)
            => (await Vars.Main.GetTextChannelAsync(ChannelID))?.SendMessageAsync(Message);
    }
}