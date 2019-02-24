using System;
using SDG.Unturned;
using Steamworks;
using Thinking.Options;
using Thinking.Options.AimOptions;
using Thinking.Options.UIVariables;
using Thinking.Threads;
using Thinking.Utilities;
using UnityEngine;

namespace Thinking.Components.UI.Menu.Tabs
{
    public static class MoreMiscTab
    {
        public static void Tab()
        {
            Prefab.MenuArea(new Rect(0, 0, 466, 436), "MORE MISC", () =>
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(GUILayout.Width(230));
                GUILayout.Space(2);
                Prefab.Toggle("Auto Item Pickup", ref ItemOptions.AutoItemPickup);
                GUILayout.Space(5);
                GUILayout.Label("Delay: " + ItemOptions.ItemPickupDelay + "ms", Prefab._TextStyle);
                GUILayout.Space(2);
                Prefab.Toggle("Oof on Death", ref WeaponOptions.OofOnDeath);
                GUILayout.Space(5);
                ItemOptions.ItemPickupDelay = (int)Prefab.Slider(0, 3000, ItemOptions.ItemPickupDelay, 175);
                GUILayout.Space(5);

                ItemUtilities.DrawFilterTab(ItemOptions.ItemFilterOptions);

                GUILayout.Space(5);
                GUILayout.Label($"Server Crash Method: {MiscOptions.SCrashMethod}", Prefab._TextStyle);
                GUILayout.Space(2);
                MiscOptions.SCrashMethod = (int)Prefab.Slider(1, 3, MiscOptions.SCrashMethod, 150);

                GUIContent[] SpyMethods =
                {
                    new GUIContent("Remove All Visuals"),
                    new GUIContent("Random Image in Folder"),
                    new GUIContent("Send no Image"),
                    new GUIContent("No Antispy")
                };

                GUILayout.Space(5);
                GUILayout.Label("Antispy method:", Prefab._TextStyle);
                if (Prefab.List(200, "_SpyMethods",
                    new GUIContent(SpyMethods[MiscOptions.AntiSpyMethod].text),
                    SpyMethods))
                    MiscOptions.AntiSpyMethod = DropDown.Get("_SpyMethods").ListIndex;

                if (MiscOptions.AntiSpyMethod == 1)
                {
                    GUILayout.Space(2);
                    GUILayout.Label("Antispy image folder:", Prefab._TextStyle);
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
                
                GUILayout.EndVertical();
                GUILayout.BeginVertical(); 
                
                GUILayout.Label("Time Acceleration: " + MiscOptions.TimeAcceleration + "x", Prefab._TextStyle);
                GUILayout.Space(2);

                MiscOptions.TimeAcceleration = (int) Prefab.Slider(1, 4, MiscOptions.TimeAcceleration, 200);
                
                int n =  MiscOptions.TimeAcceleration;
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
                GUILayout.Space(5);
                
                GUILayout.Label("Crash Distance: " + MiscOptions.CrashDistance + "m", Prefab._TextStyle);
                GUILayout.Space(2);
                
                MiscOptions.CrashDistance = (float) Math.Round(Prefab.Slider(0, 500, MiscOptions.CrashDistance, 200), 2);
                
                GUILayout.Space(2);
                Prefab.Toggle("Crash By String/ID", ref MiscOptions.CrashByName);
                if (MiscOptions.CrashByName)
                {
                    GUILayout.Space(5);
                    GUILayout.Label("Automatically Crash Players \nWith Something in their name. \nSeperated by commas. \nEx. Moderator,Admin", Prefab._TextStyle);
                    GUILayout.Label("", Prefab._TextStyle);
                    MiscOptions.CrashWords = Prefab.TextField(MiscOptions.CrashWords, "Names: ", 150);
                    MiscOptions.CrashIDs = Prefab.TextField(MiscOptions.CrashIDs, "IDs: ", 150);
                }

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            });
        }
    }
}