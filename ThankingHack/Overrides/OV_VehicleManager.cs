using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Thanking.Attributes;
using Thanking.Managers.Submanagers;
using Thanking.Options;
using Thanking.Utilities;

namespace Thanking.Overrides
{
	public static class OV_VehicleManager
	{
		[Override(typeof(VehicleManager), "tellVehicles", BindingFlags.Public | BindingFlags.Instance)]
		public static void OV_tellVehicles(CSteamID steamID)
		{
			if (!Level.isLoadingVehicles)
				return;

			OverrideUtilities.CallOriginal(VehicleManager.instance, steamID);
		}
	}
}
