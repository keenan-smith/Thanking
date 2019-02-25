using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SDG.Unturned;
using Thanking.Misc;
using Thanking.Misc.Enums;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

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
                    RaycastUtilities.Objects.Clear();
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

                    RaycastUtilities.Objects.Clear();

                    foreach (TargetPriority target in RaycastOptions.Targets)
                    {
                        switch (target)
                        {
                            case TargetPriority.Players:
                            {
                                CachedPlayers.Clear();
                                foreach (GameObject g in gameObjects)
                                {
                                    Player p = DamageTool.getPlayer(g.transform);
                                    if (p == null || CachedPlayers.Contains(p) ||
                                        p == OptimizationVariables.MainPlayer || p.life.isDead)
                                        continue;

                                    CachedPlayers.Add(p);
                                }

                                RaycastUtilities.Objects.AddRange(CachedPlayers.Select(c => c.gameObject));
                                break;
                            }
                            case TargetPriority.Zombies:
                            {
                                RaycastUtilities.Objects.AddRange(
                                    gameObjects.Where(g => g.GetComponent<Zombie>() != null)
                                        .ToArray());
                                break;
                            }
                            case TargetPriority.Sentries:
                            {
                                RaycastUtilities.Objects.AddRange(gameObjects.Where(g =>
                                    g.GetComponent<InteractableSentry>() != null));
                                break;
                            }
                            case TargetPriority.Beds:
                            {
                                RaycastUtilities.Objects.AddRange(gameObjects.Where(g =>
                                    g.GetComponent<InteractableBed>() != null));
                                break;
                            }
                            case TargetPriority.ClaimFlags:
                            {
                                RaycastUtilities.Objects.AddRange(gameObjects.Where(g =>
                                    g.GetComponent<InteractableClaim>() != null));
                                break;
                            }
                            case TargetPriority.Vehicles:
                            {
                                RaycastUtilities.Objects.AddRange(gameObjects.Where(g =>
                                    g.GetComponent<InteractableVehicle>() != null));
                                break;
                            }
                            case TargetPriority.Storage:
                            {
                                RaycastUtilities.Objects.AddRange(gameObjects.Where(g =>
                                    g.GetComponent<InteractableStorage>() != null));
                                break;
                            }
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