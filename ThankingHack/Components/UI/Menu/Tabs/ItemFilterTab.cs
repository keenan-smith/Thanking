using SDG.Unturned;
using System.Collections.Generic;
using Thanking.Components.Basic;
using Thanking.Options;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
	public static class ItemFilterTab
	{
		public static void Tab()
		{
			if (Prefab.Toggle("Auto Item Pickup", ref ItemOptions.AutoItemPickup))
			{
				GUILayout.Space(2);
				GUILayout.Label("Delay: " + ItemOptions.ItemPickupDelay + "ms", Prefab._TextStyle);
				GUILayout.Space(2);
				ItemOptions.ItemPickupDelay = (int)Prefab.Slider(0, 3000, ItemOptions.ItemPickupDelay, 175);
				GUILayout.Space(5);

				ItemUtilities.DrawFilterTab(ItemOptions.ItemFilterOptions);
			}			
		}
	}
}
