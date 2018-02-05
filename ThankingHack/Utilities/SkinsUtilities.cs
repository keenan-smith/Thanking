using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Components.UI.Menu;
using Thanking.Options;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Utilities
{
    public static class SkinsUtilities
    {
        private static HumanClothes CharacterClothes => OptimizationVariables.MainPlayer.clothing.characterClothes;
        private static HumanClothes FirstClothes => OptimizationVariables.MainPlayer.clothing.firstClothes;
        private static HumanClothes ThirdClothes => OptimizationVariables.MainPlayer.clothing.thirdClothes;

        public static Vector2 ScrollPos;
        private static string SearchString = "";

        public static void Apply(Skin skin, ESkinType skinType)
        {
            if (skinType == ESkinType.WEAPONS)
            {
                Dictionary<ushort, int> skins = OptimizationVariables.MainPlayer.channel.owner.itemSkins;
                if (skins == null) return;

                ushort inventoryItemID = Provider.provider.economyService.getInventoryItemID(skin.ID);

                SkinOptions.SkinConfig.WeaponSkins.Clear();

                if (skins.TryGetValue(inventoryItemID, out int value))
                    skins[inventoryItemID] = skin.ID;
                else
                    skins.Add(inventoryItemID, skin.ID);

                OptimizationVariables.MainPlayer.equipment.applySkinVisual();
                OptimizationVariables.MainPlayer.equipment.applyMythicVisual();

                foreach (KeyValuePair<ushort, int> pair in skins)
                    SkinOptions.SkinConfig.WeaponSkins.Add(new WeaponSave(pair.Key, pair.Value));
            }
            else
                ApplyClothing(skin, skinType);
        }

        private static void ApplyClothing(Skin skin, ESkinType type)
        {
            switch (type)
            {
                case ESkinType.SHIRTS:
                    CharacterClothes.visualShirt = skin.ID;
                    FirstClothes.visualShirt = skin.ID;
                    ThirdClothes.visualShirt = skin.ID;
                    SkinOptions.SkinConfig.ShirtID = skin.ID;
                    break;
                case ESkinType.PANTS:
                    CharacterClothes.visualPants = skin.ID;
                    FirstClothes.visualPants = skin.ID;
                    ThirdClothes.visualPants = skin.ID;
                    SkinOptions.SkinConfig.PantsID = skin.ID;
                    break;
                case ESkinType.BACKPACKS:
                    CharacterClothes.visualBackpack = skin.ID;
                    FirstClothes.visualBackpack = skin.ID;
                    ThirdClothes.visualBackpack = skin.ID;
                    SkinOptions.SkinConfig.BackpackID = skin.ID;
                    break;
                case ESkinType.VESTS:
                    CharacterClothes.visualVest = skin.ID;
                    FirstClothes.visualVest = skin.ID;
                    ThirdClothes.visualVest = skin.ID;
                    SkinOptions.SkinConfig.VestID = skin.ID;
                    break;
                case ESkinType.HATS:
                    CharacterClothes.visualHat = skin.ID;
                    FirstClothes.visualHat = skin.ID;
                    ThirdClothes.visualHat = skin.ID;
                    SkinOptions.SkinConfig.HatID = skin.ID;
                    break;
                case ESkinType.MASKS:
                    CharacterClothes.visualMask = skin.ID;
                    FirstClothes.visualMask = skin.ID;
                    ThirdClothes.visualMask = skin.ID;
                    SkinOptions.SkinConfig.MaskID = skin.ID;
                    break;
                case ESkinType.GLASSES:
                    CharacterClothes.visualGlasses = skin.ID;
                    FirstClothes.visualGlasses = skin.ID;
                    ThirdClothes.visualGlasses = skin.ID;
                    SkinOptions.SkinConfig.GlassesID = skin.ID;
                    break;
            }
            CharacterClothes.apply();
            FirstClothes.apply();
            ThirdClothes.apply();
        }

        public static void ApplyFromConfig()
        {
            Dictionary<ushort, int> skins = new Dictionary<ushort, int>();
            foreach (WeaponSave save in SkinOptions.SkinConfig.WeaponSkins)
                skins[save.WeaponID] = save.SkinID;
            OptimizationVariables.MainPlayer.channel.owner.itemSkins = skins;

            if (SkinOptions.SkinConfig.ShirtID != 0)
            {
                CharacterClothes.visualShirt = SkinOptions.SkinConfig.ShirtID;
                FirstClothes.visualShirt = SkinOptions.SkinConfig.ShirtID;
                ThirdClothes.visualShirt = SkinOptions.SkinConfig.ShirtID;
            }

            if (SkinOptions.SkinConfig.PantsID != 0)
            {
                CharacterClothes.visualPants = SkinOptions.SkinConfig.PantsID;
                FirstClothes.visualPants = SkinOptions.SkinConfig.PantsID;
                ThirdClothes.visualPants = SkinOptions.SkinConfig.PantsID;
            }

            if (SkinOptions.SkinConfig.BackpackID != 0)
            {
                CharacterClothes.visualBackpack = SkinOptions.SkinConfig.BackpackID;
                FirstClothes.visualBackpack = SkinOptions.SkinConfig.BackpackID;
                ThirdClothes.visualBackpack = SkinOptions.SkinConfig.BackpackID;
            }

            if (SkinOptions.SkinConfig.VestID != 0)
            {
                CharacterClothes.visualVest = SkinOptions.SkinConfig.VestID;
                FirstClothes.visualVest = SkinOptions.SkinConfig.VestID;
                ThirdClothes.visualVest = SkinOptions.SkinConfig.VestID;
            }

            if (SkinOptions.SkinConfig.HatID != 0)
            {
                CharacterClothes.visualHat = SkinOptions.SkinConfig.HatID;
                FirstClothes.visualHat = SkinOptions.SkinConfig.HatID;
                ThirdClothes.visualHat = SkinOptions.SkinConfig.HatID;
            }

            if (SkinOptions.SkinConfig.MaskID != 0)
            {
                CharacterClothes.visualMask = SkinOptions.SkinConfig.MaskID;
                FirstClothes.visualMask = SkinOptions.SkinConfig.MaskID;
                ThirdClothes.visualMask = SkinOptions.SkinConfig.MaskID;
            }

            if (SkinOptions.SkinConfig.GlassesID != 0)
            {
                CharacterClothes.visualGlasses = SkinOptions.SkinConfig.GlassesID;
                FirstClothes.visualGlasses = SkinOptions.SkinConfig.GlassesID;
                ThirdClothes.visualGlasses = SkinOptions.SkinConfig.GlassesID;
            }

            CharacterClothes.apply();
            FirstClothes.apply();
            ThirdClothes.apply();
        }

        public static void DrawSkins(SkinOptionList OptionList)
        {
            Prefab.SectionTabButton(OptionList.Type.ToString(), () =>
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(60);
                SearchString = Prefab.TextField(SearchString, "Search:", 480);
                GUILayout.EndHorizontal();
                Prefab.ScrollView(new Rect(70, 15 + 25, 540, 420 - 25), OptionList.Type.ToString(), ref ScrollPos, () =>
                {
                    foreach (Skin skin in OptionList.Skins) // haha xd kr4ken's gonna get mad :D
                    {
                        bool isShown = skin.Name.ToLower().Contains(SearchString.ToLower()) ? true : false;

                        if (isShown)
                            if (Prefab.Button(skin.Name, 540 - 45))
                                Apply(skin, OptionList.Type);
                    }
                });
            });
        }
    }
}