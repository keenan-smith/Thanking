using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Thanking.Attributes;
using Thanking.Managers.Submanagers;
using Thanking.Threads;
using Thanking.Utilities;

namespace Thanking.Overrides
{
	public static class OV_Provider
	{
		[Override(typeof(Provider), "receiveClient", BindingFlags.NonPublic | BindingFlags.Static)]
		public static void OV_receiveClient(CSteamID steamID, byte[] packet, int offset, int size, int channel)
		{
			ESteamPacket esteamPacket = (ESteamPacket)packet[offset];
			if (esteamPacket.ToString().ToLower().Contains("chunk") && CrashThread.CrashServerEnabled)
				return;

			OverrideUtilities.CallOriginal(null, steamID, packet, offset, size, channel);
		}
	}
}
