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
			#if DEBUG
			DebugUtilities.Log("Crash Thread Started");
			#endif
			
			Provider.onClientDisconnected += OnDisconnect;
			
			while (true)
				if (CrashServerEnabled)
					VehicleManager.instance.channel.send("askVehicles", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER);
		}

		public static void OnDisconnect() =>
			CrashServerEnabled = false;
	}
}
