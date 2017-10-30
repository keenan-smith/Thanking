using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Components.Basic;
using Thanking.Coroutines;
using UnityEngine;

namespace Thanking.Managers.Main
{
	public static class PacketManager
	{
		public static void Init()
		{

		}

		public static void OnReceivePacket(SteamChannel channel, CSteamID steamID, byte[] packet, int offset, int size)
		{
			if (!channel.checkServer(steamID))
				return;

			ESteamPacket esteamPacket = (ESteamPacket)packet[offset];
			int num = packet[offset + 1];

			if (esteamPacket == ESteamPacket.UPDATE_VOICE)
				return;

			object[] objects = SteamPacker.getObjects(steamID, offset, 2, packet, channel.calls[num].types);
			string call = channel.calls[num].method.Name;
			
			//do shit here xd
		}
	}
}
