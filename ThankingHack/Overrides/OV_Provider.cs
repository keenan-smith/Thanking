using SDG.Unturned;
using Steamworks;
using System.Reflection;
using Thanking.Attributes;
using Thanking.Threads;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Overrides
{
	public static class OV_Provider
	{
		public static MethodInfo ReceiveClient;

		[Initializer]
		public static void OnInit() =>
			ReceiveClient = typeof(Provider).GetMethod("receiveClient", BindingFlags.NonPublic | BindingFlags.Static);

		[Override(typeof(Provider), "listenClient", BindingFlags.NonPublic | BindingFlags.Static)]
		public static void OV_listenClient()
		{
			while (PacketThread.PacketQueue.Count > 0)
			{
				Packet p = PacketThread.PacketQueue.Dequeue();

				SteamChannel channel = Provider.receivers[p.id];
				Debug.Log(channel.name);

				ReceiveClient.Invoke(null, new object[] { p.steamid, p.packet, 0, p.size, p.id });
			}
		}
	}
}
