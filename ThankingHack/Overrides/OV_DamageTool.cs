using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Overrides
{
	public enum OverrideType
	{
		None,
		ExtendedMelee,
		SilentAim
	}
	public static class OV_DamageTool
	{
		public static OverrideType OVType = OverrideType.None;
	
	    [Override(typeof(DamageTool), "raycast", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)] 
		public static RaycastInfo OV_raycast(Ray ray, float range, int mask)
	    {
		    if (OVType == OverrideType.ExtendedMelee)
			    return RaycastUtilities.GenerateOriginalRaycast(ray, MiscOptions.MeleeRangeExtension, mask);

		    if (OVType != OverrideType.SilentAim) 
			    return RaycastUtilities.GenerateOriginalRaycast(ray, range, mask);
		    
		    return RaycastUtilities.GenerateRaycast(out RaycastInfo ri) ? ri : RaycastUtilities.GenerateOriginalRaycast(ray, range, mask);
	    }
	}
}