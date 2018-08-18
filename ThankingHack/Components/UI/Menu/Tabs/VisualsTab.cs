using Thanking.Components.Basic;
using Thanking.Options;
using Thanking.Options.UIVariables;
using Thanking.Options.VisualOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class VisualsTab
    {
        public static void Tab()
        {
            Prefab.MenuArea(new Rect(0, 0, 225, 436), "ESP", () =>
            {
                Prefab.SectionTabButton("Players", () =>
                {					
					GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical(GUILayout.Width(240));
	                
                    BasicControls(ESPTarget.Players);

					if (!ESPOptions.VisualOptions[(int)ESPTarget.Players].Enabled)
						return;

					GUILayout.EndVertical();
                    GUILayout.BeginVertical();
                    Prefab.Toggle("Show Player Weapon", ref ESPOptions.ShowPlayerWeapon);
					Prefab.Toggle("Show Player Vehicle", ref ESPOptions.ShowPlayerVehicle);
					Prefab.Toggle("Use Player Group", ref ESPOptions.UsePlayerGroup);
                    GUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                });
                Prefab.SectionTabButton("Zombies", () =>
                {
                    BasicControls(ESPTarget.Zombies);
                });
                Prefab.SectionTabButton("Vehicles", () =>
                {
	                GUILayout.BeginHorizontal();
	                GUILayout.BeginVertical(GUILayout.Width(240));
	                
                    BasicControls(ESPTarget.Vehicles);
	                
	                if (!ESPOptions.VisualOptions[(int)ESPTarget.Vehicles].Enabled)
		                return;
	                
	                GUILayout.EndVertical();
	                GUILayout.BeginVertical();
	                
	                Prefab.Toggle("Show Vehicle Fuel", ref ESPOptions.ShowVehicleFuel);
	                Prefab.Toggle("Show Vehicle Health", ref ESPOptions.ShowVehicleHealth);
	                Prefab.Toggle("Show Vehicle Locked", ref ESPOptions.ShowVehicleLocked);
	                Prefab.Toggle("Filter Out Locked", ref ESPOptions.FilterVehicleLocked);
	                
	                GUILayout.EndVertical();
	                GUILayout.FlexibleSpace();
	                GUILayout.EndHorizontal();
                });
                Prefab.SectionTabButton("Items", () =>
                {
	                GUILayout.BeginHorizontal();
	                GUILayout.BeginVertical(GUILayout.Width(240));
	                
					BasicControls(ESPTarget.Items);
	                
	                if (!ESPOptions.VisualOptions[(int)ESPTarget.Items].Enabled)
		                return;
	                
	                GUILayout.EndVertical();
	                GUILayout.BeginVertical();
	                
					Prefab.Toggle("Filter Items", ref ESPOptions.FilterItems);
					
					if (ESPOptions.FilterItems) 
					{
						GUILayout.Space(5);
						ItemUtilities.DrawFilterTab(ItemOptions.ItemESPOptions);
					}
	                
	                GUILayout.EndVertical();
	                GUILayout.FlexibleSpace();
	                GUILayout.EndHorizontal();
                });
                Prefab.SectionTabButton("Storages", () =>
                {
	                BasicControls(ESPTarget.Storage);
                });
                Prefab.SectionTabButton("Beds", () =>
                {
	                GUILayout.BeginHorizontal();
	                GUILayout.BeginVertical(GUILayout.Width(240));
	                
	                BasicControls(ESPTarget.Beds);
	                
	                if (!ESPOptions.VisualOptions[(int)ESPTarget.Beds].Enabled)
		                return;
	                
	                GUILayout.EndVertical();
	                GUILayout.BeginVertical();

	                Prefab.Toggle("Show Claimed", ref ESPOptions.ShowClaimed);
	                
	                GUILayout.EndVertical();
	                GUILayout.FlexibleSpace();
	                GUILayout.EndHorizontal();
                });
                Prefab.SectionTabButton("Generators", () =>
                {
	                GUILayout.BeginHorizontal();
	                GUILayout.BeginVertical(GUILayout.Width(240));
	                
	                BasicControls(ESPTarget.Generators);
	                
	                if (!ESPOptions.VisualOptions[(int)ESPTarget.Generators].Enabled)
		                return;
	                
	                GUILayout.EndVertical();
	                GUILayout.BeginVertical();
	                
	                Prefab.Toggle("Show Generator Fuel", ref ESPOptions.ShowGeneratorFuel);
	                Prefab.Toggle("Show Generator Powered", ref ESPOptions.ShowGeneratorPowered);
	                
	                GUILayout.EndVertical();
	                GUILayout.FlexibleSpace();
	                GUILayout.EndHorizontal();
                });
                Prefab.SectionTabButton("Sentries", () =>
                {
	                GUILayout.BeginHorizontal();
	                GUILayout.BeginVertical(GUILayout.Width(240));
	                
	                BasicControls(ESPTarget.Sentries);
	                
	                if (!ESPOptions.VisualOptions[(int)ESPTarget.Sentries].Enabled)
		                return;
	                
	                GUILayout.EndVertical();
	                GUILayout.BeginVertical();

	                Prefab.Toggle("Show Sentry Item", ref ESPOptions.ShowSentryItem);
	                
	                GUILayout.EndVertical();
	                GUILayout.FlexibleSpace();
	                GUILayout.EndHorizontal();
                });
                Prefab.SectionTabButton("Claim Flags", () =>
                {
                    BasicControls(ESPTarget.ClaimFlags);
                });

            });

            Prefab.MenuArea(new Rect(225 + 5, 0, 466 - 225 - 5, 180), "OTHER", () =>
            {
                Prefab.SectionTabButton("Radar", () =>
                {
                    GUILayout.Label("Coming soon!");
                });
                Prefab.Toggle("Mirror Camera", ref MirrorCameraOptions.Enabled);
                GUILayout.Space(5);
                if (Prefab.Button("Fix Camera", 100))
					MirrorCameraComponent.FixCam();
			});

            Prefab.MenuArea(new Rect(225 + 5, 180 + 5, 466 - 225 - 5, 436 - 186), "TOGGLE", () =>
            {
	            if (Prefab.Toggle("ESP", ref ESPOptions.Enabled))
	            {
		            if (!ESPOptions.Enabled)
		            {
			            for (int i = 0; i < ESPOptions.VisualOptions.Length; i++)
				            ESPOptions.VisualOptions[i].Glow = false;
		            
			            Loader.HookObject.GetComponent<ESPComponent>().OnGUI();		
		            }
	            }
	            
				Prefab.Toggle("Chams", ref ESPOptions.ChamsEnabled);

				if (ESPOptions.ChamsEnabled)
					Prefab.Toggle("Flat Chams", ref ESPOptions.ChamsFlat);

                Prefab.Toggle("No Rain", ref MiscOptions.NoRain);
                Prefab.Toggle("No Snow", ref MiscOptions.NoSnow);
                Prefab.Toggle("Night Vision", ref MiscOptions.NightVision);
                Prefab.Toggle("Compass", ref MiscOptions.Compass);
                Prefab.Toggle("GPS", ref MiscOptions.GPS);
                Prefab.Toggle("Show Players On Map", ref MiscOptions.ShowPlayersOnMap);
            });
        }

        private static void BasicControls(ESPTarget esptarget)
        {
            int target = (int)esptarget;
			ESPVisual visual = ESPOptions.VisualOptions[target];
			Prefab.Toggle("Enabled", ref visual.Enabled);
			if (!visual.Enabled)
				return;

			Prefab.Toggle("Labels", ref visual.Labels);
	        if (visual.Labels)
	        {
		        Prefab.Toggle("Show Name", ref visual.ShowName);
		        Prefab.Toggle("Show Distance", ref visual.ShowDistance);
		        Prefab.Toggle("Show Angle", ref visual.ShowAngle);
	        }
	        
			Prefab.Toggle("Box ESP", ref visual.Boxes);
	        
	        if (visual.Boxes)
				Prefab.Toggle("2D Boxes", ref visual.TwoDimensional);
	        
			Prefab.Toggle("Glow", ref visual.Glow);
			Prefab.Toggle("Line To Object", ref visual.LineToObject);

			Prefab.Toggle("Text Scaling", ref visual.TextScaling);
			if (visual.TextScaling)
			{
				visual.MinTextSize = Prefab.TextField(visual.MinTextSize, "Min Text Size:", 30);
				visual.MaxTextSize = Prefab.TextField(visual.MaxTextSize, "Max Text Size:", 30);
				GUILayout.Space(3);
				GUILayout.Label("Text Scaling Falloff Distance: " + Mathf.RoundToInt(visual.MinTextSizeDistance), Prefab._TextStyle);
				Prefab.Slider(0, 1000, ref visual.MinTextSizeDistance, 200);
				GUILayout.Space(3);
			}
	        else
				visual.FixedTextSize = Prefab.TextField(visual.FixedTextSize, "Fixed Text Size:", 30);

			Prefab.Toggle("Infinite Distance", ref visual.InfiniteDistance);
			if (!visual.InfiniteDistance)
			{
				GUILayout.Label("ESP Distance: " + Mathf.RoundToInt(visual.Distance), Prefab._TextStyle);
				Prefab.Slider(0, 4000, ref visual.Distance, 200);
				GUILayout.Space(3);
			}	
	        
	        Prefab.Toggle("Limit Object Numer", ref visual.UseObjectCap);
	        if (visual.UseObjectCap)
		        visual.ObjectCap = Prefab.TextField(visual.ObjectCap, "Object cap:", 30);
	        
			visual.BorderStrength = Prefab.TextField(visual.BorderStrength, "Border Strength:", 30);
			GUILayout.Space(3);
			GUIContent[] LabelLocations = { new GUIContent("Top Right"), new GUIContent("Top Middle"), new GUIContent("Top Left"), new GUIContent("Middle Right"), new GUIContent("Center"), new GUIContent("Middle Left"), new GUIContent("Bottom Right"), new GUIContent("Bottom Middle"), new GUIContent("Bottom Left") };
			if (Prefab.List(200, "_LabelLocations", new GUIContent("Label Location: " + LabelLocations[(int)ESPOptions.VisualOptions[target].Location].text), LabelLocations))
				ESPOptions.VisualOptions[target].Location = (LabelLocation)DropDown.Get("_LabelLocations").ListIndex;
		}
	}
}
