using Thanking.Options;
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
                GUILayout.Label("Delay: " + ItemOptions.ItemPickupDelay + "ms", Prefab._TextStyle);
                GUILayout.Space(2);
                ItemOptions.ItemPickupDelay = (int) Prefab.Slider(0, 3000, ItemOptions.ItemPickupDelay, 175);
                GUILayout.Space(5);

                ItemUtilities.DrawFilterTab(ItemOptions.ItemFilterOptions);
            });
        }
    }
}