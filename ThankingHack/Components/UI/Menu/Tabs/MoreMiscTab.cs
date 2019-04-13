using System;
using System.Linq;
using SDG.Unturned;
using Thanking.Options;
using Thanking.Options.AimOptions;
using Thanking.Threads;
using Thanking.Utilities;
using Thanking.Variables.UIVariables;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class MoreMiscTab
    {
        static Vector2 Scroll;
        static Vector2 Scroll1;
        static string text;
        static string text1;
        public static void Tab()
        {
            Prefab.MenuArea(new Rect(0, 0, 466, 436), "MORE MISC", () =>
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(GUILayout.Width(230));
                GUILayout.Space(2);
                Prefab.Toggle("Auto Item Pickup", ref ItemOptions.AutoItemPickup);

                if (ItemOptions.AutoItemPickup)
                {
                    GUILayout.Space(5);
                    GUILayout.Label("Delay: " + ItemOptions.ItemPickupDelay + "ms", Prefab._TextStyle);
                    GUILayout.Space(2);
                    ItemOptions.ItemPickupDelay = (int)Prefab.Slider(0, 3000, ItemOptions.ItemPickupDelay, 175);
                }

                GUILayout.Space(5);

                Prefab.Toggle("Oof on Death", ref WeaponOptions.OofOnDeath);

                GUILayout.Space(5);

                ItemUtilities.DrawFilterTab(ItemOptions.ItemFilterOptions);

                GUILayout.Space(5);

                GUIContent[] SpyMethods =
                {
                    new GUIContent("Remove All Visuals"),
                    new GUIContent("Random Image in Folder"),
                    new GUIContent("Send No Image"),
                    new GUIContent("No Anti-Spy")
                };

                GUILayout.Label("Anti-Spy Method:", Prefab._TextStyle);
                if (Prefab.List(200, "_SpyMethods",
                    new GUIContent(SpyMethods[MiscOptions.AntiSpyMethod].text),
                    SpyMethods))
                    MiscOptions.AntiSpyMethod = DropDown.Get("_SpyMethods").ListIndex;

                if (MiscOptions.AntiSpyMethod == 1)
                {
                    GUILayout.Space(2);
                    GUILayout.Label("Anti-Spy Image Folder:", Prefab._TextStyle);
                    MiscOptions.AntiSpyPath = Prefab.TextField(MiscOptions.AntiSpyPath, "", 225);
                }

                GUILayout.Space(5);
                Prefab.Toggle("Alert on Spy", ref MiscOptions.AlertOnSpy);

                GUILayout.Space(5);
                Prefab.Toggle("Punch Killaura", ref MiscOptions.PunchAura);

                GUILayout.Space(5);
                if (Prefab.Button("Instant Disconnect", 200))
                    Provider.disconnect();

                GUILayout.Space(5);
                if (Prefab.Button("Clear Auto Crasher", 200))
                    PlayerCrashThread.CrashTargets.Clear();

                GUILayout.Space(5);
                Prefab.Toggle("Spinbot", ref MiscOptions.Spinbot);

                if (MiscOptions.Spinbot)
                {
                    GUILayout.Space(2);
                    Prefab.Toggle("Static Pitch", ref MiscOptions.StaticSpinbotPitch);
                    GUILayout.Space(2);
                    GUILayout.Label($"Spinbot Pitch {(!MiscOptions.StaticSpinbotPitch ? "Incr." : "")}: " + MiscOptions.SpinbotPitch, Prefab._TextStyle);
                    GUILayout.Space(2);
                    MiscOptions.SpinbotPitch = Prefab.Slider(0, 180, MiscOptions.SpinbotPitch, 200);
                    GUILayout.Space(2);
                    Prefab.Toggle("Static Yaw", ref MiscOptions.StaticSpinbotYaw);
                    GUILayout.Space(2);
                    GUILayout.Label($"Spinbot Yaw {(!MiscOptions.StaticSpinbotYaw ? "Incr." : "")}: " + MiscOptions.SpinbotYaw, Prefab._TextStyle);
                    GUILayout.Space(2);
                    MiscOptions.SpinbotYaw = Prefab.Slider(0, 360, MiscOptions.SpinbotYaw, 200);
                }

                GUILayout.EndVertical();
                GUILayout.BeginVertical();

                Prefab.Toggle("Crash By List", ref MiscOptions.CrashByName);
                Prefab.SectionTabButton("Crash Lists", () =>
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical(GUILayout.Width(300));
                    Prefab.ScrollView(new Rect(10, 20, 298, 300), "Crash Prefixes", ref Scroll, () =>
                    {
                        foreach (string Word in MiscOptions.CrashWords)
                        {
                            if (Prefab.Button(Word, 255))
                            {
                                MiscOptions.CrashWords.Remove(Word);
                            }
                        }
                    });

                    GUILayout.Space(310);
                    text = Prefab.TextField(text, "Import Prefix/Name: ", 100);
                    if (Prefab.Button("Add", 100))
                    {
                        MiscOptions.CrashWords.AddRange(text.Split(',').Reverse().ToList());
                        text = "";
                    }

                    GUILayout.EndVertical();
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical(GUILayout.Width(300));
                    Prefab.ScrollView(new Rect(312, 20, 300, 300), "Crash Steam IDs", ref Scroll1, () =>
                    {
                        foreach (string Word in MiscOptions.CrashIDs)
                        {
                            if (Prefab.Button(Word, 255))
                            {
                                MiscOptions.CrashIDs.Remove(Word);
                            }
                        }
                    });

                    GUILayout.Space(310);
                    text1 = Prefab.TextField(text1, "Import Steam ID: ", 100);
                    if (Prefab.Button("Add", 100))
                    {
                        MiscOptions.CrashIDs.AddRange(text1.Split(',').Reverse().ToList());
                        text1 = "";
                    }
                    GUILayout.EndVertical();
                });

                GUILayout.Label("Time Acceleration: " + MiscOptions.TimeAcceleration + "x", Prefab._TextStyle);
                GUILayout.Space(2);

                MiscOptions.TimeAcceleration = (int)Prefab.Slider(1, 4, MiscOptions.TimeAcceleration, 200);

                int n = MiscOptions.TimeAcceleration;
                int v = n;

                v--;
                v |= v >> 1;
                v |= v >> 2;
                v |= v >> 4;
                v |= v >> 8;
                v |= v >> 16;
                v++; // next power of 2

                int x = v >> 1; // previous power of 2
                MiscOptions.TimeAcceleration = (v - n) > (n - x) ? x : v;

                GUILayout.Space(5);

                Prefab.Toggle("Player Distance Crashing", ref MiscOptions.EnableDistanceCrash);

                if (MiscOptions.EnableDistanceCrash)
                {
                    GUILayout.Space(5);

                    GUILayout.Label("Crash Distance: " + MiscOptions.CrashDistance + "m", Prefab._TextStyle);

                    GUILayout.Space(2);

                    MiscOptions.CrashDistance = (float)Math.Round(Prefab.Slider(0, 500, MiscOptions.CrashDistance, 200), 2);
                }

                GUILayout.Space(5);

                Prefab.Toggle("Pickup Through Walls", ref MiscOptions.NearbyItemRaycast);

                GUILayout.Space(5);

                Prefab.Toggle("Extended Pickup Range", ref MiscOptions.IncreaseNearbyItemDistance);

                if (MiscOptions.IncreaseNearbyItemDistance)
                {
                    GUILayout.Space(2);
                    GUILayout.Label("Range: " + MiscOptions.NearbyItemDistance, Prefab._TextStyle);
                    GUILayout.Space(2);
                    MiscOptions.NearbyItemDistance = (float)Math.Round(Prefab.Slider(0, 20, MiscOptions.NearbyItemDistance, 200), 2);
                }

                GUILayout.Space(5);

                Prefab.Toggle("Voicechat Key Pressed", ref MiscOptions.PerpetualVoiceChat);

                GUILayout.Space(5);

                Prefab.Toggle("Zoom on Hotkey", ref MiscOptions.ZoomOnHotkey);

                if (MiscOptions.ZoomOnHotkey)
                {
                    GUILayout.Space(2);
                    Prefab.Toggle("Zoom Instantly", ref MiscOptions.InstantZoom);
                    GUILayout.Space(2);
                    GUILayout.Label("Zoom FOV: " + MiscOptions.ZoomFOV, Prefab._TextStyle);
                    GUILayout.Space(2);
                    MiscOptions.ZoomFOV = (int)Prefab.Slider(0, 32, MiscOptions.ZoomFOV, 200);
                }

                GUILayout.Space(5);

                Prefab.Toggle("Message on Kill", ref MiscOptions.MessageOnKill);

                if (MiscOptions.MessageOnKill)
                {
                    GUILayout.Space(2);
                    MiscOptions.KillMessage = Prefab.TextField(MiscOptions.KillMessage, "Message: ", 175);
                }

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            });
        }
    }
}