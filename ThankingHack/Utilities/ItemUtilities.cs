using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Options;

namespace Thanking.Utilities
{
    public static class ItemUtilities
    {
        public static bool Whitelisted(ItemAsset asset)
        {
            if (ItemOptions.ItemfilterCustom && ItemOptions.AddedItems.Contains(asset.id))
                return true;
            if (ItemOptions.ItemfilterGun && asset is ItemGunAsset)
                return true;
            if (ItemOptions.ItemfilterAmmo && asset is ItemMagazineAsset)
                return true;
            if (ItemOptions.ItemfilterMedical && asset is ItemMedicalAsset)
                return true;
            if (ItemOptions.ItemfilterFoodAndWater && (asset is ItemFoodAsset || asset is ItemWaterAsset))
                return true;
            if (ItemOptions.ItemfilterBackpack && asset is ItemBackpackAsset)
                return true;
            if (ItemOptions.ItemfilterCharges && asset is ItemChargeAsset)
                return true;
            if (ItemOptions.ItemfilterFuel && asset is ItemFuelAsset)
                return true;
            if (ItemOptions.ItemfilterClothing && asset is ItemClothingAsset)
                return true;

            return false;
        }
    }
}
