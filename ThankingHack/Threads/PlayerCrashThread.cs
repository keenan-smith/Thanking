using System.Linq;
using SDG.Unturned;
using Steamworks;    
using Thanking.Attributes;
using Thanking.Utilities;

namespace Thanking.Threads
{
    public static class PlayerCrashThread
    {
        public static bool PlayerCrashEnabled;
        public static CSteamID CrashTarget;
        
        [Thread]
        public static void Start()
        {
            #if DEBUG
			DebugUtilities.Log("Player Crash Thread Started");
			#endif
			
            Provider.onClientDisconnected += OnDisconnect;
            Provider.onEnemyDisconnected += OnEnemyDisconnect;
			
            while (true)
                if (PlayerCrashEnabled)
                    VehicleManager.instance.channel.send("askVehicles", CrashTarget, ESteamPacket.UPDATE_RELIABLE_INSTANT);
        }

        public static void OnDisconnect() =>
            PlayerCrashEnabled = false;

        public static void OnEnemyDisconnect(SteamPlayer player)
        {
            if (CrashTarget == player.playerID.steamID)
                PlayerCrashEnabled = false;
        }
            
    }
}
