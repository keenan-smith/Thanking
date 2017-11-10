using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot
{
    public class Vars
    {
        public static DiscordSocketClient Client;
        
        public static Random Random;
        
        public static String Token = "Mzc3OTM1NjEyNzU1NDQzNzEy.DOULiQ.PdUxpwlKdh0qBY6JJ5LNu2MJAPk";

        public static IGuild Main;

        public static ITextChannel Developers, Logging;

        public static CommandService Commands;
    }
}