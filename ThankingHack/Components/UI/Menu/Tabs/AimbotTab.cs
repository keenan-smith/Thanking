﻿using Thanking.Options.AimOptions;
using Thanking.Options.UIVariables;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class AimbotTab
    {
        public static void Tab()
        {
            Prefab.MenuArea(new Rect(0, 0, 466, 436), "AIMBOT", () =>
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(GUILayout.Width(230));
                Prefab.Toggle("Enabled", ref AimbotOptions.Enabled);
                Prefab.Toggle("Use Gun Distance", ref AimbotOptions.UseGunDistance);
                Prefab.Toggle("Smooth", ref AimbotOptions.Smooth);
                GUILayout.Space(3);
				if (AimbotOptions.Smooth)
				{
					GUILayout.Label("Aim Speed: " + AimbotOptions.AimSpeed, Prefab._TextStyle);
					AimbotOptions.AimSpeed = (int)Prefab.Slider(1, AimbotOptions.MaxSpeed, AimbotOptions.AimSpeed, 200);
				}

                GUILayout.Label("FOV: " + AimbotOptions.FOV, Prefab._TextStyle);
                AimbotOptions.FOV = (int)Prefab.Slider(1, 300, AimbotOptions.FOV, 200);
                GUILayout.Label("Distance: " + AimbotOptions.Distance, Prefab._TextStyle);
                AimbotOptions.Distance = (int)Prefab.Slider(50, 1000, AimbotOptions.Distance, 200);
                GUIContent[] TargetMode = {
                    new GUIContent("Distance"),
                    new GUIContent("FOV")
                };

                if (Prefab.List(200, "_TargetMode", new GUIContent("Target Mode: " + TargetMode[DropDown.Get("_TargetMode").ListIndex].text), TargetMode))
                {
                    AimbotOptions.TargetMode = (TargetMode)DropDown.Get("_TargetMode").ListIndex;
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            });
        }
    }
}
