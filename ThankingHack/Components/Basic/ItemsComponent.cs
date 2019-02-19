using System.Collections.Generic;
using SDG.Unturned;
using Thinking.Attributes;
using Thinking.Coroutines;
using UnityEngine;

namespace Thinking.Components.Basic
{
    /// <summary>
    /// Component used to track items and start the pickup coroutine
    /// </summary>
    [Component]
    public class ItemsComponent : MonoBehaviour
    {
        public static List<ItemAsset> items = new List<ItemAsset>();

        public static void RefreshItems() //Loop through all possible items and add them to the list if they exist
        {
            items.Clear();
            for (ushort i = 0; i < ushort.MaxValue; i++)
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
