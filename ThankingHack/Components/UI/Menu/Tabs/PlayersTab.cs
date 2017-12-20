using System;
using System.Reflection.Emit;
using SDG.Unturned;
using Thanking.Options;
using Thanking.Threads;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class PlayersTab
    {
	    public static Vector2 PlayersScroll;
        public static Player SelectedPlayer = null;
        
        public static void Tab()
        {
            Prefab.ScrollView(new Rect(0, 0, 466, 200), "Players", ref PlayersScroll, () =>
            {
                for (int i = 0; i < Provider.clients.Count; i++)
                {
					Player player = Provider.clients[i].player;
                    
                    if (Provider.clients[i].player == Player.player)
                        continue;

                    bool Friend = FriendUtilities.IsFriendly(player);
                    bool Crash = PlayerCrashThread.CrashTarget == player.channel.owner.playerID.steamID;
                    bool Selected = player == SelectedPlayer;
                    
                    string color = 
                        Crash ? "<color=#ff0000ff>"
                        : (Friend ? "<color=#00ff00ff>" : "");
                    
                    if (Prefab.Button((Selected ? "<b>" : "") + color + $"{Provider.clients[i].player.name}" + (Friend || Crash ? "</color>" : "") + (Selected ? "</b>" : ""), 400))
                        SelectedPlayer = player;

					GUILayout.Space(2);
                }
            });
            
            Prefab.MenuArea(new Rect(0, 200 + 10, 466, 200), "OPTIONS", () =>
            {
                if (SelectedPlayer == null)
                    return;
                
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                bool Friend = FriendUtilities.IsFriendly(SelectedPlayer);
                if (Friend)
                {
                    if (Prefab.Button("Remove Friend", 150))
                        FriendUtilities.RemoveFriend(SelectedPlayer);
                }
                else
                {
                    if (Prefab.Button("Add Friend", 150))
                        FriendUtilities.AddFriend(SelectedPlayer);
                }

                if (Prefab.Button("Crash Player", 150))
                {
                    PlayerCrashThread.CrashTarget = SelectedPlayer.channel.owner.playerID.steamID;
                    PlayerCrashThread.PlayerCrashEnabled = true;
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            });
		}
    }
}