using System.Collections.Generic;
using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Overrides
{
	public static class OV_ItemManager
	{
		[Override(typeof(ItemManager), "getItemsInRadius", BindingFlags.Public | BindingFlags.Static)]
		public static void OV_getItemsInRadius(Vector3 center, float sqrRadius, List<RegionCoordinate> search,
			List<InteractableItem> result)
		{
			if (MiscOptions.IncreaseNearbyItemDistance)
				OverrideUtilities.CallOriginal(null, center, Mathf.Pow(MiscOptions.NearbyItemDistance, 2), search,
					result);

			else
				OverrideUtilities.CallOriginal(null, center, sqrRadius, search, result);
		}
		
	}
}