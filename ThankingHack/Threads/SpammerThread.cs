using System.Threading;
using SDG.Unturned;
using Thinking.Attributes;
using Thinking.Options;
using Thinking.Utilities;

namespace Thinking.Threads
{
    public static class SpammerThread
    {
        [Thread]
        public static void Spammer()
        {
            #if DEBUG
            DebugUtilities.Log("Spammer Thread Started");
            #endif        
    
            while (true)
            {
                Thread.Sleep(MiscOptions.SpammerDelay);
                if (!MiscOptions.SpammerEnabled) 
                    continue;

                ChatManager.instance.channel.send("askChat", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER,
                    (byte) EChatMode.GLOBAL, MiscOptions.SpamText);
            }
        }
    }
}