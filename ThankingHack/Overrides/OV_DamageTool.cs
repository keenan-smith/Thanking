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
		Regular,
		SilentAim
	}
	public static class OV_DamageTool
	{
		public static OverrideType OVType = OverrideType.None;

	    [Override(typeof(DamageTool), "raycast", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)] 
		public static RaycastInfo OV_Raycast(Ray ray, float range, int mask)
		{
			switch (OVType)
			{
				case OverrideType.Regular:
					return (RaycastInfo)OverrideUtilities.CallOriginal(null, ray, MiscOptions.MeleeRangeExtension, mask);

				case OverrideType.SilentAim:
					return RaycastUtilities.GenerateRaycast();
			}

			return (RaycastInfo)OverrideUtilities.CallOriginal(null, ray, range, mask);
		}
	}
}