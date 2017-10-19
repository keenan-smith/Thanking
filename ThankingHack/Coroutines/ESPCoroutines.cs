using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Components.UI;
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
				List<ESPObject> objects = ESPVariables.Objects;
				objects.Clear();

				for (int i = 0; i < ESPOptions.EnabledOptions.Length; i++)
				{
					if (ESPOptions.EnabledOptions[i])
					{
						//i would make this nicer but it looks like i can't because of generic type argument shit
						ESPTarget target = (ESPTarget)i;
						switch (target)
						{
							case ESPTarget.Players:
								{
									foreach (SteamPlayer player in Provider.clients)
									{
										Player plr = player.player;

										if (plr.life.isDead || plr == Player.player)
											continue;

										ESPVariables.Objects.Add(new ESPObject(ESPTarget.Players, player.player));
									}
									break;
								}
							case ESPTarget.Items:
								{
									InteractableItem[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableItem>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableItem obj = objarr[j];
										ESPVariables.Objects.Add(new ESPObject(ESPTarget.Items, obj));
									}

									break;
								}
							case ESPTarget.Sentries:
								{
									InteractableSentry[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableSentry>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableSentry obj = objarr[j];
										ESPVariables.Objects.Add(new ESPObject(ESPTarget.Sentries, obj));
									}

									break;
								}
							case ESPTarget.Beds:
								{
									InteractableBed[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableBed>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableBed obj = objarr[j];
										ESPVariables.Objects.Add(new ESPObject(ESPTarget.Beds, obj));
									}

									break;
								}
							case ESPTarget.ClaimFlags:
								{
									InteractableClaim[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableClaim>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableClaim obj = objarr[j];
										ESPVariables.Objects.Add(new ESPObject(ESPTarget.ClaimFlags, obj));
									}

									break;
								}
							case ESPTarget.Vehicles:
								{
									InteractableVehicle[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableVehicle>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableVehicle obj = objarr[j];
										ESPVariables.Objects.Add(new ESPObject(ESPTarget.Vehicles, obj));
									}

									break;
								}
							case ESPTarget.Storage:
								{
									InteractableStorage[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableStorage>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableStorage obj = objarr[j];
										ESPVariables.Objects.Add(new ESPObject(ESPTarget.Storage, obj));
									}

									break;
								}
							case ESPTarget.Generators:
								{
									InteractableGenerator[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableGenerator>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableGenerator obj = objarr[j];
										ESPVariables.Objects.Add(new ESPObject(ESPTarget.Generators, obj));
									}

									break;
								}
						}
					}
				}

				yield return new WaitForSeconds(2);
			}
		}
	}
}
