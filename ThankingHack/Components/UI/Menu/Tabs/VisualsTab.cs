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
					if (!ESPOptions.VisualOptions[(int)ESPTarget.Players].Enabled)
						return;
					
					GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical(GUILayout.Width(240));
                    BasicControls(ESPTarget.Players);
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical();
                    Prefab.Toggle("Show Player Name", ref ESPOptions.ShowPlayerName);
                    Prefab.Toggle("Show Player Distance", ref ESPOptions.ShowPlayerDistance);
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
                    BasicControls(ESPTarget.Vehicles);
                });
                Prefab.SectionTabButton("Items", () =>
                {
					if (!ESPOptions.VisualOptions[(int)ESPTarget.Items].Enabled)
						return;

					BasicControls(ESPTarget.Items);
					Prefab.Toggle("Filter Items", ref ESPOptions.FilterItems);
					
					if (ESPOptions.FilterItems) 
					{
						GUILayout.Space(5);
						ItemUtilities.DrawFilterTab(ItemOptions.ItemESPOptions);
					}
                });
                Prefab.SectionTabButton("Storages", () =>
                {
                    BasicControls(ESPTarget.Storage);
                });
                Prefab.SectionTabButton("Beds", () =>
                {
                    BasicControls(ESPTarget.Beds);
                });
                Prefab.SectionTabButton("Generators", () =>
                {
                    BasicControls(ESPTarget.Generators);
                });
                Prefab.SectionTabButton("Sentries", () =>
                {
                    BasicControls(ESPTarget.Sentries);
                });
                Prefab.SectionTabButton("Claim Flags", () =>
                {
                    BasicControls(ESPTarget.ClaimFlags);
                });

            });

            Prefab.MenuArea(new Rect(225 + 5, 0, 466 - 225 - 5, 235), "OTHER", () =>
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

            Prefab.MenuArea(new Rect(225 + 5, 120 + 5 + 110 + 5, 466 - 225 - 5, 436 - 235 - 5), "TOGGLE", () =>
            {
                Prefab.Toggle("ESP", ref ESPOptions.Enabled);
				Prefab.Toggle("Chams", ref ESPOptions.ChamsEnabled);

				if (ESPOptions.ChamsEnabled)
					Prefab.Toggle("Flat Chams", ref ESPOptions.ChamsFlat);

                Prefab.Toggle("No Rain", ref MiscOptions.NoRain);
                Prefab.Toggle("No Snow", ref MiscOptions.NoSnow);
                Prefab.Toggle("Night Vision", ref MiscOptions.NightVision);
                Prefab.Toggle("Compass", ref MiscOptions.Compass);
                Prefab.Toggle("GPS", ref MiscOptions.GPS);
                Prefab.Toggle("Show Players On Map", ref MiscOptions.SPOM);
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
			Prefab.Toggle("Box ESP", ref visual.Boxes);
			Prefab.Toggle("2D Boxes", ref visual.TwoDimensional);
			Prefab.Toggle("Glow", ref visual.Glow);
			Prefab.Toggle("Line To Object", ref visual.LineToObject);

			Prefab.Toggle("Text Scaling", ref visual.TextScaling);
			if (visual.TextScaling)
			{
				visual.FixedTextSize = Prefab.TextField(visual.FixedTextSize, "Fixed Text Size:", 30);
				GUILayout.Space(3);
				visual.MinTextSize = Prefab.TextField(visual.MinTextSize, "Min Text Size:", 30);
				GUILayout.Space(3);
				visual.MaxTextSize = Prefab.TextField(visual.MaxTextSize, "Max Text Size:", 30);
				GUILayout.Space(3);
				GUILayout.Label("Text Scaling Falloff Distance: " + Mathf.RoundToInt(visual.MinTextSizeDistance), Prefab._TextStyle);
				Prefab.Slider(0, 1000, ref visual.MinTextSizeDistance, 200);
				GUILayout.Space(3);
			}

			Prefab.Toggle("Infinite Distance", ref visual.InfiniteDistance);
			if (!visual.InfiniteDistance)
			{
				GUILayout.Label("ESP Distance: " + Mathf.RoundToInt(visual.Distance), Prefab._TextStyle);
				Prefab.Slider(0, 4000, ref visual.Distance, 200);
				GUILayout.Space(3);
			}
			visual.BorderStrength = Prefab.TextField(visual.BorderStrength, "Border Strength:", 30);
			GUILayout.Space(6);
			GUIContent[] LabelLocations = { new GUIContent("Top Right"), new GUIContent("Top Middle"), new GUIContent("Top Left"), new GUIContent("Middle Right"), new GUIContent("Center"), new GUIContent("Middle Left"), new GUIContent("Bottom Right"), new GUIContent("Bottom Middle"), new GUIContent("Bottom Left") };
			if (Prefab.List(200, "_LabelLocations", new GUIContent("Label Location: " + LabelLocations[DropDown.Get("_LabelLocations").ListIndex].text), LabelLocations))
				ESPOptions.VisualOptions[target].Location = (LabelLocation)DropDown.Get("_LabelLocations").ListIndex;
		}
	}
}
