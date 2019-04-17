using Thanking.Components.Basic;
using Thanking.Misc;
using Thanking.Misc.Classes.ESP;
using Thanking.Misc.Enums;
using Thanking.Options;
using Thanking.Options.VisualOptions;
using Thanking.Utilities;
using Thanking.Variables;
using Thanking.Variables.UIVariables;
using Thnkng;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class VisualsTab
    {
        public static void Tab()
        {
            Prefab.MenuArea(new Rect(0, 0, 225, 436), "ESP", () =>
            {
                //Prefab.SectionTabButton("Global Override", () =>
                //{
                //    
                //});

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
                    Prefab.Toggle("2D Radar", ref RadarOptions.Enabled);
                    if (RadarOptions.Enabled)
                    {
                        GUILayout.Space(5);
                        string type = "";
                        if (RadarOptions.type == 1)
                            type = "Global";
                        if (RadarOptions.type == 2)
                            type = "Static Local";
                        if (RadarOptions.type == 3)
                            type = "Dynamic Local";

                        GUILayout.Label("Radar Type: " + type, Prefab._TextStyle);
                        
                        RadarOptions.type = (int)Prefab.Slider(1, 3, RadarOptions.type, 200);


                        Prefab.Toggle("Show Players", ref RadarOptions.ShowPlayers);
                        if (RadarOptions.ShowPlayers)
                            Prefab.Toggle("Detailed Plyers", ref RadarOptions.DetialedPlayers);
                        Prefab.Toggle("Show Vehicles", ref RadarOptions.ShowVehicles);
                        if (RadarOptions.ShowVehicles)
                            Prefab.Toggle("Show Only Unlocked", ref RadarOptions.ShowVehiclesUnlocked);
                        GUILayout.Space(5);
                        GUILayout.Label("Radar Zoom Multiplier: " + Mathf.Round(RadarOptions.RadarZoom), Prefab._TextStyle);
                        Prefab.Slider(0, 10, ref RadarOptions.RadarZoom, 200);
                        if (Prefab.Button("Reset Zoom", 100))
                            RadarOptions.RadarZoom = 1;
                        GUILayout.Space(5);
                        GUILayout.Label("Radar Size: " + Mathf.RoundToInt(RadarOptions.RadarSize), Prefab._TextStyle);
                        Prefab.Slider(50, 1000, ref RadarOptions.RadarSize, 200);
                    }
                });

                Prefab.Toggle("Show Vanish Players", ref ESPOptions.ShowVanishPlayers);
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

                        Ldr.HookObject.GetComponent<ESPComponent>().OnGUI();
                    }
                }

                Prefab.Toggle("Chams", ref ESPOptions.ChamsEnabled);

                if (ESPOptions.ChamsEnabled)
                    Prefab.Toggle("Flat Chams", ref ESPOptions.ChamsFlat);
                Prefab.Toggle("Ignore Z", ref ESPOptions.IgnoreZ);
                Prefab.Toggle("No Rain", ref MiscOptions.NoRain);
                Prefab.Toggle("No Snow", ref MiscOptions.NoSnow);
                Prefab.Toggle("No Flinch", ref MiscOptions.NoFlinch);
                Prefab.Toggle("No Grayscale", ref MiscOptions.NoGrayscale);
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
                Prefab.Toggle("Custom Text Color", ref visual.CustomTextColor);
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
        }
    }
}
