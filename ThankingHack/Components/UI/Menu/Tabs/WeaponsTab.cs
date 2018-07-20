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
		        Prefab.Toggle("No Recoil", ref WeaponOptions.NoRecoil);
		        Prefab.Toggle("No Spread", ref WeaponOptions.NoSpread);
		        Prefab.Toggle("No Sway", ref WeaponOptions.NoSway);
                Prefab.Toggle("No Drop", ref WeaponOptions.NoDrop);
		        Prefab.Toggle("Triggerbot", ref TriggerbotOptions.Enabled);
		        Prefab.Toggle("Auto Reload", ref WeaponOptions.AutoReload);
		        Prefab.Toggle("Oof on Death", ref WeaponOptions.OofOnDeath);
		        Prefab.Toggle("Show Weapon Information", ref WeaponOptions.ShowWeaponInfo);
		        GUILayout.Space(20);
		       
                Prefab.Toggle("Silent Aimbot", ref RaycastOptions.Enabled);
		        GUILayout.Space(10);
		        if (RaycastOptions.Enabled)
		        {
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

			        Prefab.Toggle("Custom Material", ref RaycastOptions.UseTargetMaterial);
			        GUILayout.Space(2);
			        Prefab.Toggle("Sphere position prediction", ref SphereOptions.SpherePrediction);
			        GUILayout.Space(5);

			        if (!SphereOptions.SpherePrediction)
			        {
				        GUILayout.Label("Sphere Radius: " + Math.Round(SphereOptions.SphereRadius, 2) + "m", Prefab._TextStyle);
				        Prefab.Slider(0, 16, ref SphereOptions.SphereRadius, 200);
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
			        
			        GUILayout.Space(2);
			        
			        if (RaycastOptions.UseTargetMaterial)
				        if (Prefab.List(200, "_TargetMaterial",
					        new GUIContent("Material: " + Materials[DropDown.Get("_TargetMaterial").ListIndex].text), Materials))
					        RaycastOptions.TargetMaterial = (EPhysicsMaterial) DropDown.Get("_TargetMaterial").ListIndex;
	
		        }

		        GUILayout.EndVertical();
		        GUILayout.BeginVertical();

		        if (RaycastOptions.Enabled)
		        {
			        Prefab.Toggle("Random Limb", ref RaycastOptions.UseRandomLimb);
		        
			        if (!RaycastOptions.UseRandomLimb)
				        Prefab.Toggle("Custom Limb", ref RaycastOptions.UseCustomLimb);
		        
			        Prefab.Toggle("Select Player", ref RaycastOptions.EnablePlayerSelection);
			        if (RaycastOptions.EnablePlayerSelection)
			        {
				        GUILayout.Space(2);
				        GUILayout.Label("Selection FOV: " + RaycastOptions.FOV, Prefab._TextStyle);
				        RaycastOptions.FOV = Prefab.Slider(1, 300, RaycastOptions.FOV, 200);
				        Prefab.Toggle("Only Shoot Selected", ref RaycastOptions.OnlyShootAtSelectedPlayer);
			        }
			        Prefab.Toggle("Custom Ragdoll Vector", ref RaycastOptions.UseModifiedVector);
			        
			        GUILayout.Space(2);
			        if (RaycastOptions.UseModifiedVector)
			        {
				        GUILayout.Label("Ragdoll Vector: X: " + RaycastOptions.TargetRagdoll.x, Prefab._TextStyle);
				        RaycastOptions.TargetRagdoll.x = (int) Prefab.Slider(-25, 25, RaycastOptions.TargetRagdoll.x, 200);
				        GUILayout.Label("Ragdoll Vector: Y: " + RaycastOptions.TargetRagdoll.y, Prefab._TextStyle);
				        RaycastOptions.TargetRagdoll.y = (int) Prefab.Slider(-25, 25, RaycastOptions.TargetRagdoll.y, 200);
				        GUILayout.Label("Ragdoll Vector: Z: " + RaycastOptions.TargetRagdoll.z, Prefab._TextStyle);
				        RaycastOptions.TargetRagdoll.z = (int) Prefab.Slider(-25, 25, RaycastOptions.TargetRagdoll.z, 200);
			        }
		        }
		        
		        GUILayout.EndVertical();
		        GUILayout.FlexibleSpace();
		        GUILayout.EndHorizontal();
	        });
        }
    }
}
