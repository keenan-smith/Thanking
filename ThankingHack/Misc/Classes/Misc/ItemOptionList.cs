using System.Collections.Generic;
using Thanking.Misc.Serializables;

namespace Thanking.Misc.Classes.Misc
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
		public SerializableVector2 additemscroll = new SerializableVector2(0, 0);
		public SerializableVector2 removeitemscroll = new SerializableVector2(0, 0);
	}
}