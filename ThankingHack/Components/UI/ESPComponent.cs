using System;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Components.Basic;
using Thanking.Coroutines;
using Thanking.Options;
using Thanking.Options.VisualOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;
using HighlightingSystem;
using System.Collections.Generic;
using System.Linq;
using Thanking.Managers.Main;

namespace Thanking.Components.UI
{
	[SpyComponent]
	[Component]
	public class ESPComponent : MonoBehaviour
	{
		public static Material GLMat;
		public static Font ESPFont;

        private static List<Highlighter> highlighters = new List<Highlighter>();

        public static Camera MainCamera;

		public void Start()
        {
            CoroutineComponent.ESPCoroutine = StartCoroutine(ESPCoroutines.UpdateObjectList());
            CoroutineComponent.ChamsCoroutine = StartCoroutine(ESPCoroutines.DoChams());
            MainCamera = Camera.main; // lets define it once ok?

			for (int i = 0; i < ESPOptions.VisualOptions.Length; i++)
				ColorUtilities.addColor(new Options.UIVariables.ColorVariable($"_{(ESPTarget)i}", $"ESP - {(ESPTarget)i}", new Color32(255, 0, 0, 255)));
        }

        public void Update()
        {
            if (MiscOptions.NoRain)
                LevelLighting.rainyness = ELightingRain.NONE;
            if (MiscOptions.NoSnow)
                LevelLighting.snowyness = ELightingSnow.NONE;
        }

