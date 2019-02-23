using SDG.Unturned;
using Steamworks;
using System.Diagnostics;
using System.Reflection;
using Thinking.Attributes;
using Thinking.Threads;
using Thinking.Utilities;
using UnityEngine;

namespace Thinking.Overrides
{
	public static class OV_Provider
	{
		public static bool IsConnected;
	
		[Override(typeof(Provider), "receiveClient", BindingFlags.NonPublic | BindingFlags.Static)]
		public static void OV_receiveClient(CSteamID steamID, byte[] packet, int offset, int size, int channel)
		{
			if (!IsConnected)
				IsConnected = true;
			
			if (ServerCrashThread.ServerCrashEnabled && packet[0] == (byte)ESteamPacket.WORKSHOP)
				return;
			
			if (steamID != Provider.server && packet[0] != (byte)ESteamPacket.UPDATE_VOICE)
				return;

            OverrideUtilities.CallOriginal(null, steamID, packet, offset, size, channel); 
        }
		
		[Override(typeof(Provider), "OnApplicationQuit", BindingFlags.NonPublic | BindingFlags.Instance)]
		public static void OV_OnApplicationQuit() =>
			Process.GetCurrentProcess().Kill();
	}
}