using System.Collections;
using System.Linq;
using SDG.Unturned;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Coroutines
{
    public class RaycastCoroutines
    {
        public static IEnumerator UpdateObjects()
        {
            while (true)
            {
                switch (RaycastOptions.Target)
                {
                        
                    case TargetPriority.Zombies:
                    {
                        RaycastUtilities.Objects = UnityEngine.Object.FindObjectsOfType<Zombie>().Select(z => z.gameObject).ToArray();
                        break;
                    }
                    case TargetPriority.Sentries:
                    {
                        RaycastUtilities.Objects = UnityEngine.Object.FindObjectsOfType<InteractableSentry>().Select(s => s.gameObject).ToArray();
                        break;
                    }
                    case TargetPriority.Beds:
                    {
                        RaycastUtilities.Objects = UnityEngine.Object.FindObjectsOfType<InteractableBed>().Select(b => b.gameObject).ToArray();
                        break;
                    }
                    case TargetPriority.ClaimFlags:
                    {
                        RaycastUtilities.Objects = UnityEngine.Object.FindObjectsOfType<InteractableClaim>().Select(c => c.gameObject).ToArray();
                        break;
                    }
                    case TargetPriority.Vehicles:
                    {
                        RaycastUtilities.Objects = UnityEngine.Object.FindObjectsOfType<InteractableVehicle>().Select(s => s.gameObject).ToArray();
                        break;
                    }
                    case TargetPriority.Storage:
                    {
                        RaycastUtilities.Objects = UnityEngine.Object.FindObjectsOfType<InteractableStorage>().Select(s => s.gameObject).ToArray();
                        break;
                    }
                }

                yield return new WaitForSeconds(5);
            }
        }
    }
}