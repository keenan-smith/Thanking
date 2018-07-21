using System;
using System.Linq;
using System.Reflection.Emit;
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
        public static Player SelectedPlayer = null;
        
        public static void Tab()
        {
            Prefab.ScrollView(new Rect(0, 0, 466, 250), "Players", ref PlayersScroll, () =>
            {
                for (int i = 0; i < Provider.clients.Count; i++)
                {
					Player player = Provider.clients[i].player;
                    
                    if (player == OptimizationVariables.MainPlayer || player == null)
                        continue;

                    bool Friend = FriendUtilities.IsFriendly(player);
                    bool Spectating = MiscOptions.SpectatedPlayer == player;
                    bool Crash = PlayerCrashThread.CrashTarget == player.channel.owner.playerID.steamID;
                    bool Selected = player == SelectedPlayer;
                    
                    string color = 
                        Crash ? "<color=#ff0000ff>"
                        : (Friend ? "<color=#00ff00ff>" : "");
                    
                    if (Prefab.Button((Selected ? "<b>" : "") + (Spectating ? "<color=#0000ffff>[SPECTATING]</color> " : "") + color + $"{player.name}" + (Friend || Crash ? "</color>" : "") + (Selected ? "</b>" : ""), 400))
                        SelectedPlayer = player;

					GUILayout.Space(2);
                }
            });
            
            Prefab.MenuArea(new Rect(0, 250 + 10, 190, 150), "OPTIONS", () =>
            {
                if (SelectedPlayer == null)
                    return;
                
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

                if (Prefab.Button("Crash Player", 150))
                {
                    PlayerCrashThread.CrashTarget = SelectedPlayer.channel.owner.playerID.steamID;
                    PlayerCrashThread.PlayerCrashEnabled = true;
                }

                if (PlayerCrashThread.PlayerCrashEnabled && PlayerCrashThread.CrashTarget == SelectedPlayer.channel.owner.playerID.steamID)
                    if (Prefab.Button("Stop Crashing", 150))
                    {
                        PlayerCrashThread.CrashTarget = CSteamID.Nil;
                        PlayerCrashThread.PlayerCrashEnabled = false;
                    }

                if (Prefab.Button("Spectate", 150))
                    MiscOptions.SpectatedPlayer = SelectedPlayer;

                if (MiscOptions.SpectatedPlayer != null && MiscOptions.SpectatedPlayer == SelectedPlayer)
                    if (Prefab.Button("End Spectating", 150))
                        MiscOptions.SpectatedPlayer = null;
                
                if (MiscOptions.NoMovementVerification)
                    if (Prefab.Button("Teleport to player", 150))
                        OptimizationVariables.MainPlayer.transform.position = SelectedPlayer.transform.position;
                
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            });
            Prefab.MenuArea(new Rect(190 + 6, 250 + 10, 270, 150), "INFO", () =>
            {
                if (SelectedPlayer == null)
                    return;
                
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                
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