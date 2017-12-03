using System.Collections.Generic;
using System.Linq;
using SDG.Unturned;
using Steamworks;
using Thanking.Options.VisualOptions;

namespace Thanking.Managers.Main
{
    public class NetManager : PlayerCaller
    {
        public static IEnumerable<ulong> Heccers;
        
        public void sendHasHacks() => 
            channel.send("getHasHacks", ESteamCall.CLIENTS, ESteamPacket.UPDATE_RELIABLE_BUFFER, 1);

        [SteamCall]
        public void getHasHacks(CSteamID steamID, byte md)
        {
            if (md == 0 && ESPOptions.ShowHeccers || md == 1)
                channel.send("getConfirmHack", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER);
        }

        [SteamCall]
        public void getConfirmHack(CSteamID steamID) => 
            Heccers = Heccers?.Concat(new [] {steamID.m_SteamID}).ToArray() ?? new[] {steamID.m_SteamID};

        public NetManager()
        {
            Provider.onEnemyConnected += steamPlayer => sendHasHacks();

            Provider.onClientConnected += sendHasHacks;
        }
    }
}