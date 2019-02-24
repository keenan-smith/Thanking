using System.Collections.Generic;
using SDG.Unturned;
using Thanking.Components.Basic;
using Thanking.Components.UI.Menu;
using Thanking.Misc.Classes.Misc;
using Thanking.Options;
using UnityEngine;

namespace Thanking.Utilities
{
    public static class ItemUtilities
    {
        public static bool Whitelisted(ItemAsset asset, ItemOptionList OptionList)
        {
            if (OptionList.ItemfilterCustom && OptionList.AddedItems.Contains(asset.id))
                return true;
            if (OptionList.ItemfilterGun && asset is ItemGunAsset)
                return true;
            if (OptionList.ItemfilterAmmo && asset is ItemMagazineAsset)
                return true;
            if (OptionList.ItemfilterMedical && asset is ItemMedicalAsset)
                return true;
            if (OptionList.ItemfilterFoodAndWater && (asset is ItemFoodAsset || asset is ItemWaterAsset))
                return true;
            if (OptionList.ItemfilterBackpack && asset is ItemBackpackAsset)
                return true;
            if (OptionList.ItemfilterCharges && asset is ItemChargeAsset)
                return true;
            if (OptionList.ItemfilterFuel && asset is ItemFuelAsset)
                return true;
            if (OptionList.ItemfilterClothing && asset is ItemClothingAsset)
                return true;

            return false;
        }

		public static void DrawItemButton(ItemAsset asset, HashSet<ushort> AddedItems)
		{
			string name = asset.itemName;
			if (asset.itemName.Length > 60)
			{
				name = asset.itemName.Substring(0, 60) + "..";
			}

			if (Prefab.Button(name, 490))
			{
				if (AddedItems.Contains(asset.id))
					AddedItems.Remove(asset.id);
				else
					AddedItems.Add(asset.id);
			}
			GUILayout.Space(3);
		}

		public static void DrawFilterTab(ItemOptionList OptionList)
		{
			Prefab.SectionTabButton("ITEM FILTER", () =>
			{
				Prefab.Toggle("Guns", ref OptionList.ItemfilterGun);
				Prefab.Toggle("Ammo", ref OptionList.ItemfilterAmmo);
				Prefab.Toggle("Medical", ref OptionList.ItemfilterMedical);
				Prefab.Toggle("Backpacks", ref OptionList.ItemfilterBackpack);
				Prefab.Toggle("Charges", ref OptionList.ItemfilterCharges);
				Prefab.Toggle("Fuel", ref OptionList.ItemfilterFuel);
				Prefab.Toggle("Clothing", ref OptionList.ItemfilterClothing);
				Prefab.Toggle("Food and Water", ref OptionList.ItemfilterFoodAndWater);
				Prefab.Toggle("Enable Custom Filter", ref OptionList.ItemfilterCustom);
				if (OptionList.ItemfilterCustom)
				{
					GUILayout.Space(5);
					Prefab.SectionTabButton("CUSTOM FILTER", () =>
					{
						GUILayout.BeginHorizontal();
						GUILayout.Space(55);
						OptionList.searchstring = Prefab.TextField(OptionList.searchstring, "Search:", 200);
						GUILayout.Space(5);
						
						if (Prefab.Button("Refresh", 276))
							ItemsComponent.RefreshItems();
						
						GUILayout.FlexibleSpace();
						GUILayout.EndHorizontal();
						Prefab.ScrollView(new Rect(70, 0 + 50, 620 - 70 - 10, 190), "Add", ref OptionList.additemscroll, () =>
						{
							GUILayout.Space(5);
							for (var i = 0; i < ItemsComponent.items.Count; i++)
							{
								ItemAsset asset = ItemsComponent.items[i];
								bool isShown = false;

								if (asset.itemName.ToLower().Contains(OptionList.searchstring.ToLower()))
									isShown = true;
								if (OptionList.searchstring.Length < 2)
									isShown = false;
								if (OptionList.AddedItems.Contains(asset.id))
									isShown = false;

								if (isShown)
									DrawItemButton(asset, OptionList.AddedItems);
							}
							GUILayout.Space(2);
						});
						Prefab.ScrollView(new Rect(70, 200 + 5 + 40, 620 - 70 - 10, 191), "Remove", ref OptionList.removeitemscroll, () =>
						{
							GUILayout.Space(5);
							for (var i = 0; i < ItemsComponent.items.Count; i++)
							{
								ItemAsset asset = ItemsComponent.items[i];
								bool isShown = false;

								if (asset.itemName.ToLower().Contains(OptionList.searchstring.ToLower()))
									isShown = true;
								if (!OptionList.AddedItems.Contains(asset.id))
									isShown = false;

								if (isShown)
									DrawItemButton(asset, OptionList.AddedItems);
							}
							GUILayout.Space(2);
						});
					});
				}
			});
		}
	}
}
