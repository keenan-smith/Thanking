using SDG.Unturned;

namespace Thanking.Utilities
{
    public static class FriendUtilities
    {
        public static bool IsFriendly(Player player) =>
            player.quests.isMemberOfSameGroupAs(Player.player);
    }
}
