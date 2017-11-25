using SDG.Unturned;
using Steamworks;
using System.Reflection;
using Thanking.Attributes;
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
			if (esteamPacket == ESteamPacket.UPDATE_RELIABLE_CHUNK_INSTANT && CrashThread.CrashServerEnabled)
				return;

			OverrideUtilities.CallOriginal(null, steamID, packet, offset, size, channel);
		}
	}
}
