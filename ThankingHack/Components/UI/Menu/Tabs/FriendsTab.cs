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
            Prefab.ScrollView(new Rect(0, 0, 466, 200), "Non-Friends on Server", ref FriendsScroll, () =>
            {
                for (int i = 0; i < Provider.clients.Count; i++)
                {
					Player player = Provider.clients[i].player;
					if (FriendUtilities.IsFriendly(player))
                        continue;

                    if (Provider.clients[i].player == Player.player)
                        continue;

                    if (Prefab.Button(
                        $"{Provider.clients[i].playerID.characterName}[{Provider.clients[i].playerID.playerName}]",
                        400))
						FriendUtilities.AddFriend(Provider.clients[i].player);

					GUILayout.Space(2);
                }
            });

			Prefab.ScrollView(new Rect(0, 0, 466, 200), "Friends on Server", ref FriendsScroll, () =>
			{
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					Player player = Provider.clients[i].player;
					
					if (!FriendUtilities.IsFriendly(player))
						continue;

					if (Prefab.Button(
						$"{Provider.clients[i].playerID.characterName}[{Provider.clients[i].playerID.playerName}]",
						400))
						FriendUtilities.RemoveFriend(Provider.clients[i].player);

					GUILayout.Space(2);
				}
			});
		}
    }
}