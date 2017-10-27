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

				//Dictionary<ESPTarget, List<object>> objDict = new Dictionary<ESPTarget, List<object>>();
				objects.Clear();

				for (int i = 0; i < ESPOptions.VisualOptions.Length; i++)
				{
					if (ESPOptions.VisualOptions[i].Enabled)
					{
						//i would make this nicer but it looks like i can't because of generic type argument shit
						ESPTarget target = (ESPTarget)i;
						//objDict.Add(target, null);

						switch (target)
						{
							case ESPTarget.Players:
								{
									foreach (SteamPlayer player in Provider.clients)
									{
										Player plr = player.player;

										if (plr.life.isDead || plr == Player.player)
											continue;

										objects.Add(new ESPObject(target, plr));
									}

									break;
								}
							case ESPTarget.Items:
								{
									//List<InteractableItem> objs = new List<InteractableItem>();

									InteractableItem[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableItem>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableItem obj = objarr[j];
										objects.Add(new ESPObject(target, obj));
									}
									break;
								}
							case ESPTarget.Sentries:
								{
									InteractableSentry[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableSentry>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableSentry obj = objarr[j];
										objects.Add(new ESPObject(target, obj));
									}
									break;
								}
							case ESPTarget.Beds:
								{
									InteractableBed[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableBed>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableBed obj = objarr[j];
										objects.Add(new ESPObject(target, obj));
									}
									break;
								}
							case ESPTarget.ClaimFlags:
								{
									InteractableClaim[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableClaim>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableClaim obj = objarr[j];
										objects.Add(new ESPObject(target, obj));
									}
									break;
								}
							case ESPTarget.Vehicles:
								{
									InteractableVehicle[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableVehicle>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableVehicle obj = objarr[j];
										objects.Add(new ESPObject(target, obj));
									}
									break;
								}
							case ESPTarget.Storage:
								{
									InteractableStorage[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableStorage>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableStorage obj = objarr[j];
										objects.Add(new ESPObject(target, obj));
									}
									break;
								}
							case ESPTarget.Generators:
								{
									InteractableGenerator[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableGenerator>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableGenerator obj = objarr[j];
										objects.Add(new ESPObject(target, obj));
									}

									break;
								}
						}
					}
				}
				yield return new WaitForSeconds(5);
			}
		}
	}
}
