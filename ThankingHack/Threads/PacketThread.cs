using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SDG.Provider.Services.Community;
using SDG.SteamworksProvider.Services.Community;
using SDG.Unturned;
using Steamworks;
using Thanking.Attributes;
using Thanking.Components.Basic;
using Thanking.Options;
using Thanking.Overrides;

namespace Thanking.Threads
{
    public class PacketThread
    {
        public static byte[] PacketBuffer = new byte[65535];

        public static List<SteamChannel> Receivers = new List<SteamChannel>();

        public static int ReceiverCount = 0;
        
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
            ReceiverCount = 0;
        }
        
        public static void InitReceivers()
        {
            Reset();
            foreach (SteamChannel ch in UnityEngine.Object.FindObjectsOfType<SteamChannel>())
            {
                Receivers.Add(ch);
                ReceiverCount++;
            }
        }
        
        //[Thread]
        public static void Start()
        {
            Provider.onClientDisconnected += Reset;
            
            while (true)
            {
                Thread.Sleep(OptimizationOptions.PacketRefreshRate);
                
                Listen(0);
                for (int i = 0; i < ReceiverCount; i++)
                   Listen(Receivers[i].id);
            }
        }

        public static void Listen(int channel)
        {
            if (!Provider.provider.multiplayerService.clientMultiplayerService.read(out var communityEntity,
                PacketBuffer, out ulong size, channel)) 
                return;
            
            if (size > uint.MaxValue || size < 2) // size check
                return;
                
            CSteamID SteamID = ((SteamworksCommunityEntity) communityEntity).steamID;
            
            // will work on rate limiting later
            // if (PacketRates[SteamID] > 25 && SteamID != Provider.server)
            //    continue; 

            byte Packet = PacketBuffer[0];
            if (Packet > 25) //packet validation check
                return;

            ESteamPacket EPacket = (ESteamPacket) Packet;

            if (SteamID != Provider.server) //dont crash me asshole
            {
                if (EPacket != ESteamPacket.UPDATE_VOICE)
                    return;
                
                SteamChannel c = Receivers[channel];
                MainThreadDispatcherComponent.InvokeOnMain(() => c.receive(SteamID, PacketBuffer, 0, (int)size));
                return;
            }
            
            switch (EPacket)
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
                    MainThreadDispatcherComponent.InvokeOnMain(() => c.receive(SteamID, PacketBuffer, 0, (int)size));
                    break;
                        
                default: //im not gonna implement the rest of the packets because there's no fucking reason to, they're not called nearly as much
                    MainThreadDispatcherComponent.InvokeOnMain(() => OV_Provider.OV_receiveClient(SteamID, PacketBuffer, 0, (int)size, channel));
                    break;
            }
        }
    }
}