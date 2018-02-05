using System;
using System.Reflection.Emit;
using SDG.Unturned;
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
        public static Player SelectedPlayer = null;
        
        public static void Tab()
        {
            Prefab.ScrollView(new Rect(0, 0, 466, 200), "Players", ref PlayersScroll, () =>
            {
                for (int i = 0; i < Provider.clients.Count; i++)
                {
					Player player = Provider.clients[i].player;
                    
                    if (Provider.clients[i].player == OptimizationVariables.MainPlayer)
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
            
            Prefab.MenuArea(new Rect(0, 200 + 10, 230, 200), "OPTIONS", () =>
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
                
                if (Prefab.Button("Spectate", 150))
                {
                    OptimizationVariables.MainPlayer.look.isOrbiting = true;
                    MiscOptions.SpectatedPlayer = SelectedPlayer;
                }

                if (MiscOptions.SpectatedPlayer != null)
                    if (Prefab.Button("End Spectating", 150))
                    {
                        MiscOptions.SpectatedPlayer = null;
                        OptimizationVariables.MainPlayer.look.isOrbiting = false;
                    }

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            });
            Prefab.MenuArea(new Rect(230 + 6, 200 + 10, 230, 200), "INFO", () =>
            {
                if (SelectedPlayer == null)
                    return;
                
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                GUILayout.Label("Current Weapon: " + SelectedPlayer.equipment.useable.name, Prefab._TextStyle);
                GUILayout.Label("Current Vehicle: " + (SelectedPlayer.movement.getVehicle() != null ? SelectedPlayer.movement.getVehicle().asset.name : "No Vehicle"), Prefab._TextStyle);
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            });
		}
    }
}