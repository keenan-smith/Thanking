using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Misc.Enums;
using Thanking.Options;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Overrides
{
	public static class OV_DamageTool
	{
		public static OverrideType OVType = OverrideType.None;
		
	    [Override(typeof(DamageTool), "raycast", BindingFlags.Public | BindingFlags.Static)] 
		public static RaycastInfo OV_raycast(Ray ray, float range, int mask)
	    {
		    switch (OVType)
		    {
			    case OverrideType.Extended:
				    return RaycastUtilities.GenerateOriginalRaycast(ray, MiscOptions.MeleeRangeExtension, mask);

			    case OverrideType.SilentAim: {
				    return RaycastUtilities.GenerateRaycast(out RaycastInfo ri)
					    ? ri
					    : RaycastUtilities.GenerateOriginalRaycast(ray, range, mask);
			    }

			    case OverrideType.SilentAimMelee: {
				    return RaycastUtilities.GenerateRaycast(out RaycastInfo ri)
					    ? ri
					    : RaycastUtilities.GenerateOriginalRaycast(ray, MiscOptions.MeleeRangeExtension, mask);
			    }

			    case OverrideType.PlayerHit: 
				    for (int i = 0; i < Provider.clients.Count; i++)
				    {
					    if (VectorUtilities.GetDistance(Player.player.transform.position,
						        Provider.clients[i].player.transform.position) > 15.5)
						    continue;
                        
					    RaycastUtilities.GenerateRaycast(out RaycastInfo ri2);
					    return ri2;
				    }

				    break;
		    }
		    
			return RaycastUtilities.GenerateOriginalRaycast(ray, range, mask);
	    }
	}
}