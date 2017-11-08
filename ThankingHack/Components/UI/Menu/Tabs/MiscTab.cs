using System;
using Thanking.Options;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class MiscTab
    {
        public static void Tab()
        {
            Prefab.MenuArea(new Rect(0, 0, 466, 436), "MISC", () =>
            {
                Prefab.Toggle("Vehicle Flight", ref MiscOptions.VehicleFly);
                GUILayout.Space(2);
                GUILayout.Label("Speed Multiplier: " + MiscOptions.SpeedMultiplier + "x", Prefab._TextStyle);
                GUILayout.Space(2);
                MiscOptions.SpeedMultiplier = (float)Math.Round(Prefab.Slider(0, 10, MiscOptions.SpeedMultiplier, 175), 2);
                Prefab.MenuArea(new Rect(10, 436 - 125 - 10, 220, 125), "SPAMMER", () =>
                {
                    Prefab.Toggle("Enabled", ref MiscOptions.SpammerEnabled);
                    GUILayout.Space(5);
                    MiscOptions.SpamText = Prefab.TextField(MiscOptions.SpamText, "Text: ", 150);
                    GUILayout.Space(10);
                    GUILayout.Label("Delay: " + MiscOptions.SpammerDelay + "ms", Prefab._TextStyle);
                    GUILayout.Space(5);
                    MiscOptions.SpammerDelay = (int)Prefab.Slider(0, 3000, MiscOptions.SpammerDelay, 175);
                });
                Prefab.MenuArea(new Rect(220 + 10 + 5, 436 - 125 - 10, 221, 125), "INTERACT", () =>
                {
                    Prefab.Toggle("Hit Structures", ref InteractionOptions.HitStructures);
                    Prefab.Toggle("Hit Barricades", ref InteractionOptions.HitBarricades);
                    Prefab.Toggle("Hit Items", ref InteractionOptions.HitItems);
                    Prefab.Toggle("Hit Vehicles", ref InteractionOptions.HitVehicles);
                    Prefab.Toggle("Hit Resources", ref InteractionOptions.HitResources);
                });
            });
        }
    }
}