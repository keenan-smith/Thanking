﻿using SDG.Unturned;

namespace Thanking.Utilities
{
	public static class PacketUtilities
	{
		public static bool IsChunk(ESteamPacket packet) =>
			packet.ToString().Contains("CHUNK");

		public static bool IsUpdate(ESteamPacket packet) =>
			packet.ToString().Contains("UPDATE");
	}
}
