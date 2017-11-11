using System;
using System.Threading.Tasks;
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
        
        public static async Task<Embed> Features()
        {
            Action<EmbedFieldBuilder>[] Fields = 
            {
                await Utilities.CreateEmbedField("Misc",
                    "Interact through walls\nAuto item pickup\nCustom item filters\nLarge playermodels\nVehicle flight\nBuild anywhere\nSet time\nNight vision\nFake crosshair\nItem spam\nSlow fall"),
                await Utilities.CreateEmbedField("ESP",
                    "Line to players\nChams\nGlow\nPlayers\nStorage\nItems\nVehicles\nGenerators\nClaim flags\nBeds\nTurrets"),
                await Utilities.CreateEmbedField("Weapons",
                    "No recoil\nNo sway\nNo spread\nZoom\nZoom w/out scope\nSkin changer\nAuto reload"),
                await Utilities.CreateEmbedField("Aimbot",
                    "Random limb\nAlways headshots\nSilent aim\nTriggerbot\nShoot through walls"),
                await Utilities.CreateEmbedField("Anti-Detection", "Show connected admins\nAnti-spy"),
                await Utilities.CreateEmbedField("Extras", "Custom menu\nCustomizable\nConstantly updated")
            };

            return await Utilities.CreateEmbed("Features", Color.Green, "https://i.imgur.com/HU6P9LA.png", "", Fields);
        }
    }
}