using System;
using System.Linq;
using SDG.Unturned;
using Steamworks;
using Thanking.Options;
using Thanking.Threads;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class PlayersTab
    {
	    public static Vector2 PlayersScroll;
        public static Player SelectedPlayer;
        public static string SearchString = "";

        public static SteamPlayer GetSteamPlayer(Player player)
        {
            foreach (var user in Provider.clients)
            {
                if (user.player == player)
                    return user;
            }

            return null;
        }

        public static void Tab()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            SearchString = Prefab.TextField(SearchString, "Search: ", 466);
            GUILayout.EndHorizontal();
            
            Prefab.ScrollView(new Rect(0, 25 + 5, 466, 250 - (25 + 10)), "Players", ref PlayersScroll, () =>
            {
                for (int i = 0; i < Provider.clients.Count; i++)
                {
					Player player = Provider.clients[i].player;
                    
                    if (player == OptimizationVariables.MainPlayer || player == null || (SearchString != "" && player.name.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) == -1))
                        continue;

                    bool Friend = FriendUtilities.IsFriendly(player);
                    bool Spectating = MiscOptions.SpectatedPlayer == player;
                    bool Selected = player == SelectedPlayer;
                    
                    string color = Friend ? "<color=#00ff00ff>" : "";
                    
                    if (Prefab.Button((Selected ? "<b>" : "") + (Spectating ? "<color=#0000ffff>[SPECTATING]</color> " : "") + color + $"{player.name}" + (Friend ? "</color>" : "") + (Selected ? "</b>" : ""), 400))
                        SelectedPlayer = player;

					GUILayout.Space(2);
                }
            });
            
            Prefab.MenuArea(new Rect(0, 250 + 10, 190, 175), "OPTIONS", () =>
            {
                if (SelectedPlayer == null)
                    return;

                CSteamID steamId = SelectedPlayer.channel.owner.playerID.steamID;
                
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                if (FriendUtilities.IsFriendly(SelectedPlayer))
                {
                    if (Prefab.Button("Remove Friend", 150))
                        FriendUtilities.RemoveFriend(SelectedPlayer);
                }
                else
                {
                    if (Prefab.Button("Add Friend", 150))
                        FriendUtilities.AddFriend(SelectedPlayer);
                }

                if (MiscOptions.SpectatedPlayer != SelectedPlayer)
                {
                    if (Prefab.Button("Spectate", 150))
                        MiscOptions.SpectatedPlayer = SelectedPlayer;
                }
                else if (MiscOptions.SpectatedPlayer != null && MiscOptions.SpectatedPlayer == SelectedPlayer)
                    if (Prefab.Button("End Spectating", 150))
                        MiscOptions.SpectatedPlayer = null;

                if (MiscOptions.CamPlayer != SelectedPlayer)
                {
                    if (Prefab.Button("Player Cam", 150))
                        MiscOptions.CamPlayer = SelectedPlayer;
                }
                else if (MiscOptions.CamPlayer != null && MiscOptions.CamPlayer == SelectedPlayer)
                    if (Prefab.Button("Remove Cam", 150))
                        MiscOptions.CamPlayer = null;


                if (MiscOptions.NoMovementVerification)
                    if (Prefab.Button("Teleport to player", 150))
                        OptimizationVariables.MainPlayer.transform.position = SelectedPlayer.transform.position;

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            });

            Prefab.MenuArea(new Rect(190 + 6, 250 + 10, 270, 175), "INFO", () =>
            {
                if (SelectedPlayer == null)
                    return;
                
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                
                GUILayout.Label("SteamID:");
                GUILayout.TextField(SelectedPlayer.channel.owner.playerID.steamID.ToString(), Prefab._TextStyle);
                GUILayout.Space(2);
                
                GUILayout.TextField("Closest Location: " + LocationUtilities.GetClosestLocation(SelectedPlayer.transform.position).name, Prefab._TextStyle);
                GUILayout.Label("Current Weapon: " + (SelectedPlayer.equipment.asset != null ? SelectedPlayer.equipment.asset.itemName : "Fists"), Prefab._TextStyle);
                GUILayout.Label("Current Vehicle: " + (SelectedPlayer.movement.getVehicle() != null ? SelectedPlayer.movement.getVehicle().asset.name : "No Vehicle"), Prefab._TextStyle);
                GUILayout.Label("Current Group Members: " + Provider.clients.Count(c => c.player != SelectedPlayer && c.player.quests.isMemberOfSameGroupAs(SelectedPlayer)), Prefab._TextStyle);
                
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            });
		}
    }
}