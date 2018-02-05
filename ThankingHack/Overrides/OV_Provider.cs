using SDG.Unturned;
using Steamworks;
using System.Diagnostics;
using System.Reflection;
using Thanking.Attributes;
using Thanking.Threads;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Overrides
{
	public static class OV_Provider
	{
		[Override(typeof(Provider), "receiveClient", BindingFlags.NonPublic | BindingFlags.Static)]
		public static void OV_receiveClient(CSteamID steamID, byte[] packet, int offset, int size, int channel)
		{
			if (steamID != Provider.server && PlayerCrashThread.PlayerCrashEnabled)
				return;
			
            OverrideUtilities.CallOriginal(null, steamID, packet, offset, size, channel);
        }

		[Override(typeof(Provider), "OnApplicationQuit", BindingFlags.Instance | BindingFlags.NonPublic)]
		public static void OV_OnApplicationQuit() =>
			Process.GetCurrentProcess().Kill();
	}
}
