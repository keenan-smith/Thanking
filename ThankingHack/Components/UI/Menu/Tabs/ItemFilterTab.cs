using SDG.Unturned;
using Thanking.Components.Basic;
using Thanking.Options;
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
            Prefab.MenuArea(new Rect(0, 0, 466, 436), "ITEM FILTER", () =>
            {
                Prefab.Toggle("Auto Item Pickup", ref ItemOptions.AutoItemPickup);
                GUILayout.Space(2);
                GUILayout.Label("Delay: " + ItemOptions.ItemPickupDelay + "ms", Prefab._TextStyle);
                GUILayout.Space(2);
                ItemOptions.ItemPickupDelay = (int)Prefab.Slider(0, 3000, ItemOptions.ItemPickupDelay, 175);
                GUILayout.Space(5);
                Prefab.Toggle("Guns", ref ItemOptions.ItemfilterGun);
                Prefab.Toggle("Ammo", ref ItemOptions.ItemfilterAmmo);
                Prefab.Toggle("Medical", ref ItemOptions.ItemfilterMedical);
                Prefab.Toggle("Backpacks", ref ItemOptions.ItemfilterBackpack);
                Prefab.Toggle("Charges", ref ItemOptions.ItemfilterCharges);
                Prefab.Toggle("Fuel", ref ItemOptions.ItemfilterFuel);
                Prefab.Toggle("Clothing", ref ItemOptions.ItemfilterClothing);
                Prefab.Toggle("Food and Water", ref ItemOptions.ItemfilterFoodAndWater);
                Prefab.Toggle("Enable Custom Filter", ref ItemOptions.ItemfilterCustom);
                GUILayout.Space(5);
                Prefab.SectionTabButton("CUSTOM FILTER", () =>
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(55);
                    searchstring = Prefab.TextField(searchstring, "Search:", 200);
                    GUILayout.Space(5);
                    if (Prefab.Button("Refresh", 276))
                    {
                        ItemsComponent.RefreshItems();
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    Prefab.ScrollView(new Rect(70, 0 + 50, 620 - 70 - 10, 190), "Add", ref additemscroll, () =>
                    {
                        GUILayout.Space(5);
                        for (var i = 0; i < ItemsComponent.items.Count; i++)
                        {
                            ItemAsset asset = ItemsComponent.items[i];
                            bool isShown = false;

                            if (asset.itemName.ToLower().Contains(searchstring.ToLower()))
                                isShown = true;
                            if (searchstring.Length < 2)
                                isShown = false;
                            if (ItemOptions.AddedItems.Contains(asset.id))
                                isShown = false;

                            if (isShown)
                                DrawItemButton(asset);
                        }
                        GUILayout.Space(2);
                    });
                    Prefab.ScrollView(new Rect(70, 200 + 5 + 40, 620 - 70 - 10, 191), "Remove", ref removeitemscroll, () =>
                    {
                        GUILayout.Space(5);
                        for (var i = 0; i < ItemsComponent.items.Count; i++)
                        {
                            ItemAsset asset = ItemsComponent.items[i];
                            bool isShown = false;

                            if (asset.itemName.ToLower().Contains(searchstring.ToLower()))
                                isShown = true;
                            if (!ItemOptions.AddedItems.Contains(asset.id))
                                isShown = false;

                            if (isShown)
                                DrawItemButton(asset);
                        }
                        GUILayout.Space(2);
                    });
                });
            });
        }

        public static void DrawItemButton(ItemAsset asset)
        {
            string name = asset.itemName;
            if (asset.itemName.Length > 60)
            {
                name = asset.itemName.Substring(0, 60) + "..";
            }

            if (Prefab.Button(name, 490))
            {
                if (ItemOptions.AddedItems.Contains(asset.id))
                    ItemOptions.AddedItems.Remove(asset.id);
                else
                    ItemOptions.AddedItems.Add(asset.id);
            }
            GUILayout.Space(3);
        }
    }
}
