using System.Reflection;
using SDG.Unturned;
using Steamworks;
using Thanking.Attributes;
using Thanking.Threads;
using Thanking.Utilities;

namespace Thanking.Overrides 
{ 
    public static class OV_BarricadeManager 
    { 
        [Override(typeof(BarricadeManager), "tellBarricades", BindingFlags.Public | BindingFlags.Instance)] 
        public static void OV_tellBarricades(CSteamID steamID)
        {
            if (CrashThread.CrashServerEnabled)
                return; 
 
            OverrideUtilities.CallOriginal(BarricadeManager.instance, steamID); 
        } 
    } 
}