using SDG.Unturned;
using System.Collections.Generic;
using Thanking.Attributes;
using Thanking.Components.Basic;
using Thanking.Coroutines;
using Thanking.Options.VisualOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Components.UI
{
	[UIComponent]
	[Component]
	public class ESPComponent : MonoBehaviour
	{
		public static Material mat = null;

		public void Start()
		{
			CoroutineComponent.ESPCoroutine = StartCoroutine(ESPCoroutines.UpdateObjectList());
		}

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

				switch (obj.Target)
				{
					case ESPTarget.Players:
						{
							Player p = (Player)obj.Object;

							if (p == null)
								continue;

							Vector3 position = p.transform.position;
							Vector3 cpos = cam.WorldToScreenPoint(position);

							if (cpos.z <= 0)
								continue;
							
							string text = string.Format("<size=12><color=#{0}>{1}\n{2}</color></size>", DrawUtilities.ColorToHex(c), p.name, Mathf.Round(VectorUtilities.GetDistance(Player.player.transform.position, position)));

							Bounds b = new Bounds();
							Vector3[] vectors;
							Vector3 LabelVector = Vector3.zero;
							if (!visual.Rectangle)
							{
								Renderer re = p.GetComponentInChildren<Renderer>();
								b = re.bounds;

								vectors = DrawUtilities.GetBoxVectors(b);
								LabelVector = DrawUtilities.GetW2SVector(cam, b, vectors, ll);

								DrawUtilities.PrepareBoxLines(vectors, c);
							}
							else
							{
								b = p.GetComponentInChildren<Collider>().bounds;

								vectors = DrawUtilities.GetRectangleVectors(p.transform, b);
								LabelVector = DrawUtilities.GetW2SVector(cam, b, vectors, ll);

								DrawUtilities.PrepareRectangleLines(vectors, c);
							}

							b.size = b.size / 2;
							b.size = new Vector3(b.size.x, b.size.y * 1.25f, b.size.z);
							
							DrawUtilities.DrawLabel(ll, LabelVector, text);
							break;
						}
					case ESPTarget.Items:
						{
							InteractableItem item = (InteractableItem)obj.Object;

							if (item == null)
								continue;

							Vector3 position = item.transform.position;
							Vector3 cpos = cam.WorldToScreenPoint(position);

							if (cpos.z <= 0)
								continue;

							Renderer re = item.GetComponentInChildren<Renderer>();

							Vector3[] vectors = DrawUtilities.GetBoxVectors(re.bounds);
							Vector2 LabelVector = DrawUtilities.GetW2SVector(cam, re.bounds, vectors, ll);

							string text = string.Format("<size=12><color=#{0}>{1}\n{2}</color></size>", DrawUtilities.ColorToHex(c), item.asset.itemName, Mathf.Round(VectorUtilities.GetDistance(Player.player.transform.position, position)));

							DrawUtilities.PrepareBoxLines(vectors, c);
							DrawUtilities.DrawLabel(ll, LabelVector, text);
							break;
						}
				}
			}

			float steps = Mathf.Ceil(ESPVariables.DrawBuffer.Count / 2);

			for (int i = 0; i < steps; i++)
			{
				int step = i * 2;

				GL.PushMatrix();
				GL.Begin(GL.LINES);
				AssetVariables.GLMaterial.SetPass(0);
				GL.LoadIdentity();

				GL.LoadProjectionMatrix(cam.projectionMatrix);
				GL.modelview = cam.worldToCameraMatrix;

				for (int j = 0; j < 2; j++)
				{
					int curr = step + j;

					if (j >= ESPVariables.DrawBuffer.Count)
						return;

					ESPBox box = ESPVariables.DrawBuffer[curr];
					GL.Color(box.Color);
					for (int k = 0; k < box.Vertices.Length; k++)
						GL.Vertex(box.Vertices[k]);
				}

				GL.End();
				GL.PopMatrix();
			}
			ESPVariables.DrawBuffer.Clear();
		}
	}
}
