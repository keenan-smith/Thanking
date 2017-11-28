using System.Collections.Generic;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Coroutines;
using UnityEngine;

namespace Thanking.Components.Basic
{
    [Component]
    public class ItemsComponent : MonoBehaviour
    {
        public static List<ItemAsset> items = new List<ItemAsset>();
        public static ushort uAmount = ushort.MaxValue;

        public static void RefreshItems()
        {
            items.Clear();
            for (ushort i = 0; i < uAmount; i++)
            {
                ItemAsset asset = (ItemAsset)Assets.find(EAssetType.ITEM, i);

                if (!string.IsNullOrEmpty(asset?.itemName) && !items.Contains(asset))
                    items.Add(asset);
            }
        }

        public void Start() =>
			CoroutineComponent.ItemPickupCoroutine = StartCoroutine(ItemCoroutines.PickupItems());
	}
}
