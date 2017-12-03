using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SDG.Unturned;
using Thanking.Components.Basic;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Thanking.Coroutines
{
    public class RaycastCoroutines
    {
        public static IEnumerator UpdateObjects()
        {
            while (true)
            {
                if (!DrawUtilities.ShouldRun())
                {
                    RaycastUtilities.Objects = new GameObject[0];
                    yield return new WaitForSeconds(1);
                    continue;
                }
                
                try
                {
                    switch (RaycastOptions.Target)
                    {
                        case TargetPriority.Players:
                        {
                            RaycastUtilities.Objects = Provider.clients
                                .Where(o => !o.player.life.isDead && o.player != Player.player &&
                                            FriendUtilities.IsFriendly(o.player)).Select(o => o.player.gameObject)
                                .ToArray();
                            break;
                        }
                        case TargetPriority.Zombies:
                        {
                            RaycastUtilities.Objects = UnityEngine.Object.FindObjectsOfType<Zombie>()
                                .Select(z => z.gameObject).ToArray();
                            break;
                        }
                        case TargetPriority.Sentries:
                        {
                            RaycastUtilities.Objects = UnityEngine.Object.FindObjectsOfType<InteractableSentry>()
                                .Select(s => s.gameObject).ToArray();
                            break;
                        }
                        case TargetPriority.Beds:
                        {
                            RaycastUtilities.Objects = UnityEngine.Object.FindObjectsOfType<InteractableBed>()
                                .Select(b => b.gameObject).ToArray();
                            break;
                        }
                        case TargetPriority.ClaimFlags:
                        {
                            RaycastUtilities.Objects = UnityEngine.Object.FindObjectsOfType<InteractableClaim>()
                                .Select(c => c.gameObject).ToArray();
                            break;
                        }
                        case TargetPriority.Vehicles:
                        {
                            RaycastUtilities.Objects = UnityEngine.Object.FindObjectsOfType<InteractableVehicle>()
                                .Select(s => s.gameObject).ToArray();
                            break;
                        }
                        case TargetPriority.Storage:
                        {
                            RaycastUtilities.Objects = UnityEngine.Object.FindObjectsOfType<InteractableStorage>()
                                .Select(s => s.gameObject).ToArray();
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    #if DEBUG
                    DebugUtilities.LogException(e);
                    #endif
                }
                
                yield return new WaitForSeconds(5);
            }
        }
    }
}