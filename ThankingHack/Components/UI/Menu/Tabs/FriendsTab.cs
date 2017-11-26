using System;
using SDG.Unturned;
using Thanking.Options;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class FriendsTab
    {
        public static Vector2 FriendsScroll;
        
        public static void Tab()
        {
            Prefab.ScrollView(new Rect(0, 0, 466, 436), "Friends", ref FriendsScroll, () =>
            {
                for (int i = 0; i < Provider.clients.Count; i++)
                {
                    if (Provider.clients[i].player.quests.isMemberOfSameGroupAs(Player.player))
                        continue;

                    if (Prefab.Button(
                        $"{Provider.clients[i].playerID.characterName}[{Provider.clients[i].playerID.playerName}]",
                        400))
                    {
                        if (FriendUtilities.IsFriendly(Provider.clients[i].player))
                        {
                            FriendUtilities.RemoveFriend(Provider.clients[i].player);
                            continue;
                        }
                        
                        FriendUtilities.AddFriend(Provider.clients[i].player);
                    }
                    
                    GUILayout.Space(2);
                }
            });
        }
    }
}