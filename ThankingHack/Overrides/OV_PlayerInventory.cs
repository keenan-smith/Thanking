using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Coroutines;
using UnityEngine;
using Thanking.Options;
using Thanking.Utilities;
using Thanking.Variables;

namespace Thanking.Overrides
{
    public class OV_PlayerInventory : MonoBehaviour
    {
	    public static MethodInfo RefreshStaticMap;
		
	    public static bool WasGPSEnabled;
		public static bool WasCompassEnabled;
        
	    [OnSpy]
	    public static void Disable()
	    {
		    WasGPSEnabled = MiscOptions.GPS;
		    WasCompassEnabled = MiscOptions.Compass;

		    MiscOptions.GPS = false;
		    MiscOptions.Compass = false;
		    
		    PlayerLifeUI.updateCompass();
		    RefreshStaticMap.Invoke(OptimizationVariables.MainPlayer.inventory, new object[] {GetMap()});
	    }
        
	    [OffSpy]
	    public static void Enable()
	    {
		    MiscOptions.GPS = WasGPSEnabled;
		    MiscOptions.Compass = WasCompassEnabled;
		    
		    PlayerLifeUI.updateCompass();
		    RefreshStaticMap.Invoke(OptimizationVariables.MainPlayer.inventory, new object[] {GetMap()});
	    }


	    [Initializer]
	    public static void Init() =>
		    RefreshStaticMap =
			    typeof(PlayerDashboardInformationUI).GetMethod("refreshStaticMap", BindingFlags.NonPublic | BindingFlags.Static);
	    
        [Override(typeof(PlayerInventory), "has", BindingFlags.Public | BindingFlags.Instance)]
        public InventorySearch has(ushort id)
        {
            if (DrawUtilities.ShouldRun())
            {
                if (id == 1176 && MiscOptions.GPS) return new InventorySearch(0, new ItemJar(new Item(1176, false)));
                if (id == 1508 && MiscOptions.Compass) return new InventorySearch(0, new ItemJar(new Item(1508, false)));
            }

	        return (InventorySearch) OverrideUtilities.CallOriginal(OptimizationVariables.MainPlayer.inventory, id);
        }

	    public static int GetMap()
	    {
		    PlayerInventory plrInv = OptimizationVariables.MainPlayer.inventory;

		    if (MiscOptions.GPS || plrInv.has(1176) != null)
			    return 1;

		    return 0;
	    }
    }
}