using SDG.Unturned;
using System;
using Thanking.Options;
using Thanking.Threads;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class MiscTab
    {
        public static void Tab()
        {
            Prefab.MenuArea(new Rect(0, 0, 466, 436), "MISC", () =>
			{
				GUILayout.BeginHorizontal();
				GUILayout.BeginVertical(GUILayout.Width(230));

				Prefab.Toggle("Vehicle Flight", ref MiscOptions.VehicleFly);

				if (MiscOptions.VehicleFly)
				{
					GUILayout.Label("Speed Multiplier: " + MiscOptions.SpeedMultiplier + "x", Prefab._TextStyle);
					GUILayout.Space(2);
					MiscOptions.SpeedMultiplier = (float)Math.Round(Prefab.Slider(0, 10, MiscOptions.SpeedMultiplier, 175), 2);
					GUILayout.Space(8);
				}

				Prefab.Toggle("Custom Salvage Time", ref MiscOptions.CustomSalvageTime);

				if (MiscOptions.CustomSalvageTime)
				{
					GUILayout.Label("Salvage Time: " + MiscOptions.SalvageTime + " seconds", Prefab._TextStyle);
					GUILayout.Space(2);
					MiscOptions.SalvageTime = (float)Math.Round(Prefab.Slider(0, 10, MiscOptions.SalvageTime, 175));
					GUILayout.Space(8);
				}


				Prefab.Toggle("Custom Day Time", ref MiscOptions.SetTimeEnabled);
				if (MiscOptions.SetTimeEnabled)
				{
					GUILayout.Label("Time: " + MiscOptions.Time, Prefab._TextStyle);
					GUILayout.Space(2);
					MiscOptions.Time = (float)Math.Round(Prefab.Slider(0, 0.9f, MiscOptions.Time, 175), 2);
				}

				GUILayout.EndVertical();
				GUILayout.BeginVertical();
				
				if (Provider.isConnected)
					Prefab.Toggle("Freecam", ref Player.player.look.isOrbiting);

				Prefab.Toggle("Crasher", ref CrashThread.CrashServerEnabled);
				Prefab.Toggle("Extended Melee Range", ref MiscOptions.ExtendMeleeRange);
				if (MiscOptions.ExtendMeleeRange)
				{
					GUILayout.Space(2);
					GUILayout.Label("Range: " + MiscOptions.MeleeRangeExtension, Prefab._TextStyle);
					GUILayout.Space(2);
					MiscOptions.MeleeRangeExtension = (float)Math.Round(Prefab.Slider(0, 15, MiscOptions.MeleeRangeExtension, 175), 1);
				}
				
				Prefab.Toggle("Auto Item Pickup", ref ItemOptions.AutoItemPickup);

				GUILayout.Space(2);
				GUILayout.Label("Delay: " + ItemOptions.ItemPickupDelay + "ms", Prefab._TextStyle);
				GUILayout.Space(2);
				ItemOptions.ItemPickupDelay = (int)Prefab.Slider(0, 3000, ItemOptions.ItemPickupDelay, 175);
				GUILayout.Space(5);

				ItemUtilities.DrawFilterTab(ItemOptions.ItemFilterOptions);
				
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();

				Prefab.MenuArea(new Rect(10, 436 - 135 - 10, 220, 135), "SPAMMER", () =>
                {
                    Prefab.Toggle("Enabled", ref MiscOptions.SpammerEnabled);

					GUILayout.Space(5);
                    MiscOptions.SpamText = Prefab.TextField(MiscOptions.SpamText, "Text: ", 150);
                    GUILayout.Space(10);
                    GUILayout.Label("Delay: " + MiscOptions.SpammerDelay + "ms", Prefab._TextStyle);
                    GUILayout.Space(5);
                    MiscOptions.SpammerDelay = (int)Prefab.Slider(0, 3000, MiscOptions.SpammerDelay, 175);
                });

                Prefab.MenuArea(new Rect(220 + 10 + 5, 436 - 135 - 10, 221, 135), "INTERACT", () =>
				{
					Prefab.Toggle("Interact Through Walls", ref InteractionOptions.InteractThroughWalls);

					if (!InteractionOptions.InteractThroughWalls)
						return;

                    Prefab.Toggle("Hit Structures", ref InteractionOptions.HitStructures);
                    Prefab.Toggle("Hit Barricades", ref InteractionOptions.HitBarricades);
                    Prefab.Toggle("Hit Items", ref InteractionOptions.HitItems);
                    Prefab.Toggle("Hit Vehicles", ref InteractionOptions.HitVehicles);
                    Prefab.Toggle("Hit Resources", ref InteractionOptions.HitResources);
                });
            });
        }
    }
}