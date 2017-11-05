using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thanking.Utilities
{
    public static class FriendUtilities
    {
        public static bool IsFriendly(Player player)
        {
            return player.quests.isMemberOfSameGroupAs(Player.player);
        }
    }
}
