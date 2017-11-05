using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class ItemFilterTab
    {
        public static Vector2 additemscroll;
        public static Vector2 removeitemscroll;
        public static string searchstring = "";
        public static void Tab()
        {
            searchstring = Prefab.TextField(searchstring, "Search:", 200);
            GUILayout.Space(5);
            if (Prefab.Button("Refresh", 466))
            {

            }
            Prefab.ScrollView(new Rect(0, 0 + 50, 466, 190), "Add", ref additemscroll, () =>
            {

            });
            Prefab.ScrollView(new Rect(0, 200 + 5 + 40, 466, 191), "Remove", ref removeitemscroll, () =>
            {

            });
        }
    }
}
