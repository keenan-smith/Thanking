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
	public static class OV_BarricadeManager
	{
		[Override(typeof(BarricadeManager), "tellBarricades", BindingFlags.Public | BindingFlags.Instance)]
		public static void OV_tellBarricades(CSteamID steamID)
		{
			if (!Level.isLoadingBarricades)
				return;

			OverrideUtilities.CallOriginal(BarricadeManager.instance, steamID);
		}
	}
}