using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Components.MultiAttach;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Coroutines
{
	public static class RaycastCoroutines
	{
		public static IEnumerator UpdateObjectList()
		{
			while (true)
			{
				if (!DrawUtilities.ShouldRun())
				{
					yield return new WaitForSeconds(2);
					continue;
				}

				try
				{
					Vector3 pPos = Player.player.transform.position;
					switch (RaycastOptions.Target)
					{
						case TargetPriority.Players:
							{
								RaycastUtilities.Objects = Provider.clients.Where(o => !o.player.life.isDead).Select(o => o.player.gameObject).ToArray();
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
						RaycastUtilities.Objects[i].AddComponent<VelocityComponent>();
				}
				catch (Exception e)
				{
					Debug.LogError("Error Updating Raycast Objects:");
					Debug.LogError(e);
				}

				yield return new WaitForSeconds(3);
			}
		}
	}
}
