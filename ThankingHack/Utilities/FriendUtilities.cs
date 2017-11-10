﻿using SDG.Unturned;
using Thanking.Options;

namespace Thanking.Utilities
{
    public static class FriendUtilities
    {
		public static bool IsFriendly(Player player) =>
			player.quests.isMemberOfSameGroupAs(Player.player) || MiscOptions.Friends.Contains(player.channel.owner.playerID.steamID.m_SteamID);

		public static void AddFriend(Player Friend)
		{
			ulong steamid = Friend.channel.owner.playerID.steamID.m_SteamID;
			if (!MiscOptions.Friends.Contains(steamid))
				MiscOptions.Friends.Add(steamid);
		}

		public static void RemoveFriend(Player Friend)
		{
			ulong steamid = Friend.channel.owner.playerID.steamID.m_SteamID;
			if (MiscOptions.Friends.Contains(steamid))
				MiscOptions.Friends.Remove(steamid);
		}
	}
}
