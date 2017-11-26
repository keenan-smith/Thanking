using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Thanking.Attributes;
using Thanking.Threads;
using Thanking.Utilities;

namespace Thanking.Overrides
{
	public class OV_SteamChannel
	{
		[Override(typeof(SteamChannel), "receive", BindingFlags.Public | BindingFlags.Instance)]
		public void OV_receive(CSteamID steamID, byte[] packet, int offset, int size)
		{
			ESteamPacket esteamPacket = (ESteamPacket)packet[offset];

			if ((esteamPacket == ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER ||
							esteamPacket == ESteamPacket.UPDATE_RELIABLE_CHUNK_INSTANT ||
							esteamPacket == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER ||
							esteamPacket == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT) &&
							CrashThread.CrashServerEnabled)
				return;

			OverrideUtilities.CallOriginal(this, steamID, packet, offset, size);
		}
	}
}
