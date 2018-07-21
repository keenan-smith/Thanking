using SDG.Unturned;
using System;
using System.Reflection;
using Thanking.Components.Basic;
using Thanking.Options;
using Thanking.Threads;
using Thanking.Utilities;
using Thanking.Variables;
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
					GUILayout.Space(8);
				}

				if (MiscOptions.NoMovementVerification)
				{
					Prefab.Toggle("Player Flight", ref MiscOptions.PlayerFlight);
					if (MiscOptions.PlayerFlight)
					{
						GUILayout.Label("Speed Multiplier: " + MiscOptions.FlightSpeedMultiplier + "x", Prefab._TextStyle);
						GUILayout.Space(2);
						MiscOptions.FlightSpeedMultiplier = (float)Math.Round(Prefab.Slider(0, 10, MiscOptions.FlightSpeedMultiplier, 175), 2);
					}
				}
				
				Prefab.Toggle("Punch Silent Aim", ref MiscOptions.PunchSilentAim);

				GUILayout.EndVertical();
				GUILayout.BeginVertical();

				if (Provider.isConnected && OptimizationVariables.MainPlayer != null)
				{
					if (!OptimizationVariables.MainPlayer.look.isOrbiting)
						OptimizationVariables.MainPlayer.look.orbitPosition = Vector3.zero;
					
					Prefab.Toggle("Freecam", ref MiscOptions.Freecam);
					if (OptimizationVariables.MainPlayer.look.isOrbiting)
						if (Prefab.Button("Reset Camera", 150))
							OptimizationVariables.MainPlayer.look.orbitPosition = Vector3.zero;
				}

				Prefab.Toggle("Crash Server", ref ServerCrashThread.ServerCrashEnabled);
				Prefab.Toggle("Crash Server Automatically", ref ServerCrashThread.AlwaysCrash);
				
				Prefab.Toggle("Crash All Players", ref PlayerCrashThread.ContinuousPlayerCrash);
				
				Prefab.Toggle("Auto Check Movement", ref MiscOptions.AlwaysCheckMovementVerification);

				if (Provider.isConnected)
				{
					if (Prefab.Button("Check Movement Verification", 210))
						MiscComponent.CheckMovementVerification();
				}
									
				Prefab.Toggle("Extended Melee Range", ref MiscOptions.ExtendMeleeRange);
				if (MiscOptions.ExtendMeleeRange)
				{
					GUILayout.Space(2);
					GUILayout.Label("Range: " + MiscOptions.MeleeRangeExtension, Prefab._TextStyle);
					GUILayout.Space(2);
					MiscOptions.MeleeRangeExtension = (float)Math.Round(Prefab.Slider(0, 7.5f, MiscOptions.MeleeRangeExtension, 175), 1);
				}

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
                    MiscOptions.SpammerDelay = (int)Prefab.Slider(0, 10000, MiscOptions.SpammerDelay, 175);
                });

                Prefab.MenuArea(new Rect(220 + 10 + 5, 436 - 135 - 30, 221, 155), "INTERACT", () =>
				{
					Prefab.Toggle("Interact Through Things", ref InteractionOptions.InteractThroughWalls);

					if (!InteractionOptions.InteractThroughWalls)
						return;

                    Prefab.Toggle("Walls/Floors/Etc", ref InteractionOptions.NoHitStructures);
                    Prefab.Toggle("Lockers/Doors/Etc", ref InteractionOptions.NoHitBarricades);
                    Prefab.Toggle("Items", ref InteractionOptions.NoHitItems);
                    Prefab.Toggle("Vehicles", ref InteractionOptions.NoHitVehicles);
                    Prefab.Toggle("Resources", ref InteractionOptions.NoHitResources);
					Prefab.Toggle("Ground/Buildings", ref InteractionOptions.NoHitEnvironment);
                });
            });
        }
    }
}