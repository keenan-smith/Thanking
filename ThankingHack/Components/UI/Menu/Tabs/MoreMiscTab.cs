using SDG.Unturned;
using Thanking.Options;
using Thanking.Options.AimOptions;
using Thanking.Options.UIVariables;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class MoreMiscTab
    {
        public static void Tab()
        {
            Prefab.MenuArea(new Rect(0, 0, 466, 436), "MORE MISC", () =>
            {
                GUILayout.Space(2);
                Prefab.Toggle("Auto Item Pickup", ref ItemOptions.AutoItemPickup);
                GUILayout.Space(5);
                GUILayout.Label("Delay: " + ItemOptions.ItemPickupDelay + "ms", Prefab._TextStyle);
                GUILayout.Space(2);
                ItemOptions.ItemPickupDelay = (int) Prefab.Slider(0, 3000, ItemOptions.ItemPickupDelay, 175);
                GUILayout.Space(5);

                ItemUtilities.DrawFilterTab(ItemOptions.ItemFilterOptions);
                
                GUILayout.Space(5);
                GUILayout.Label($"Player Crash Method: {MiscOptions.PCrashMethod}", Prefab._TextStyle);
                GUILayout.Space(2);
                MiscOptions.PCrashMethod = (int) Prefab.Slider(1, 5, (float)MiscOptions.PCrashMethod, 150);
                
                GUILayout.Space(5);
                GUILayout.Label($"Server Crash Method: {MiscOptions.SCrashMethod}", Prefab._TextStyle);
                GUILayout.Space(2);
                MiscOptions.SCrashMethod = (int) Prefab.Slider(1, 3, (float)MiscOptions.SCrashMethod, 150);

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
                    new GUIContent(SpyMethods[DropDown.Get("_SpyMethods").ListIndex].text),
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
            });
        }
    }
}