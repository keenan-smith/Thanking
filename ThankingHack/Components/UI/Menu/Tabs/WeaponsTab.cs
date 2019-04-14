using SDG.Unturned;
using Thanking.Options.AimOptions;
using Thanking.Variables.UIVariables;
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
                Prefab.Toggle("Automatic Semi-Automatic", ref WeaponOptions.FastSemiAuto);
                Prefab.Toggle("No Recoil", ref WeaponOptions.NoRecoil);
		        Prefab.Toggle("No Spread", ref WeaponOptions.NoSpread);
		        Prefab.Toggle("No Sway", ref WeaponOptions.NoSway);
                Prefab.Toggle("No Drop", ref WeaponOptions.NoDrop);
		        Prefab.Toggle("Triggerbot", ref TriggerbotOptions.Enabled);
		        Prefab.Toggle("Auto Reload", ref WeaponOptions.AutoReload);
		        Prefab.Toggle("Show Weapon Information", ref WeaponOptions.ShowWeaponInfo);
                Prefab.Toggle("Bullet Drop Prediction", ref WeaponOptions.EnableBulletDropPrediction);
                Prefab.Toggle("Highlight Prediction Target", ref WeaponOptions.HighlightBulletDropPredictionTarget);
		        Prefab.Toggle("Custom Material", ref RaycastOptions.UseTargetMaterial);
		        
		        Prefab.Toggle("Random Limb", ref RaycastOptions.UseRandomLimb);
		        
		        if (!RaycastOptions.UseRandomLimb)
			        Prefab.Toggle("Custom Limb", ref RaycastOptions.UseCustomLimb);
		        
		        GUILayout.Space(2);
		        
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
		        
		        GUILayout.Space(2);
		        
		        if (RaycastOptions.UseCustomLimb && !RaycastOptions.UseRandomLimb)
			        if (Prefab.List(230, "_TargetLimb",
				        new GUIContent("Limb: " + Limbs[(int)RaycastOptions.TargetLimb].text),
				        Limbs))
				        RaycastOptions.TargetLimb = (ELimb) DropDown.Get("_TargetLimb").ListIndex;

		        
		        GUILayout.Space(2);
			        
		        if (RaycastOptions.UseTargetMaterial)
			        if (Prefab.List(230, "_TargetMaterial",
				        new GUIContent("Material: " + Materials[(int)RaycastOptions.TargetMaterial].text), Materials))
				        RaycastOptions.TargetMaterial = (EPhysicsMaterial) DropDown.Get("_TargetMaterial").ListIndex;
			        
		        GUILayout.EndVertical();
		        GUILayout.BeginVertical(GUILayout.Width(230));
		        
		        Prefab.Toggle("Custom Ragdoll Vector", ref RaycastOptions.UseModifiedVector);
		        
		        GUILayout.Space(2);
		        if (RaycastOptions.UseModifiedVector)
		        {
			        GUILayout.Label("Ragdoll Vector: X: " + RaycastOptions.TargetRagdoll.x, Prefab._TextStyle);
			        RaycastOptions.TargetRagdoll.x = (int) Prefab.Slider(-100, 100, RaycastOptions.TargetRagdoll.x, 200);
			        GUILayout.Label("Ragdoll Vector: Y: " + RaycastOptions.TargetRagdoll.y, Prefab._TextStyle);
			        RaycastOptions.TargetRagdoll.y = (int) Prefab.Slider(-100, 100, RaycastOptions.TargetRagdoll.y, 200);
			        GUILayout.Label("Ragdoll Vector: Z: " + RaycastOptions.TargetRagdoll.z, Prefab._TextStyle);
			        RaycastOptions.TargetRagdoll.z = (int) Prefab.Slider(-100, 100, RaycastOptions.TargetRagdoll.z, 200);
		        }
		        GUILayout.Space(2);
		        Prefab.Toggle("Tracers", ref WeaponOptions.Tracers);
		        
		        GUILayout.EndVertical();
		        GUILayout.FlexibleSpace();
		        GUILayout.EndHorizontal();
	        });
        }
    }
}
