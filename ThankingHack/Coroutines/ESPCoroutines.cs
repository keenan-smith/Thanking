using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Thanking.Options.VisualOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Coroutines
{
	public static class ESPCoroutines
	{
		public static IEnumerator UpdateObjectList()
		{
			while (true)
			{
				if (!Provider.isConnected || Provider.isLoading)
				{
					yield return new WaitForSeconds(2);
					continue;
				}

				try
				{
					List<ESPObject> objects = ESPVariables.Objects;
					objects.Clear();

					List<ESPTarget> targets = ESPOptions.PriorityTable.Keys.OrderByDescending(k => ESPOptions.PriorityTable[k]).ToList();

					for (int i = 0; i < targets.Count; i++)
					{
						ESPTarget target = targets[i];
						ESPVisual vis = ESPOptions.VisualOptions[(int)target];

						if (!vis.Enabled)
							continue;

						Vector3 pPos = Player.player.transform.position;

						switch (target)
						{
							case ESPTarget.Players:
								{
									SteamPlayer[] objarray = Provider.clients.OrderByDescending(p => VectorUtilities.GetDistance(pPos, p.player.transform.position)).ToArray();

									if (vis.UseObjectCap)
										objarray.Take(vis.ObjectCap);

									foreach (SteamPlayer player in objarray)
									{
										Player plr = player.player;

										if (plr.life.isDead || plr == Player.player)
											continue;

										objects.Add(new ESPObject(target, plr, plr.gameObject));
									}

									break;
								}
							case ESPTarget.Zombies:
								{
									Zombie[] objarr = ZombieManager.tickingZombies.OrderByDescending(obj => VectorUtilities.GetDistance(pPos, obj.transform.position)).ToArray();

									if (vis.UseObjectCap)
										objarr.Take(vis.ObjectCap);

									for (int j = 0; j < objarr.Length; j++)
									{
										Zombie obj = objarr[j];
										objects.Add(new ESPObject(target, obj, obj.gameObject));
									}
									break;
								}
							case ESPTarget.Items:
								{
									InteractableItem[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableItem>().OrderByDescending(obj => VectorUtilities.GetDistance(pPos, obj.transform.position)).ToArray();

									if (vis.UseObjectCap)
										objarr.Take(vis.ObjectCap);

									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableItem obj = objarr[j];
										objects.Add(new ESPObject(target, obj, obj.gameObject));
									}
									break;
								}
							case ESPTarget.Sentries:
								{
									InteractableSentry[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableSentry>().OrderByDescending(obj => VectorUtilities.GetDistance(pPos, obj.transform.position)).ToArray();

									if (vis.UseObjectCap)
										objarr.Take(vis.ObjectCap);

									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableSentry obj = objarr[j];
										objects.Add(new ESPObject(target, obj, obj.gameObject));
									}
									break;
								}
							case ESPTarget.Beds:
								{
									InteractableBed[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableBed>().OrderByDescending(obj => VectorUtilities.GetDistance(pPos, obj.transform.position)).ToArray();

									if (vis.UseObjectCap)
										objarr.Take(vis.ObjectCap);

									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableBed obj = objarr[j];
										objects.Add(new ESPObject(target, obj, obj.gameObject));
									}
									break;
								}
							case ESPTarget.ClaimFlags:
								{
									InteractableClaim[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableClaim>().OrderByDescending(obj => VectorUtilities.GetDistance(pPos, obj.transform.position)).ToArray();

									if (vis.UseObjectCap)
										objarr.Take(vis.ObjectCap);

									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableClaim obj = objarr[j];
										objects.Add(new ESPObject(target, obj, obj.gameObject));
									}
									break;
								}
							case ESPTarget.Vehicles:
								{
									InteractableVehicle[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableVehicle>().OrderByDescending(obj => VectorUtilities.GetDistance(pPos, obj.transform.position)).ToArray();

									if (vis.UseObjectCap)
										objarr.Take(vis.ObjectCap);

									if (vis.UseObjectCap)
										objarr.Take(vis.ObjectCap);

									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableVehicle obj = objarr[j];
										objects.Add(new ESPObject(target, obj, obj.gameObject));
									}
									break;
								}
							case ESPTarget.Storage:
								{
									InteractableStorage[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableStorage>().OrderByDescending(obj => VectorUtilities.GetDistance(pPos, obj.transform.position)).ToArray();

									if (vis.UseObjectCap)
										objarr.Take(vis.ObjectCap);

									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableStorage obj = objarr[j];
										objects.Add(new ESPObject(target, obj, obj.gameObject));
									}
									break;
								}
							case ESPTarget.Generators:
								{
									InteractableGenerator[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableGenerator>().OrderByDescending(obj => VectorUtilities.GetDistance(pPos, obj.transform.position)).ToArray();

									if (vis.UseObjectCap)
										objarr.Take(vis.ObjectCap);

									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableGenerator obj = objarr[j];
										objects.Add(new ESPObject(target, obj, obj.gameObject));
									}

									break;
								}
						}
					}
				}
				catch(Exception e)
				{
					Debug.LogError("Error Updating ESP Objects:");
					Debug.LogError(e);
				}

				yield return new WaitForSeconds(5);
			}
		}
	}
}
