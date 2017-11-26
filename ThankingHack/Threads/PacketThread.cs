using SDG.Provider.Services.Community;
using SDG.SteamworksProvider.Services.Community;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
using Thanking.Options;
using UnityEngine;

namespace Thanking.Threads
{
	public class Packet
	{
		public CSteamID steamid;
		public byte[] packet;
		public int size;
		public int id;
	}
	public static class PacketThread
	{
		public static Queue<Packet> PacketQueue = new Queue<Packet>();
		[Thread]
		public static void Listen()
		{
			byte[] buffer = new byte[65535];
			while (true)
			{
				for (int i = 0; i < Provider.receivers.Count; i++)
				{
					while (Provider.provider.multiplayerService.clientMultiplayerService.read(out ICommunityEntity communityEntity, buffer, out ulong size, i))
					{
						ESteamPacket esteamPacket = (ESteamPacket)buffer[0];
						if ((esteamPacket == ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER ||
							esteamPacket == ESteamPacket.UPDATE_RELIABLE_CHUNK_INSTANT ||
							esteamPacket == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER ||
							esteamPacket == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT) &&
							CrashThread.CrashServerEnabled)
							continue;

						PacketQueue.Enqueue(new Packet()
						{
							steamid = ((SteamworksCommunityEntity)communityEntity).steamID,
							packet = (byte[])buffer.Clone(),
							size = (int)size,
							id = i
						});

						buffer = new byte[65535];
					}
				}
			}
		}
	}
}
