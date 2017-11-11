using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DiscordBot
{
    class Program
    {
        static async Task Main(String[] Args)
        {
            Vars.Random = new Random();
            
            Vars.Client = new DiscordSocketClient();
            await Vars.Client.LoginAsync(TokenType.Bot, Vars.Token);
            await Vars.Client.StartAsync();

            Vars.Client.Ready += OnStart;

            await Task.Delay(-1);
        }

        private static async Task OnStart()
        {
            Vars.Main = Vars.Client.GetGuild(369662339433234442);
            Vars.Developers = await Vars.Main.GetTextChannelAsync(369663671938449408);
            Vars.Logging = await Vars.Main.GetTextChannelAsync(378420036739530753);
            
            await MessageHandler.InitCMDs();
            await MessageHandler.InitMessageLogger();
            await MessageHandler.InitResponses();
        }
    }
}