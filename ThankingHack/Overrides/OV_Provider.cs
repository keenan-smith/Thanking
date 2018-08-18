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
		public static bool IsConnected;
	
		[Override(typeof(Provider), "receiveClient", BindingFlags.NonPublic | BindingFlags.Static)]
		public static void OV_receiveClient(CSteamID steamID, byte[] packet, int offset, int size, int channel)
		{
			if (!IsConnected)
				IsConnected = true;
			
			if (steamID == Provider.server && ServerCrashThread.ServerCrashEnabled)
				return;

			if ((ESteamPacket) packet[0] == ESteamPacket.PING_RESPONSE && steamID == Provider.server)
			{
				OverrideUtilities.CallOriginal(null, steamID, packet, offset, size, channel);
				return;
			}
			
			if (steamID == PlayerCrashThread.CrashTarget && PlayerCrashThread.PlayerCrashEnabled)
				return;

            OverrideUtilities.CallOriginal(null, steamID, packet, offset, size, channel); 
        }

		//[Override(typeof(Provider), "connect", BindingFlags.Public | BindingFlags.Static)]
		public static void OV_connect(SteamServerInfo info, string password)
		{
			PacketThread.InitReceivers();
			OverrideUtilities.CallOriginal(null, info, password);
		}
		
		//[Override(typeof(Provider), "listenClient", BindingFlags.NonPublic | BindingFlags.Static)]
		public static void OV_listenClient(int channel) { }
		
		[Override(typeof(Provider), "OnApplicationQuit", BindingFlags.NonPublic | BindingFlags.Instance)]
		public static void OV_OnApplicationQuit() =>
			Process.GetCurrentProcess().Kill();
	}
}