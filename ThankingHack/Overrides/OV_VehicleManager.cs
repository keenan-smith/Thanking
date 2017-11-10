using System.Reflection;
using SDG.Unturned;
using Steamworks;
using Thanking.Attributes;
using Thanking.Threads;
using Thanking.Utilities;

namespace Thanking.Overrides 
{ 
    public static class OV_VehicleManager 
    { 
        [Override(typeof(VehicleManager), "tellVehicles", BindingFlags.Public | BindingFlags.Instance)] 
        public static void OV_tellVehicles(CSteamID steamID) 
        {
            if (CrashThread.CrashServerEnabled)
                return; 
 
            OverrideUtilities.CallOriginal(VehicleManager.instance, steamID); 
        } 
    } 
} 