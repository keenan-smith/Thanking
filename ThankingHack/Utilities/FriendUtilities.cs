using SDG.Unturned;
using Steamworks;
using Thanking.Variables;

namespace Thanking.Utilities
{
    public static class FriendUtilities
    {
        public static bool IsFriendly(Player player) =>
            player.quests.isMemberOfSameGroupAs(Player.player) || FriendVariables.Friends.Contains(player);

		public static void AddFriend(Player Friend)
		{
			if (!FriendVariables.Friends.Contains(Friend))
				FriendVariables.Friends.Add(Friend);
		}

		public static void RemoveFriend(Player Friend)
		{
			if (FriendVariables.Friends.Contains(Friend))
				FriendVariables.Friends.Remove(Friend);
		}
	}
}
