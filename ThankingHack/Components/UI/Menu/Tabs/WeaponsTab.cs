using System;
using SDG.Unturned;
using Thanking.Options;
using Thanking.Options.AimOptions;
using Thanking.Options.UIVariables;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class WeaponsTab
    {
        public static void Tab()
        {
	        Prefab.MenuArea(new Rect(0, 0, 466, 436), "WEAPONS", () =>
	        {
		        GUILayout.BeginHorizontal();
		        GUILayout.BeginVertical(GUILayout.Width(230));
		        Prefab.Toggle("Triggerbot", ref TriggerbotOptions.Enabled);
		        Prefab.Toggle("No Recoil", ref WeaponOptions.NoRecoil);
		        Prefab.Toggle("No Spread", ref WeaponOptions.NoSpread);
		        Prefab.Toggle("No Sway", ref WeaponOptions.NoSway);
                Prefab.Toggle("No Drop", ref WeaponOptions.NoDrop);
                Prefab.Toggle("Auto Reload", ref WeaponOptions.AutoReload);
		        Prefab.Toggle("Oof on Death", ref WeaponOptions.OofOnDeath);
		        Prefab.Toggle("Show Weapon Information", ref WeaponOptions.ShowWeaponInfo);
		        GUILayout.Space(20);
		        Prefab.Toggle("Random Limb", ref RaycastOptions.UseRandomLimb);
		        Prefab.Toggle("Custom Ragdoll Vector", ref RaycastOptions.UseModifiedVector);
		        Prefab.Toggle("Custom Limb", ref RaycastOptions.UseCustomLimb);
		        Prefab.Toggle("Custom Material", ref RaycastOptions.UseTargetMaterial);
		        Prefab.Toggle("Silent Aimbot", ref RaycastOptions.Enabled);
		        GUILayout.Space(10);

		        if (RaycastOptions.Enabled)
		        {
			        GUILayout.Space(10);
			        Prefab.Toggle("Dynamic Sphere Radius", ref SphereOptions.DynamicSphere);
			        GUILayout.Space(5);

			        if (!SphereOptions.DynamicSphere)
			        {
				        GUILayout.Label("Sphere Radius: " + Math.Round(SphereOptions.SphereRadius, 2) + "m", Prefab._TextStyle);
				        Prefab.Slider(0, 16, ref SphereOptions.SphereRadius, 200);
				        GUILayout.Label("Vehicle Sphere Radius: " + Math.Round(SphereOptions.VehicleSphereRadius, 2) + "m",
					        Prefab._TextStyle);
				        Prefab.Slider(0, 16, ref SphereOptions.VehicleSphereRadius, 200);
			        }

			        GUILayout.Label("Recursion Level: " + SphereOptions.RecursionLevel, Prefab._TextStyle);
			        SphereOptions.RecursionLevel = (int) Prefab.Slider(0, 4, SphereOptions.RecursionLevel, 200);

			        GUIContent[] TargetPriorities =
			        {
				        new GUIContent("Players"),
				        new GUIContent("Zombies"),
				        new GUIContent("Sentries"),
				        new GUIContent("Beds"),
				        new GUIContent("Claim Flags"),
				        new GUIContent("Storage"),
				        new GUIContent("Vehicles")
			        };

			        if (Prefab.List(200, "_TargetPriority",
				        new GUIContent("Priority: " + TargetPriorities[DropDown.Get("_TargetPriority").ListIndex].text),
				        TargetPriorities))
				        RaycastOptions.Target = (TargetPriority) DropDown.Get("_TargetPriority").ListIndex;
		        }
		        
		        GUIContent[] Limbs =
		        {
			        new GUIContent("Left Foot"),
			        new GUIContent("Left Leg"),
			        new GUIContent("Right Foot"),
			        new GUIContent("Right Leg"),
			        new GUIContent("Left Hand"),
			        new GUIContent("Left Arm"),
			        new GUIContent("Right Hand"),
			        new GUIContent("Right Arm"),
			        new GUIContent("Left Back"),
			        new GUIContent("Right Back"),
			        new GUIContent("Left Front"),
			        new GUIContent("Right Front"),
			        new GUIContent("Spine"),
			        new GUIContent("Skull")
		        };
		        if (RaycastOptions.UseCustomLimb)
			        if (Prefab.List(200, "_TargetLimb",
				        new GUIContent("Limb: " + Limbs[DropDown.Get("_TargetLimb").ListIndex].text),
				        Limbs))
				        RaycastOptions.TargetLimb = (ELimb) DropDown.Get("_TargetLimb").ListIndex;


		        GUIContent[] Materials =
		        {
			        new GUIContent("None"),
			        new GUIContent("Cloth Dynamic"),
			        new GUIContent("Cloth Static"),
			        new GUIContent("Tile Dynamic"),
			        new GUIContent("Tile Static"),
			        new GUIContent("Concrete Dynamic"),
			        new GUIContent("Concrete Static"),
			        new GUIContent("Flesh Dynamic"),
			        new GUIContent("Gravel Dynamic"),
			        new GUIContent("Gravel Static"),
			        new GUIContent("Metal Dynamic"),
			        new GUIContent("Metal Static"),
			        new GUIContent("Metal Slip"),
			        new GUIContent("Wood Dynamic"),
			        new GUIContent("Wood Static"),
			        new GUIContent("Foliage Static"),
			        new GUIContent("Foliage Dynamic"),
			        new GUIContent("Snow Static"),
			        new GUIContent("Ice Static"),
			        new GUIContent("Water Static"),
			        new GUIContent("Alien Dynamic")
		        };

		        if (RaycastOptions.UseTargetMaterial)
			        if (Prefab.List(200, "_TargetMaterial",
				        new GUIContent("Material: " + Materials[DropDown.Get("_TargetMaterial").ListIndex].text), Materials))
				        RaycastOptions.TargetMaterial = (EPhysicsMaterial) DropDown.Get("_TargetMaterial").ListIndex;
		        
		        GUILayout.EndVertical();
		        GUILayout.BeginVertical();

		        if (RaycastOptions.UseModifiedVector)
		        {
			        GUILayout.Label("Ragdoll Vector: X: " + RaycastOptions.TargetRagdoll.x, Prefab._TextStyle);
			        RaycastOptions.TargetRagdoll.x = (int) Prefab.Slider(-25, 25, RaycastOptions.TargetRagdoll.x, 200);
			        GUILayout.Label("Ragdoll Vector: Y: " + RaycastOptions.TargetRagdoll.y, Prefab._TextStyle);
			        RaycastOptions.TargetRagdoll.y = (int) Prefab.Slider(-25, 25, RaycastOptions.TargetRagdoll.y, 200);
			        GUILayout.Label("Ragdoll Vector: Z: " + RaycastOptions.TargetRagdoll.z, Prefab._TextStyle);
			        RaycastOptions.TargetRagdoll.z = (int) Prefab.Slider(-25, 25, RaycastOptions.TargetRagdoll.z, 200);
		        }

		        GUILayout.EndVertical();
		        GUILayout.FlexibleSpace();
		        GUILayout.EndHorizontal();
	        });
        }
    }
}
