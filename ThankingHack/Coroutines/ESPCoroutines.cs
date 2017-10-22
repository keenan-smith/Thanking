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

				Dictionary<ESPTarget, List<object>> objDict = new Dictionary<ESPTarget, List<object>>();

				for (int i = 0; i < ESPOptions.VisualOptions.Length; i++)
				{
					if (ESPOptions.VisualOptions[i].Enabled)
					{
						//i would make this nicer but it looks like i can't because of generic type argument shit
						ESPTarget target = (ESPTarget)i;
						objDict.Add(target, null);

						switch (target)
						{
							case ESPTarget.Players:
								{
									List<Player> ps = new List<Player>();

									foreach (SteamPlayer player in Provider.clients)
									{
										Player plr = player.player;

										if (plr.life.isDead || plr == Player.player)
											continue;

										ps.Add(plr);
									}

									ps.OrderBy(p => VectorUtilities.GetDistance(p.transform.position, Player.player.transform.position));

									objDict[ESPTarget.Players] = ps.Select(p => (object)p).ToList();
									break;
								}
							case ESPTarget.Items:
								{
									List<InteractableItem> objs = new List<InteractableItem>();

									InteractableItem[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableItem>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableItem obj = objarr[j];
										objs.Add(obj);
									}

									objs.OrderBy(o => VectorUtilities.GetDistance(Player.player.transform.position, o.transform.position));

									objDict[ESPTarget.Items] = objs.Select(o => (object)o).ToList();
									break;
								}
							case ESPTarget.Sentries:
								{
									List<InteractableSentry> objs = new List<InteractableSentry>();

									InteractableSentry[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableSentry>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableSentry obj = objarr[j];
										objs.Add(obj);
									}

									objs.OrderBy(o => VectorUtilities.GetDistance(Player.player.transform.position, o.transform.position));

									objDict[ESPTarget.Sentries] = objs.Select(o => (object)o).ToList();

									break;
								}
							case ESPTarget.Beds:
								{
									List<InteractableBed> objs = new List<InteractableBed>();

									InteractableBed[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableBed>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableBed obj = objarr[j];
										objs.Add(obj);
									}

									objs.OrderBy(o => VectorUtilities.GetDistance(Player.player.transform.position, o.transform.position));

									objDict[ESPTarget.Beds] = objs.Select(o => (object)o).ToList();

									break;
								}
							case ESPTarget.ClaimFlags:
								{
									List<InteractableClaim> objs = new List<InteractableClaim>();

									InteractableClaim[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableClaim>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableClaim obj = objarr[j];
										objs.Add(obj);
									}

									objs.OrderBy(o => VectorUtilities.GetDistance(Player.player.transform.position, o.transform.position));

									objDict[ESPTarget.ClaimFlags] = objs.Select(o => (object)o).ToList();

									break;
								}
							case ESPTarget.Vehicles:
								{
									List<InteractableVehicle> objs = new List<InteractableVehicle>();

									InteractableVehicle[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableVehicle>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableVehicle obj = objarr[j];
										objs.Add(obj);
									}

									objs.OrderBy(o => VectorUtilities.GetDistance(Player.player.transform.position, o.transform.position));

									objDict[ESPTarget.Vehicles] = objs.Select(o => (object)o).ToList();

									break;
								}
							case ESPTarget.Storage:
								{
									List<InteractableStorage> objs = new List<InteractableStorage>();

									InteractableStorage[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableStorage>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableStorage obj = objarr[j];
										objs.Add(obj);
									}

									objs.OrderBy(o => VectorUtilities.GetDistance(Player.player.transform.position, o.transform.position));

									objDict[ESPTarget.Storage] = objs.Select(o => (object)o).ToList();

									break;
								}
							case ESPTarget.Generators:
								{
									List<InteractableGenerator> objs = new List<InteractableGenerator>();

									InteractableGenerator[] objarr = UnityEngine.Object.FindObjectsOfType<InteractableGenerator>();
									for (int j = 0; j < objarr.Length; j++)
									{
										InteractableGenerator obj = objarr[j];
										objs.Add(obj);
									}

									objs.OrderBy(o => VectorUtilities.GetDistance(Player.player.transform.position, o.transform.position));

									objDict[ESPTarget.Generators] = objs.Select(o => (object)o).ToList();

									break;
								}
						}
					}
				}
				objects.Clear();

				foreach (ESPTarget t in objDict.Keys.OrderByDescending(o => ESPOptions.PriorityTable[o]).ToList())
					for (int i = 0; i < objDict[t].Count; i++)
						ESPVariables.Objects.Add(new ESPObject(t, objDict[t][i]));

				yield return new WaitForSeconds(5);
			}
		}
	}
}
