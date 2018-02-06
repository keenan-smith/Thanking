using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Coroutines;
using UnityEngine;
using Thanking.Options;
using Thanking.Utilities;
using Thanking.Variables;

namespace Thanking.Overrides
{
    public class OV_PlayerInventory : MonoBehaviour
	{
        [Override(typeof(PlayerInventory), "has", BindingFlags.Public | BindingFlags.Instance)]
        public InventorySearch has(ushort id)
        {
            if (DrawUtilities.ShouldRun())
            {
                if (id == 1176 && MiscOptions.GPS) return new InventorySearch(0, new ItemJar(new Item(1176, false)));
                if (id == 1508 && MiscOptions.Compass) return new InventorySearch(0, new ItemJar(new Item(1508, false)));
            }
            
            for (byte index = 0; index < PlayerInventory.PAGES - 1; index += 1)
            {
                InventorySearch inventorySearch = OptimizationVariables.MainPlayer.inventory.items[index].has(id);
                if (inventorySearch != null)
                    return inventorySearch;
            }
            return null;
        }
    }
}