using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Misc;
using Thanking.Misc.Classes.ESP;
using Thanking.Misc.Enums;
using Thanking.Options;
using Thanking.Options.VisualOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Thanking.Coroutines
{
	public static class ESPCoroutines
    {
        public static Shader LitChams;
        public static Shader UnlitChams;
        public static Shader OutlineShader;
        public static Shader Normal;
	    
        public static IEnumerator DoChams()
        {
	        #if DEBUG
	        DebugUtilities.Log("Starting Chams Coroutine");
	        #endif
	        
            while (true)
            {
                if (!DrawUtilities.ShouldRun() || UnlitChams == null)
                {
                    yield return new WaitForSeconds(1f);

                    continue;
                }
                try
                {
                    if (ESPOptions.ChamsEnabled)
                        EnableChams();
                    else
                        DisableChams();
                }
                catch (Exception e) { Debug.LogException(e); }
                yield return new WaitForSeconds(5);
            }
        }

        public static void DoChamsGameObject(GameObject pgo, Color32 front, Color32 behind)
        {
            if (UnlitChams == null) return;

            Renderer[] rds = pgo.GetComponentsInChildren<Renderer>();

            for (int j = 0; j < rds.Length; j++)
            {
	            if (!(rds[j].material.shader != LitChams | UnlitChams)) continue;
	            
	            Material[] materials = rds[j].materials;

	            for (int k = 0; k < materials.Length; k++)
	            {
		            materials[k].shader = ESPOptions.ChamsFlat ? UnlitChams : LitChams;
		            
		            materials[k].SetColor("_ColorVisible", new Color32(front.r, front.g, front.b, front.a));
		            materials[k].SetColor("_ColorBehind", new Color32(behind.r, behind.g, behind.b, behind.a));
	            }
            }
        }

        [OffSpy]
        public static void EnableChams()
        {
	        if (!ESPOptions.ChamsEnabled) return;

	        Color32 friendly_front = ColorUtilities.getColor("_ChamsFriendVisible");
	        Color32 friendly_back = ColorUtilities.getColor("_ChamsFriendInvisible");
	        Color32 enemy_front = ColorUtilities.getColor("_ChamsEnemyVisible");
	        Color32 enemy_back = ColorUtilities.getColor("_ChamsEnemyInvisible");

	        SteamPlayer[] players = Provider.clients.ToArray();
	        for (int index = 0; index < players.Length; index++)
	        {
		        SteamPlayer p = players[index];
		        Color32 front = FriendUtilities.IsFriendly(p.player) ? friendly_front : enemy_front;
		        Color32 back = FriendUtilities.IsFriendly(p.player) ? friendly_back : enemy_back;

		        Player plr = p.player;

		        if (plr == null || plr == OptimizationVariables.MainPlayer || plr.gameObject == null || plr.life == null ||
		            plr.life.isDead) continue;

		        GameObject pgo = plr.gameObject;
		        DoChamsGameObject(pgo, front, back);
	        }
        }

        [OnSpy]
        public static void DisableChams()
        {
            if (Normal == null) return;

            for (int index = 0; index < Provider.clients.ToArray().Length; index++)
            {
                Player plr = Provider.clients.ToArray()[index].player;

                if (plr == null || plr == OptimizationVariables.MainPlayer || plr.life == null ||
                    plr.life.isDead) continue;

                GameObject pgo = plr.gameObject;

                Renderer[] renderers = pgo.GetComponentsInChildren<Renderer>();

                for (int j = 0; j < renderers.Length; j++)
                {
                    Material[] materials = renderers[j].materials;

                    for (int k = 0; k < materials.Length; k++)
                        if (materials[k].shader != Normal)
                            materials[k].shader = Normal;
                }
            }
        }

		public static IEnumerator UpdateObjectList()
		{
			while (true)
			{
				if (!DrawUtilities.ShouldRun())
				{
					yield return new WaitForSeconds(2);
					continue;
				}
				List<ESPObject> objects = ESPVariables.Objects;
				objects.Clear();

				List<ESPTarget> targets =
					ESPOptions.PriorityTable.Keys.OrderByDescending(k => ESPOptions.PriorityTable[k]).ToList();

				for (int i = 0; i < targets.Count; i++)
				{
					ESPTarget target = targets[i];
					ESPVisual vis = ESPOptions.VisualOptions[(int) target];

					if (!vis.Enabled)
						continue;

					Vector3 pPos = OptimizationVariables.MainPlayer.transform.position;

					switch (target)
					{
						case ESPTarget.Players:
						{
							SteamPlayer[] objarray = Provider.clients
								.OrderByDescending(p => VectorUtilities.GetDistance(pPos, p.player.transform.position)).ToArray();

							if (vis.UseObjectCap)
								objarray = objarray.TakeLast(vis.ObjectCap).ToArray();

							for (int j = 0; j < objarray.Length; j++)
							{
								SteamPlayer sPlayer = objarray[j];
								Player plr = sPlayer.player;

								if (plr.life.isDead || plr == OptimizationVariables.MainPlayer)
									continue;

								objects.Add(new ESPObject(target, plr, plr.gameObject));
							}
							break;
						}
						case ESPTarget.Zombies:
						{
							Zombie[] objarr = ZombieManager.regions.SelectMany(r => r.zombies)
								.OrderByDescending(obj => VectorUtilities.GetDistance(pPos, obj.transform.position)).ToArray();

							if (vis.UseObjectCap)
								objarr = objarr.TakeLast(vis.ObjectCap).ToArray();

							for (int j = 0; j < objarr.Length; j++)
							{
								Zombie obj = objarr[j];
								objects.Add(new ESPObject(target, obj, obj.gameObject));
							}

							break;
						}
						case ESPTarget.Items:
						{
							InteractableItem[] objarr = Object.FindObjectsOfType<InteractableItem>()
								.OrderByDescending(obj => VectorUtilities.GetDistance(pPos, obj.transform.position)).ToArray();

							if (vis.UseObjectCap)
								objarr = objarr.TakeLast(vis.ObjectCap).ToArray();

							for (int j = 0; j < objarr.Length; j++)
							{
								InteractableItem obj = objarr[j];

								if (ItemUtilities.Whitelisted(obj.asset, ItemOptions.ItemESPOptions) || !ESPOptions.FilterItems)
									objects.Add(new ESPObject(target, obj, obj.gameObject));
							}
							break;
						}
						case ESPTarget.Sentries:
						{
							InteractableSentry[] objarr = Object.FindObjectsOfType<InteractableSentry>()
								.OrderByDescending(obj => VectorUtilities.GetDistance(pPos, obj.transform.position)).ToArray();

							if (vis.UseObjectCap)
								objarr = objarr.TakeLast(vis.ObjectCap).ToArray();

							for (int j = 0; j < objarr.Length; j++)
							{
								InteractableSentry obj = objarr[j];
								objects.Add(new ESPObject(target, obj, obj.gameObject));
							}
							break;
						}
						case ESPTarget.Beds:
						{
							InteractableBed[] objarr = Object.FindObjectsOfType<InteractableBed>()
								.OrderByDescending(obj => VectorUtilities.GetDistance(pPos, obj.transform.position)).ToArray();

							if (vis.UseObjectCap)
								objarr = objarr.TakeLast(vis.ObjectCap).ToArray();

							for (int j = 0; j < objarr.Length; j++)
							{
								InteractableBed obj = objarr[j];
								objects.Add(new ESPObject(target, obj, obj.gameObject));
							}
							break;
						}
						case ESPTarget.ClaimFlags:
						{
							InteractableClaim[] objarr = Object.FindObjectsOfType<InteractableClaim>()
								.OrderByDescending(obj => VectorUtilities.GetDistance(pPos, obj.transform.position)).ToArray();

							if (vis.UseObjectCap)
								objarr = objarr.TakeLast(vis.ObjectCap).ToArray();

							for (int j = 0; j < objarr.Length; j++)
							{
								InteractableClaim obj = objarr[j];
								objects.Add(new ESPObject(target, obj, obj.gameObject));
							}
							break;
						}
						case ESPTarget.Vehicles:
						{
							InteractableVehicle[] objarr = Object.FindObjectsOfType<InteractableVehicle>()
								.OrderByDescending(obj => VectorUtilities.GetDistance(pPos, obj.transform.position)).ToArray();

							if (vis.UseObjectCap)
								objarr = objarr.TakeLast(vis.ObjectCap).ToArray();

							for (int j = 0; j < objarr.Length; j++)
							{
								InteractableVehicle obj = objarr[j];

								if (obj.isDead)
									continue;

								objects.Add(new ESPObject(target, obj, obj.gameObject));
							}
							break;
						}
						case ESPTarget.Storage:
						{
							InteractableStorage[] objarr = Object.FindObjectsOfType<InteractableStorage>()
								.OrderByDescending(obj => VectorUtilities.GetDistance(pPos, obj.transform.position)).ToArray();

							if (vis.UseObjectCap)
								objarr = objarr.TakeLast(vis.ObjectCap).ToArray();

							for (int j = 0; j < objarr.Length; j++)
							{
								InteractableStorage obj = objarr[j];
								objects.Add(new ESPObject(target, obj, obj.gameObject));
							}
							break;
						}
						case ESPTarget.Generators:
						{
							InteractableGenerator[] objarr = Object.FindObjectsOfType<InteractableGenerator>()
								.OrderByDescending(obj => VectorUtilities.GetDistance(pPos, obj.transform.position)).ToArray();

							if (vis.UseObjectCap)
								objarr = objarr.TakeLast(vis.ObjectCap).ToArray();

							for (int j = 0; j < objarr.Length; j++)
							{
								InteractableGenerator obj = objarr[j];
								objects.Add(new ESPObject(target, obj, obj.gameObject));
							}
							break;
						}
					}
				}
				yield return new WaitForSeconds(5);
			}
		}
	}
}
