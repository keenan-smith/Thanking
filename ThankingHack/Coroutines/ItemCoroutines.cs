using System.Collections;
using SDG.Unturned;
using Thanking.Options;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Coroutines
{
    public static class ItemCoroutines
    {
        public static IEnumerator PickupItems()
        {
           // #if DEBUG
            DebugUtilities.Log("Starting Item Coroutine");
          //  #endif
            
            while (true)
            {
                if (!DrawUtilities.ShouldRun() || !ItemOptions.AutoItemPickup)
                {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }
                
                Collider[] array = Physics.OverlapSphere(OptimizationVariables.MainPlayer.transform.position, 19f, RayMasks.ITEM);

                for (int i = 0; i < array.Length; i++)
                {
                    Collider col = array[i];
                    if (col == null || col.GetComponent<InteractableItem>() == null ||
                        col.GetComponent<InteractableItem>().asset == null) continue;

                    InteractableItem item = col.GetComponent<InteractableItem>();

                    if (!ItemUtilities.Whitelisted(item.asset, ItemOptions.ItemFilterOptions)) 
                        continue;
                    
                    item.use();
                }

                yield return new WaitForSeconds(ItemOptions.ItemPickupDelay / 1000);
            }
        }
    }
}
