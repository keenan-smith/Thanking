using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Commands
{
    public class Message : ModuleBase
    {
        [Command("Message"), Summary("Messages designated user")]
        public async Task AnnounceCMD([Summary("User to message")] IUser User, [Summary("Message to send")] String Message)
            => await (await User.GetOrCreateDMChannelAsync()).SendMessageAsync(Message);
    }
}