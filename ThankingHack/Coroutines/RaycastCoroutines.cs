using System.Collections;
using System.Linq;
using SDG.Unturned;
using Thanking.Components.MultiAttach;
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
                if (!DrawUtilities.ShouldRun())
                {
                    RaycastUtilities.Objects = new GameObject[0];
                    yield return new WaitForSeconds(3);
                    continue;
                }
                
                switch (RaycastOptions.Target)
                {
                    case TargetPriority.Players: //only need to do this here 'cause players have specific properties that make it annoying to do shit with them xd
                        {
                            RaycastUtilities.Objects = Provider.clients.Where(o => !o.player.life.isDead && o.player != Player.player && FriendUtilities.IsFriendly(o.player)).Select(o => o.player.gameObject).ToArray();
                            break;
                        }       
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
                
                for (int i = 0; i < RaycastUtilities.Objects.Length; i++)
                    RaycastUtilities.Objects[i]?.AddComponent<VelocityComponent>();

                yield return new WaitForSeconds(5);
            }
        }
    }
}