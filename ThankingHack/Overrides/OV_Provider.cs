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
        /*
		public static MethodInfo ReceiveClient;

		[Initializer]
		public static void OnInit() =>
			ReceiveClient = typeof(Provider).GetMethod("receiveClient", BindingFlags.NonPublic | BindingFlags.Static);

		[Override(typeof(Provider), "listenClient", BindingFlags.NonPublic | BindingFlags.Static)]
		public static void OV_listenClient(int channel)
		{
			while (PacketThread.PacketQueue.Count > 0)
			{
				Packet p = PacketThread.PacketQueue.Dequeue();
				ReceiveClient.Invoke(null, new object[] { p.steamid, p.packet, 0, p.size, p.id });
			}
		}
		*/

		[Override(typeof(Provider), "receiveClient", BindingFlags.NonPublic | BindingFlags.Static)]
		public static void OV_receiveClient(CSteamID steamID, byte[] packet, int offset, int size, int channel)
		{
			ESteamPacket esteamPacket = (ESteamPacket)packet[offset];
			if ((esteamPacket == ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER ||
							esteamPacket == ESteamPacket.UPDATE_RELIABLE_CHUNK_INSTANT ||
							esteamPacket == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER ||
							esteamPacket == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT) &&
							CrashThread.CrashServerEnabled)
				return;

			if (steamID != Provider.server && PlayerCrashThread.PlayerCrashEnabled)
				return;
			
            OverrideUtilities.CallOriginal(null, steamID, packet, offset, size, channel);
        }

        [Override(typeof(Provider), "OnApplicationQuit", BindingFlags.Instance | BindingFlags.NonPublic)]
        public static void RageQuit()
        {
	        ProcessStartInfo rq = new ProcessStartInfo
	        {
		        FileName = "cmd.exe",
		        Arguments = "/c Taskkill /IM Unturned.exe /F"
	        };
	        Process.Start(rq);
        }
    }
}
