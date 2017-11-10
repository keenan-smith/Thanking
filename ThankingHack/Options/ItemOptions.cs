using System.Collections.Generic;
using Thanking.Attributes;

namespace Thanking.Options
{
    public static class ItemOptions
    {
        [Save] public static HashSet<ushort> AddedItems = new HashSet<ushort>();
        [Save] public static bool AutoItemPickup = false;
        [Save] public static bool ItemfilterGun = false;
        [Save] public static bool ItemfilterAmmo = false;
        [Save] public static bool ItemfilterMedical = false;
        [Save] public static bool ItemfilterBackpack = false;
        [Save] public static bool ItemfilterCharges = false;
        [Save] public static bool ItemfilterFuel = false;
        [Save] public static bool ItemfilterClothing = false;
        [Save] public static bool ItemfilterFoodAndWater = false;
        [Save] public static bool ItemfilterCustom = true;
        [Save] public static int ItemPickupDelay = 1000;
    }
}