		public void OnGUI()
        {
            if (Event.current.type != EventType.Repaint || !ESPOptions.Enabled)
				return;

			if (!DrawUtilities.ShouldRun())
				return;

			GUI.depth = 1;

            if (MainCamera == null)
                MainCamera = Camera.main;

            Vector3 localPos = OptimizationVariables.MainPlayer.transform.position;

            for (int i = 0; i < ESPVariables.Objects.Count; i++)
			{
				ESPObject obj = ESPVariables.Objects[i];
				ESPVisual visual = ESPOptions.VisualOptions[(int)obj.Target];

				if (!visual.Enabled)
					continue;

                if (obj.Target == ESPTarget.Items && ESPOptions.FilterItems)
                    if (!ItemUtilities.Whitelisted(((InteractableItem)obj.Object).asset, ItemOptions.ItemESPOptions))
                        continue;

				Color c = ColorUtilities.getColor($"_{obj.Target}");
				LabelLocation ll = visual.Location;

				GameObject go = obj.GObject;

				if (go == null)
					continue;

				Vector3 position = go.transform.position;
				double dist = VectorUtilities.GetDistance(position, localPos);

				if (dist > visual.Distance && !visual.InfiniteDistance)
					continue;

				Vector3 cpos = MainCamera.WorldToScreenPoint(position);

				if (cpos.z <= 0)
					continue;


				string text = "";
				string outerText = null;

				Vector3 scale = go.transform.localScale;
				Bounds b = obj.Target == ESPTarget.Players || obj.Target == ESPTarget.Zombies //zombies have the same issue as players where shit doesn't exist for some fucking reason
					? new Bounds(new Vector3(position.x, position.y + 1, position.z), new Vector3(scale.x * 2, scale.y * 3, scale.z * 2))
					: go.GetComponent<Collider>().bounds;

                int size = DrawUtilities.GetTextSize(visual, dist);
				double rounded = Math.Round(dist);

				/*#if DEBUG
				DebugUtilities.Log(obj.Target.ToString()); //Holy fuck nuggets this is laggy
				#endif*/
				
				switch (obj.Target)
				{
					#region Players
					case ESPTarget.Players:
						{
							Player p = (Player)obj.Object;

							if (p.life.isDead)
								continue;

							text = $"<size={size}>";
							
							if (ESPOptions.ShowPlayerName)
								text += p.name + "\n";
							if (ESPOptions.ShowPlayerWeapon)
								text += (p.equipment.asset != null ? p.equipment.asset.itemName : "Fists") + "\n";
							if (ESPOptions.ShowPlayerVehicle)
								text += (p.movement.getVehicle() != null ? p.movement.getVehicle().asset.name : "No Vehicle") + "\n";
							if (ESPOptions.ShowPlayerDistance)
								text += Math.Round(dist);
							
							if(NetManager.Heccers != null)
								if (ESPOptions.ShowHeccers)
									if (NetManager.Heccers.ToList().Contains(PlayerTool.getSteamPlayer(p.name).playerID.steamID.m_SteamID))
										text += "\nHeccer";

							text += "</size>";
							
							b.size = b.size / 2;
							b.size = new Vector3(b.size.x, b.size.y * 1.25f, b.size.z);

							if (FriendUtilities.IsFriendly(p) && ESPOptions.UsePlayerGroup)
								c = ESPOptions.SameGroupColor.ToColor();

							break;
						}
					#endregion
					#region Zombies
					case ESPTarget.Zombies:
						{
							text = $"<size={size}>Zombie\n{rounded}</size>";
							break;
						}
					#endregion
					#region Items
					case ESPTarget.Items:
						{
							InteractableItem item = (InteractableItem)obj.Object;

							text = $"<size={size}>{item.asset.itemName}\n{rounded}</size>";
							break;
						}
					#endregion
					#region Sentries
					case ESPTarget.Sentries:
						{
							InteractableSentry sentry = (InteractableSentry)obj.Object;

							text = $"<size={size}>Sentry\n{SentryName(sentry.displayItem)}\n{rounded}</size>";
							break;
						}
					#endregion
					#region Beds
					case ESPTarget.Beds:
						{
							InteractableBed bed = (InteractableBed)obj.Object;

							text = $"<size={size}>Bed\n{rounded}</size>";
							break;
						}
					#endregion
					#region Claim Flags
					case ESPTarget.ClaimFlags:
						{
							InteractableClaim flag = (InteractableClaim)obj.Object;

							text = $"<size={size}>Claim Flag\n{rounded}</size>";
							break;
						}
					#endregion
					#region Vehicles
					case ESPTarget.Vehicles:
						{
							InteractableVehicle vehicle = (InteractableVehicle)obj.Object;

							if (vehicle.isExploded)
								return;

							text = $"<size={size}>{vehicle.asset.name}\n{GetLocked(vehicle, true)}\n{rounded}</size>";
							outerText = $"<size={size}>{vehicle.asset.name}\n{GetLocked(vehicle, false)}\n{rounded}</size>";
							break;
						}
					#endregion
					#region Storage
					case ESPTarget.Storage:
						{
							InteractableStorage stor = (InteractableStorage)obj.Object;

							text = $"<size={size}>Storage\n{rounded}</size>";
							break;
						}
					#endregion
					#region Generators
					case ESPTarget.Generators:
						{
							InteractableGenerator gen = (InteractableGenerator)obj.Object;

							text = $"<size={size}>Generator\n{gen.fuel / gen.capacity}%\n{GetPowered(gen, true)}\n{rounded}</size>";
							outerText = $"<size={size}>Generator\n{gen.fuel / gen.capacity}%\n{GetPowered(gen, false)}\n{rounded}</size>";
							break;
						}
						#endregion
				}

				Vector3[] vectors = DrawUtilities.GetBoxVectors(b);

                if (visual.Boxes)
                {
                    if (visual.TwoDimensional)
                        DrawUtilities.PrepareRectangleLines(MainCamera, b, c);
                    else
                        DrawUtilities.PrepareBoxLines(vectors, c);
                }

                if (visual.Glow)
                {
                    Highlighter highlighter = go.GetComponent<Highlighter>();
                    if (highlighter == null)
                    {
                        highlighter = go.AddComponent<Highlighter>();
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOnImmediate();
                        highlighters.Add(highlighter);
                    }
                }
                else
                {
                    Highlighter highlighter = go.GetComponent<Highlighter>();
                    if (highlighter != null)
                    {
                        highlighter.ConstantOffImmediate();
                        highlighter.Die();
                    }
                }

                if (visual.Labels)
                {
                    Vector3 LabelVector = DrawUtilities.GetW2SVector(MainCamera, b, ll);
                    DrawUtilities.DrawLabel(ESPFont, ll, LabelVector, text, Color.black, c, visual.BorderStrength, outerText);
                }

				if (visual.LineToObject)
					ESPVariables.DrawBuffer2.Enqueue(new ESPBox2
					{
						Color = c,
						Vertices = new[]
						{
							new Vector2(Screen.width / 2, Screen.height),
							new Vector2(cpos.x, Screen.height - cpos.y)
						}
					});
			}
			
			GLMat.SetPass(0);

			GL.PushMatrix();
			GL.LoadProjectionMatrix(MainCamera.projectionMatrix);
			GL.modelview = MainCamera.worldToCameraMatrix;
			GL.Begin(GL.LINES);

			for (int i = 0; i < ESPVariables.DrawBuffer.Count; i++)
			{
				ESPBox box = ESPVariables.DrawBuffer.Dequeue();

				GL.Color(box.Color);

				Vector3[] vertices = box.Vertices;
				for (int j = 0; j < vertices.Length; j++)
					GL.Vertex(vertices[j]);

			}
			GL.End();
			GL.PopMatrix();

			GL.PushMatrix();
			GL.Begin(GL.LINES);

			for (int i = 0; i < ESPVariables.DrawBuffer2.Count; i++)
			{
				ESPBox2 box = ESPVariables.DrawBuffer2.Dequeue();

				GL.Color(box.Color);

				Vector2[] vertices = box.Vertices;
				for (int j = 0; j < vertices.Length; j++)
					GL.Vertex3(vertices[j].x, vertices[j].y, 0);

			}
			GL.End();
			GL.PopMatrix();
        }

        [OnSpy]
        private static void DisableHighlighters()
        {
            foreach(Highlighter highlighter in highlighters) // pls dont go apeshit kr4ken its only called once every spy and it's not like update() where its called every milisecond
            {
                highlighter.ConstantOffImmediate();
                highlighter.Die();
            }
            highlighters.Clear();
        }

		public static String SentryName(Item DisplayItem) => DisplayItem != null
			? Assets.find(EAssetType.ITEM, DisplayItem.id).name
			: "<color=#ff0000ff>No Item</color>";

		public static String GetLocked(InteractableVehicle Vehicle, bool color) =>
			Vehicle.isLocked ? color ? "<color=#ff0000ff>LOCKED</color>" : "LOCKED" : color ? "<color=#00ff00ff>UNLOCKED</color>" : "LOCKED";

		public static String GetPowered(InteractableGenerator Generator, bool color) =>
			Generator.isPowered ? color ? "<color=#00ff00ff>ON</color>" : "ON" : color ? "<color=#ff0000ff>OFF</color>" : "OFF";
	}
}
