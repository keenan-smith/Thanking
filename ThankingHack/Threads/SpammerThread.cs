using System.Threading;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options;
using Thanking.Utilities;

namespace Thanking.Threads
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