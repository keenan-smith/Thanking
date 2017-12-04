using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using SDG.Unturned;
using Steamworks;
using Thanking.Attributes;
using Thanking.Options.VisualOptions;
using Thanking.Utilities;

namespace Thanking.Managers.Main
{
    // Manager of all steam calls/packets
    public class NetManager : PlayerCaller
    {
        // List of all heccers playing with you
        public static IEnumerable<ulong> Heccers;

        // Instance to be used outside of NetManager
        public static NetManager Manager;

        // Ask client if they have hacks
        public void sendHasHacks()
        {
            DebugUtilities.Log("Sending has hacks");
            
            channel.send("getHasHacks", ESteamCall.PEERS, ESteamPacket.UPDATE_RELIABLE_BUFFER);
        }

        // Listen for hack query, respond confirming hacks
        [SteamCall]
        public void getHasHacks(CSteamID steamID)
        {
            DebugUtilities.Log("Getting has hacks");
            
            channel.send("getConfirmHack", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER);
        }

        // Listen for hack confirmation, add to Heccers if confirmed
        [SteamCall]
        public void getConfirmHack(CSteamID steamID)
        {
            DebugUtilities.Log("Confirming has hacks");
            
            Heccers = Heccers?.Concat(new [] {steamID.m_SteamID}).ToArray() ?? new[] {steamID.m_SteamID};
        }

        // Create a new instance of NetManager
        [Initializer]
        public static void Init() =>
            Provider.onClientConnected += OnConnect;

        public static void OnConnect() =>
            Manager = Player.player.gameObject.AddComponent<NetManager>();
        
        public void Start()
        {
            sendHasHacks();
            Provider.onEnemyConnected += steamPlayer => sendHasHacks();
        }
    }
}