using Thanking.Options.VisualOptions;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class SkinsTab
    {
        public static void Tab()
        {
            SkinsUtilities.DrawSkins(SkinOptions.SkinWeapons);
            SkinsUtilities.DrawSkins(SkinOptions.SkinClothesShirts);
            SkinsUtilities.DrawSkins(SkinOptions.SkinClothesPants);
            SkinsUtilities.DrawSkins(SkinOptions.SkinClothesBackpack);
            SkinsUtilities.DrawSkins(SkinOptions.SkinClothesHats);
            SkinsUtilities.DrawSkins(SkinOptions.SkinClothesMask);
            SkinsUtilities.DrawSkins(SkinOptions.SkinClothesVest);
            SkinsUtilities.DrawSkins(SkinOptions.SkinClothesGlasses);
            GUILayout.Label("Don't get too excited... All skins are client-sided meaning other's\ncan't see them! Re-equip your weapon to apply the skin.", Prefab._TextStyle);
        }
    }
}