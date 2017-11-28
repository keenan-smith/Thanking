using System.Collections.Generic;
using Thanking.Attributes;
using UnityEngine;

namespace Thanking.Options
{
    public class ItemOptionList
    {
        public HashSet<ushort> AddedItems = new HashSet<ushort>();
        public bool ItemfilterGun = false;
        public bool ItemfilterAmmo = false;
        public bool ItemfilterMedical = false;
        public bool ItemfilterBackpack = false;
        public bool ItemfilterCharges = false;
        public bool ItemfilterFuel = false;
        public bool ItemfilterClothing = false;
        public bool ItemfilterFoodAndWater = false;
        public bool ItemfilterCustom = true;
		public string searchstring = "";
		public Vector2 additemscroll = Vector2.zero;
		public Vector2 removeitemscroll = Vector2.zero;
    }

	public static class ItemOptions
	{
		[Save] public static bool AutoItemPickup = false;
		[Save] public static int ItemPickupDelay = 1000;
		[Save] public static ItemOptionList ItemFilterOptions;
		[Save] public static ItemOptionList ItemESPOptions;
	}
}
