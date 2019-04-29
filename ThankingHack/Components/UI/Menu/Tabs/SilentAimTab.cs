using System;
using Thanking.Misc.Enums;
using Thanking.Options;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
	public class SilentAimTab
	{
		public static void Tab()
		{
			Prefab.MenuArea(new Rect(0, 0, 466, 436), "SILENT AIM", () =>
			{
				GUILayout.Space(2);
                Prefab.Toggle("Enabled", ref RaycastOptions.Enabled);

                if (!RaycastOptions.Enabled)
                    return;

                GUILayout.Space(5);

                Prefab.Toggle($"Hold Key ({HotkeyUtilities.GetHotkeyString("SilentAim", "_SilentAimKey")})", ref RaycastOptions.HoldKey);

                GUILayout.Space(5);

                GUILayout.Space(10);

                Prefab.Toggle("Sphere position prediction", ref SphereOptions.SpherePrediction);
			    GUILayout.Space(5);

			    if (!SphereOptions.SpherePrediction)
			    {
				    GUILayout.Label("Sphere Radius: " + Math.Round(SphereOptions.SphereRadius, 2) + "m", Prefab._TextStyle);
				    Prefab.Slider(0, 16, ref SphereOptions.SphereRadius, 200);
			    }

			    GUILayout.Label("Recursion Level: " + SphereOptions.RecursionLevel, Prefab._TextStyle);
			    SphereOptions.RecursionLevel = (int) Prefab.Slider(0, 4, SphereOptions.RecursionLevel, 200);
			        
			    GUILayout.Space(10);

			    string[] TargetPriorities =
			    {
				    "Players",
				    "Zombies",
				    "Sentries",
				    "Beds",
				    "Claim Flags",
				    "Storage",
				    "Vehicles"
			    };

			    for (int i = 0; i < TargetPriorities.Length; i++)
			    {
				    TargetPriority target = (TargetPriority) i;
				    bool enabled = RaycastOptions.Targets.Contains(target);
				    bool wasEnabled = enabled;
				        
				    Prefab.Toggle("Include " + TargetPriorities[i], ref enabled);
				    if (enabled && !wasEnabled)
					    RaycastOptions.Targets.Add(target);
				        
				    else if (!enabled && wasEnabled)
					    RaycastOptions.Targets.Remove(target);
			    }
			        
			    GUILayout.Space(15);
			        
			    Prefab.Toggle("Select Player", ref RaycastOptions.EnablePlayerSelection);
			    if (RaycastOptions.EnablePlayerSelection)
			    {
				    GUILayout.Space(2);
				    GUILayout.Label("Selection FOV: " + RaycastOptions.SelectedFOV, Prefab._TextStyle);
				    RaycastOptions.SelectedFOV = Prefab.Slider(1, 180, RaycastOptions.SelectedFOV, 200);
				    Prefab.Toggle("Only Shoot Selected", ref RaycastOptions.OnlyShootAtSelectedPlayer);
			    }
			    GUILayout.Space(2);
			    Prefab.Toggle("Use FOV", ref RaycastOptions.SilentAimUseFOV);

			    if (RaycastOptions.SilentAimUseFOV)
			    {
				    GUILayout.Space(2);
				    GUILayout.Label("Aim FOV: " + RaycastOptions.SilentAimFOV, Prefab._TextStyle);
				    RaycastOptions.SilentAimFOV = Prefab.Slider(1, 180, RaycastOptions.SilentAimFOV, 200);
			    }
			});
		}
	}
}