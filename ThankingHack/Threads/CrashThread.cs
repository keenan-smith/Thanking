using SDG.Unturned;
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
					Provider.send(Provider.server, ESteamPacket.BATTLEYE, new byte[0], 0, 0);
		}

		public static void OnDisconnect() =>
			CrashServerEnabled = false;
	}
}
