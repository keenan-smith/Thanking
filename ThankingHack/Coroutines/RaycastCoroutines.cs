using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SDG.Unturned;
using Thanking.Components.Basic;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Thanking.Coroutines
{
    public class RaycastCoroutines
    {
        public static List<Player> CachedPlayers = new List<Player>();
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
                    ItemGunAsset currentGun = OptimizationVariables.MainPlayer.equipment.asset as ItemGunAsset;
                    float Range = currentGun?.range ?? 15.5f;
                    Range += 10f;
                    
                    GameObject[] gameObjects =
                        Physics.OverlapSphere(OptimizationVariables.MainPlayer.transform.position, Range).Select(c => c.gameObject).ToArray();

                    
                    switch (RaycastOptions.Target)
                    {
                        case TargetPriority.Players:
                        {
                            CachedPlayers.Clear();
                            foreach (GameObject g in gameObjects)
                            {
                                Player p = DamageTool.getPlayer(g.transform);
                                if (p == null || CachedPlayers.Contains(p) || p == OptimizationVariables.MainPlayer || p.life.isDead)
                                    continue;

                                CachedPlayers.Add(p);
                            }

                            RaycastUtilities.Objects = CachedPlayers.Select(c => c.gameObject).ToArray();
                            break;
                        }
                        case TargetPriority.Zombies:
                        {
                            RaycastUtilities.Objects =
                                gameObjects.Where(g => g.GetComponent<Zombie>() != null).ToArray();
                            break;
                        }
                        case TargetPriority.Sentries:
                        {
                            RaycastUtilities.Objects =
                                gameObjects.Where(g => g.GetComponent<InteractableSentry>() != null).ToArray();
                            break;
                        }
                        case TargetPriority.Beds:
                        {
                            RaycastUtilities.Objects =
                                gameObjects.Where(g => g.GetComponent<InteractableBed>() != null).ToArray();
                            break;
                        }
                        case TargetPriority.ClaimFlags:
                        {
                            RaycastUtilities.Objects =
                                gameObjects.Where(g => g.GetComponent<InteractableClaim>() != null).ToArray();
                            break;
                        }
                        case TargetPriority.Vehicles:
                        {
                            RaycastUtilities.Objects =
                                gameObjects.Where(g => g.GetComponent<InteractableVehicle>() != null).ToArray();
                            break;
                        }
                        case TargetPriority.Storage:
                        {
                            RaycastUtilities.Objects =
                                gameObjects.Where(g => g.GetComponent<InteractableStorage>() != null).ToArray();
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    DebugUtilities.LogException(e);
                }
                
                yield return new WaitForSeconds(2);
            }
        }
    }
}