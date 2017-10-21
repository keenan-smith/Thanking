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

							string text = string.Format("<size=12><color=#{0}>{1}\n{2}\n{3}</color></size>", DrawUtilities.ColorToHex(c), p.name, p.equipment.asset != null ? p.equipment.asset.itemName : "Fists", Mathf.Round(dist));

							Bounds b = new Bounds();
							Vector3 LabelVector = Vector3.zero;
							Vector3[] vectors;

							Renderer re = p.GetComponentInChildren<Renderer>();

							if (!visual.Rectangle)
							{
								b = re.bounds;
								b.size = b.size / 2;
								b.size = new Vector3(b.size.x, b.size.y * 1.25f, b.size.z);
								vectors = DrawUtilities.GetBoxVectors(b);

								DrawUtilities.PrepareBoxLines(vectors, c);
							}
							else
							{
								Transform t = Player.player.look.aim;
								b = re.bounds;
								b.size = b.size / 2;
								b.size = new Vector3(b.size.x, b.size.y * 1.25f, b.size.z);

								vectors = DrawUtilities.GetRectVectors(t, b);
								DrawUtilities.PrepareRectangleLines(vectors, c);
							}

							LabelVector = DrawUtilities.GetW2SVector(cam, b, vectors, ll);
							DrawUtilities.DrawLabel(ll, LabelVector, text);
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

							Renderer re = item.GetComponentInChildren<Renderer>();

							Vector3[] vectors = DrawUtilities.GetBoxVectors(re.bounds);
							Vector2 LabelVector = DrawUtilities.GetW2SVector(cam, re.bounds, vectors, ll);

							string text = string.Format("<size=12><color=#{0}>{1}\n{2}</color></size>", DrawUtilities.ColorToHex(c), item.asset.itemName, Mathf.Round(VectorUtilities.GetDistance(Player.player.transform.position, position)));

							DrawUtilities.PrepareBoxLines(vectors, c);
							DrawUtilities.DrawLabel(ll, LabelVector, text);
							break;
						}
						#endregion
					#region Sentries  
						#endregion
					#region Beds
						#endregion
					#region Claim Flags
						#endregion
					#region Vehicles
						#endregion
					#region Storage
						#endregion
					#region Generators
						#endregion
				}
			}

			#region Drawing
			int var = 4;
			float steps = Mathf.Ceil(ESPVariables.DrawBuffer.Count / var);

			for (int i = 0; i < steps; i++)
			{
				int step = i * var;

				GL.PushMatrix();
				GL.Begin(GL.LINES);
				AssetVariables.GLMaterial.SetPass(0);
				GL.LoadIdentity();

				GL.LoadProjectionMatrix(cam.projectionMatrix);
				GL.modelview = cam.worldToCameraMatrix;

				for (int j = 0; j < var; j++)
				{
					int curr = step + j;

					if (j >= ESPVariables.DrawBuffer.Count)
						return;

					ESPBox box = ESPVariables.DrawBuffer[curr];
					GL.Color(box.Color);

					for (int k = 0; k < box.Vertices.Length; k++)
						GL.Vertex(box.Vertices[k]);

					for (int k = 0; k < box.Vertices.Length; k++)
						GL.Vertex(box.Vertices[k]);
				}

				GL.End();
				GL.PopMatrix();
			}

			ESPVariables.DrawBuffer.Clear();
			#endregion
		}
	}
}
