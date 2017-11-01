using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Options;
using Thanking.Options.UIVariables;
using Thanking.Options.VisualOptions;
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
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical();
                    Prefab.Toggle("Show Player Name", ref ESPOptions.ShowPlayerName);
                    Prefab.Toggle("Show Player Distance", ref ESPOptions.ShowPlayerDistance);
                    Prefab.Toggle("Show Player Weapon", ref ESPOptions.ShowPlayerWeapon);
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
                    BasicControls(ESPTarget.Items);
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

            Prefab.MenuArea(new Rect(225 + 5, 0, 466 - 225 - 5, 245), "OTHER", () =>
            {
                Prefab.SectionTabButton("Spammer", () =>
                {
                    GUILayout.Label("hehe xd");
                });

                Prefab.SectionTabButton("Radar", () =>
                {
                    GUILayout.Label("lol");
                });

                Prefab.SectionTabButton("Mirror Camera", () =>
                {
                    GUILayout.Label("u just got pranked my dude");
                });
            });

            Prefab.MenuArea(new Rect(225 + 5, 120 + 5 + 120 + 5, 466 - 225 - 5, 436 - 245 - 5), "TOGGLE", () =>
            {
                Prefab.Toggle("ESP", ref ESPOptions.Enabled);
                Prefab.Toggle("No Rain", ref MiscOptions.NoRain);
                Prefab.Toggle("No Snow", ref MiscOptions.NoSnow);
            });
        }

        private static void BasicControls(ESPTarget target)
        {
            Prefab.Toggle("Enabled", ref ESPOptions.VisualOptions[target].Enabled);
            Prefab.Toggle("Labels", ref ESPOptions.VisualOptions[target].Labels);
            Prefab.Toggle("Box ESP", ref ESPOptions.VisualOptions[target].Boxes);
            Prefab.Toggle("2D Boxes", ref ESPOptions.VisualOptions[target].TwoDimensional);
            Prefab.Toggle("Line To Object", ref ESPOptions.VisualOptions[target].LineToObject);
            Prefab.Toggle("Text Scaling", ref ESPOptions.VisualOptions[target].TextScaling);
            Prefab.Toggle("Infinite Distance", ref ESPOptions.VisualOptions[target].InfiniteDistance);
            GUILayout.Space(3);
            ESPOptions.VisualOptions[target].FixedTextSize = Prefab.TextField(ESPOptions.VisualOptions[target].FixedTextSize, "Fixed Text Size:", 30);
            GUILayout.Space(3);
            ESPOptions.VisualOptions[target].MinTextSize = Prefab.TextField(ESPOptions.VisualOptions[target].MinTextSize, "Min Text Size:", 30);
            GUILayout.Space(3);
            ESPOptions.VisualOptions[target].MaxTextSize = Prefab.TextField(ESPOptions.VisualOptions[target].MaxTextSize, "Max Text Size:", 30);
            GUILayout.Space(3);
            ESPOptions.VisualOptions[target].BorderStrength = Prefab.TextField(ESPOptions.VisualOptions[target].BorderStrength, "Border Strength:", 30);
            GUILayout.Space(3);
            GUILayout.Label("Text Scaling Falloff Distance: " + Mathf.RoundToInt(ESPOptions.VisualOptions[target].MinTextSizeDistance), Prefab._TextStyle);
            Prefab.Slider(0, 1000, ref ESPOptions.VisualOptions[target].MinTextSizeDistance, 200);
            GUILayout.Space(3);
            GUILayout.Label("ESP Distance: " + Mathf.RoundToInt(ESPOptions.VisualOptions[target].Distance), Prefab._TextStyle);
            Prefab.Slider(0, 4000, ref ESPOptions.VisualOptions[target].Distance, 200);
            GUILayout.Space(3);
            GUIContent[] LabelLocations = new GUIContent[] { new GUIContent("Top Right"), new GUIContent("Top Middle"), new GUIContent("Top Left"), new GUIContent("Middle Right"), new GUIContent("Center"), new GUIContent("Middle Left"), new GUIContent("Bottom Right"), new GUIContent("Bottom Middle"), new GUIContent("Bottom Left"), };
            if (Prefab.List(200, "_LabelLocations", new GUIContent("Label Location: " + LabelLocations[DropDown.Get("_LabelLocations").ListIndex].text), LabelLocations))
            {
                ESPOptions.VisualOptions[target].Location = (LabelLocation)DropDown.Get("_LabelLocations").ListIndex;
            }
        }
    }
}
