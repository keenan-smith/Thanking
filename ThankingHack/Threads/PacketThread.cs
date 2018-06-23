using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using SDG.Provider.Services.Community;
using SDG.SteamworksProvider.Services.Community;
using SDG.Unturned;
using Steamworks;
using Thanking.Attributes;
using Thanking.Components.Basic;
using Thanking.Options;
using Thanking.Overrides;
using Thanking.Utilities;
using UnityEngine;
using Types = SDG.Unturned.Types;

namespace Thanking.Threads
{
    public class PacketThread
    {
        private static readonly byte[] PacketBuffer = new byte[Block.BUFFER_SIZE];
        private static readonly List<SteamChannel> Receivers = new List<SteamChannel>();
        private static readonly int ESTEAMPACKET_MAX_VALUE = Enum.GetValues(typeof(ESteamPacket)).Cast<int>().Max();

        //public static Dictionary<CSteamID, int> PacketRates = new Dictionary<CSteamID, int>();

        /*[Thread]
        public static void StartRlCheck()
        {
            while (true)
            {
                Thread.Sleep(1000);

                if (!OV_Provider.IsConnected) 
                    PacketRates.Clear();

                foreach (CSteamID k in PacketRates.Keys.ToList())
                {
                    if (k != Provider.server && Provider.clients.All(c => c.playerID.steamID != k))
                        PacketRates.Remove(k);
                        
                    PacketRates[k] = 0;
                }

            }
        }
        */

        public static void Reset()
        {
            Receivers.Clear();
        }
        
        public static void InitReceivers()
        {
            Reset();
            foreach (SteamChannel ch in UnityEngine.Object.FindObjectsOfType<SteamChannel>())
                Receivers.Add(ch);

            DebugUtilities.Log(Receivers.Count + " receivers loaded");
        }
        
        [Thread]
        public static void Start()
        {
            Provider.onClientDisconnected += Reset;
            
            while (true)
            {
                Thread.Sleep(OptimizationOptions.PacketRefreshRate);
                
                Listen(0);

                for (int i = 0; i < Receivers.Count; i++)
                   Listen(Receivers[i].id);
            }
        }

        public static void Listen(int channel)
        {
            while (Provider.provider.multiplayerService.clientMultiplayerService.read(out var communityEntity,
                PacketBuffer, out ulong size, channel))
            {
                byte packet = PacketBuffer[0];
                ESteamPacket packetType = (ESteamPacket)packet;

                if (packetType != ESteamPacket.PING_RESPONSE)
                    DebugUtilities.Log("Received packet: " + packetType);

                if (size > uint.MaxValue || (size < 2 && packetType != ESteamPacket.VERIFY)) // size check
                    return;

                CSteamID steamId = ((SteamworksCommunityEntity)communityEntity).steamID;

                // will work on rate limiting later
                // if (PacketRates[SteamID] > 25 && SteamID != Provider.server)
                //    continue; 

                if (packet > ESTEAMPACKET_MAX_VALUE) //packet validation check
                    return;

                if (steamId != Provider.server) //dont crash me asshole
                {
                    if (packetType != ESteamPacket.UPDATE_VOICE)
                        return;

                    SteamChannel c = Receivers.First(r => r.id == channel);
                    MainThreadDispatcherComponent.InvokeOnMain(() => c.receive(steamId, PacketBuffer, 0, (int)size));
                    return;
                }

                // simply does not work without copying the buffer
                // something something thread safety
                var copiedBuffer = new byte[Block.BUFFER_SIZE];
                PacketBuffer.CopyTo(copiedBuffer, 0);

                switch (packetType)
                {
                    case ESteamPacket.UPDATE_RELIABLE_BUFFER:
                    case ESteamPacket.UPDATE_UNRELIABLE_BUFFER:
                    case ESteamPacket.UPDATE_RELIABLE_INSTANT:
                    case ESteamPacket.UPDATE_UNRELIABLE_INSTANT:
                    case ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER:
                    case ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER:
                    case ESteamPacket.UPDATE_RELIABLE_CHUNK_INSTANT:
                    case ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT:
                        SteamChannel c = Receivers.First(r => r.id == channel);
                        MainThreadDispatcherComponent.InvokeOnMain(() => c.receive(steamId, copiedBuffer, 0, (int)size));
                        break;
                    
                    default: //im not gonna implement the rest of the packets because there's no fucking reason to, they're not called nearly as much
                        MainThreadDispatcherComponent.InvokeOnMain(() => OV_Provider.OV_receiveClient(steamId, copiedBuffer, 0, (int)size, channel));
                        break;
                }
            }
        }
    }
}