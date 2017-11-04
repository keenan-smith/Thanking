using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Components.Basic;
using Thanking.Coroutines;
using Thanking.Options.VisualOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;
using System;
using Thanking.Options;

namespace Thanking.Components.UI
{
	[SpyComponent]
	[Component]
	public class ESPComponent : MonoBehaviour
	{
		public static Material GLMat;
		public static Font ESPFont;

		public void Start() =>
			CoroutineComponent.ESPCoroutine = StartCoroutine(ESPCoroutines.UpdateObjectList());

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

			if (!Provider.isConnected || Provider.isLoading)
				return;
			
			for (int i = 0; i < ESPVariables.Objects.Count; i++)
			{
				ESPObject obj = ESPVariables.Objects[i];
				ESPVisual visual = ESPOptions.VisualOptions[(int)obj.Target];

				Color c = visual.Color.ToColor();
				LabelLocation ll = visual.Location;

				GameObject go = obj.GObject;

				if (go == null)
					continue;

				Vector3 position = go.transform.position;
				double dist = VectorUtilities.GetDistance(position, Player.player.transform.position);

				if (dist > visual.Distance && !visual.InfiniteDistance)
					continue;

				Vector3 cpos = Camera.main.WorldToScreenPoint(position);

				if (cpos.z <= 0)
					continue;


				string text = "";
				
				Bounds b = obj.Target == ESPTarget.Players
					? new Bounds(position, go.transform.localScale)
					: go.GetComponent<Collider>().bounds;
				
				int size = DrawUtilities.GetTextSize(visual, dist);
				double rounded = Math.Round(dist);

				//Debug.Log(obj.Target);
				switch (obj.Target)
				{
					#region Players
					case ESPTarget.Players:
						{
							Player p = (Player)obj.Object;
							text = $"<size={size}>";
							
							if (ESPOptions.ShowPlayerName)
								text += p.name + "\n";
							if (ESPOptions.ShowPlayerWeapon)
								text += (p.equipment.asset != null ? p.equipment.asset.itemName : "Fists") + "\n";
							if (ESPOptions.ShowPlayerDistance)
								text += Math.Round(dist);
							
							text += "</size>";
							
							b.size = b.size / 2;
							b.size = new Vector3(b.size.x, b.size.y * 1.25f, b.size.z);

							if (p.quests.isMemberOfSameGroupAs(Player.player) && ESPOptions.UsePlayerGroup)
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

							text = $"<size={size}>{vehicle.asset.name}\n{GetLocked(vehicle)}\n{rounded}</size>";
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

							text = $"<size={size}>Generator\n{gen.fuel / gen.capacity}%\n{GetPowered(gen)}\n{rounded}</size>";
							break;
						}
						#endregion
				}

				Vector3[] vectors = DrawUtilities.GetBoxVectors(b);

                if (visual.Boxes)
                {
                    if (visual.TwoDimensional)
                        DrawUtilities.PrepareRectangleLines(Camera.main, b, c);
                    else
                        DrawUtilities.PrepareBoxLines(vectors, c);
                }

                if (visual.Labels)
                {
                    Vector3 LabelVector = DrawUtilities.GetW2SVector(Camera.main, b, ll);
                    DrawUtilities.DrawLabel(ESPFont, ll, LabelVector, text, Color.black, c, visual.BorderStrength);
                }

				if (visual.LineToObject)
					ESPVariables.DrawBuffer2.Enqueue(new ESPBox2()
					{
						Color = visual.Color.ToColor(),
						Vertices = new Vector2[]
						{
							new Vector2(Screen.width / 2, Screen.height),
							new Vector2(cpos.x, Screen.height - cpos.y)
						}
					});
			}
			
			GLMat.SetPass(0);

			GL.PushMatrix();
			GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
			GL.modelview = Camera.main.worldToCameraMatrix;
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

		public static String SentryName(Item DisplayItem) => DisplayItem != null
			? Assets.find(EAssetType.ITEM, DisplayItem.id).name
			: "<color=#ff0000ff>No Item</color>";

		public static String GetLocked(InteractableVehicle Vehicle) =>
			Vehicle.isLocked ? "<color=#ff0000ff>LOCKED</color>" : "<color=#00ff00ff>UNLOCKED</color>";

		public static String GetPowered(InteractableGenerator Generator) =>
			Generator.isPowered ? "<color=#00ff00ff>ON</color>" : "<color=#ff0000ff>OFF</color>";
	}
}
