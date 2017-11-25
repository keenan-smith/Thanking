using System.Collections;
using SDG.Unturned;
using Thanking.Options;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Coroutines
{
    public static class ItemCoroutines
    {
        public static IEnumerator PickupItems()
        {
            #if DEBUG
            DebugUtilities.Log("Starting Item Coroutine");
            #endif
            
            while (true)
            {
                if (!ItemOptions.AutoItemPickup || !Provider.isConnected || Provider.isLoading ||
                        Player.player == null)
                {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }

                Collider[] array = Physics.OverlapSphere(Camera.main.transform.position, 19f, RayMasks.ITEM);

                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == null || array[i].GetComponent<InteractableItem>() == null ||
                        array[i].GetComponent<InteractableItem>().asset == null) continue;

                    InteractableItem item = array[i].GetComponent<InteractableItem>();

                    if (!ItemUtilities.Whitelisted(item.asset)) continue;
                    
                    item.use();
                }

                yield return new WaitForSeconds(ItemOptions.ItemPickupDelay / 1000);
            }
        }
    }
}
