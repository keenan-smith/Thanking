using System.Collections.Generic;
using System.Linq;
using SDG.Unturned;
using Steamworks;
using Thanking.Attributes;
using Thanking.Options.VisualOptions;

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
        public void sendHasHacks() => 
            channel.send("getHasHacks", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_BUFFER);

        // Listen for hack query, respond confirming hacks
        [SteamCall]
        public void getHasHacks(CSteamID steamID) =>
            channel.send("getConfirmHack", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER);

        // Listen for hack confirmation, add to Heccers if confirmed
        [SteamCall]
        public void getConfirmHack(CSteamID steamID) => 
            Heccers = Heccers?.Concat(new [] {steamID.m_SteamID}).ToArray() ?? new[] {steamID.m_SteamID};

        // Create a new instance of NetManager
        [Initializer]
        public static void Init() => Manager = new NetManager();

        public NetManager()
        {
            // Check all new players if they are a heccer
            Provider.onEnemyConnected += steamPlayer => sendHasHacks();

            // Check if any player in new servers are heccers
            Provider.onClientConnected += sendHasHacks;
        }
    }
}