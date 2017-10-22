using SDG.Unturned;
using System.Collections.Generic;
using Thanking.Misc;
using Thanking.Attributes;
using Thanking.Components.Basic;
using Thanking.Coroutines;
using Thanking.Options.VisualOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;
using System;

namespace Thanking.Components.UI
{
	[UIComponent]
	[Component]
	public class ESPComponent : MonoBehaviour
	{
		public static Material mat = null;

		public void Start() =>
			CoroutineComponent.ESPCoroutine = StartCoroutine(ESPCoroutines.UpdateObjectList());

		public void OnGUI()
		{
			if (Event.current.type != EventType.Repaint || !ESPOptions.Enabled)
				return;

			Camera cam = Camera.main;
			for (int i = 0; i < ESPVariables.Objects.Count; i++)
			{
				ESPObject obj = ESPVariables.Objects[i];

				int targ = (int)obj.Target;
				ESPVisual visual = ESPOptions.VisualOptions[targ];

				Color c = visual.Color.ToColor();
				LabelLocation ll = visual.Location;

				try
				{
					switch (obj.Target)
					{
						#region Players
						case ESPTarget.Players:
							{
								Player p = (Player)obj.Object;

								if (p == null)
									continue;

								Vector3 position = p.transform.position;
								float dist = VectorUtilities.GetDistance(position, Player.player.transform.position);

								if (dist > visual.Distance && !visual.InfiniteDistance)
									continue;

								Vector3 cpos = cam.WorldToScreenPoint(position);

								if (cpos.z <= 0)
									continue;

								string text = string.Format("<size=12>{0}\n{1}\n{2}</size>", p.name, p.equipment.asset != null ? p.equipment.asset.itemName : "Fists", Mathf.Round(dist));
								
								Bounds b = p.GetComponentInChildren<Renderer>().bounds;

								b.size = b.size / 2;
								b.size = new Vector3(b.size.x, b.size.y * 1.25f, b.size.z);

								Vector3[] vectors = DrawUtilities.GetBoxVectors(b);

								/*
								if (visual.Rectangle)
								{
									vectors = DrawUtilities.GetRectVectors(p.transform, b, vectors);
									DrawUtilities.PrepareRectangleLines(vectors, c);
								}
								else
								DrawUtilities.PrepareBoxLines(vectors, c);
								*/

								Vector3 LabelVector = DrawUtilities.GetW2SVector(cam, b, vectors, ll);
								DrawUtilities.DrawLabel(ll, LabelVector, text, Color.black, c, 4);

								break;
							}
						#endregion
						#region Items
						case ESPTarget.Items:
							{
								InteractableItem item = (InteractableItem)obj.Object;

								if (item == null)
									continue;

								Vector3 position = item.transform.position;
								Vector3 cpos = cam.WorldToScreenPoint(position);

								if (cpos.z <= 0)
									continue;

								Bounds b = item.GetComponentInChildren<Renderer>().bounds;
								Vector3[] vectors = DrawUtilities.GetBoxVectors(b);

								string text = string.Format("<size=12>{0}\n{1}</size>", item.asset.itemName, Mathf.Round(VectorUtilities.GetDistance(Player.player.transform.position, position)));

								/*
								if (visual.Rectangle)
								{
									vectors = DrawUtilities.GetRectVectors(item.transform, b, vectors);
									DrawUtilities.PrepareRectangleLines(vectors, c);
								}
								else
								DrawUtilities.PrepareBoxLines(vectors, c);
								*/

								Vector3 LabelVector = DrawUtilities.GetW2SVector(cam, b, vectors, ll);
								DrawUtilities.DrawLabel(ll, LabelVector, text, Color.black, c, 4);
								break;
							}
						#endregion
					}
				}
				catch (Exception ex) { Debug.LogError(ex); }
			}
			
			GL.PushMatrix();

			GL.LoadIdentity();
			GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
			GL.MultMatrix(Camera.main.worldToCameraMatrix);


			for (int i = 0; i < ESPVariables.DrawBuffer.Count; i++)
			{
				ESPBox box = ESPVariables.DrawBuffer[i];
				GL.Color(box.Color);

				Vector3[] vertices = box.Vertices;
				for (int j = 0; j < vertices.Length; j++)
					GL.Vertex(vertices[j]);
			}

			GL.PopMatrix();
			GL.Flush();
		}
	}
}
