using System.Diagnostics;
using System.Reflection;
using SDG.Unturned;
using Steamworks;
using Thanking.Attributes;
using Thanking.Threads;
using Thanking.Utilities;

namespace Thanking.Overrides
{
	public static class OV_Provider
	{
		public static bool IsConnected;
	
		[Override(typeof(Provider), "receiveClient", BindingFlags.NonPublic | BindingFlags.Static)]
		public static void OV_receiveClient(CSteamID steamID, byte[] packet, int offset, int size, int channel)
		{
			if (!IsConnected)
				IsConnected = true;
			
			if (steamID != Provider.server && packet[0] != (byte)ESteamPacket.UPDATE_VOICE)
				return;

            OverrideUtilities.CallOriginal(null, steamID, packet, offset, size, channel); 
        }
		
		[Override(typeof(Provider), "OnApplicationQuit", BindingFlags.NonPublic | BindingFlags.Instance)]
		public static void OV_OnApplicationQuit() =>
			Process.GetCurrentProcess().Kill();
	}
}