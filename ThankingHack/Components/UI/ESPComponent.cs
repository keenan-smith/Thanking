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
using Thanking.Managers.Main;

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

			if (!Provider.isConnected || Provider.isLoading)
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
							Debug.Log(0);
							Player p = (Player)obj.Object;

							if (p == null)
								continue;

							Vector3 position = p.transform.position;
							float dist = VectorUtilities.GetDistance(position, Player.player.transform.position);

							if (dist > visual.Distance && !visual.InfiniteDistance)
								continue;

							Vector3 cpos = cam.WorldToScreenPoint(position);

							Debug.Log(1);
							if (cpos.z <= 0)
								continue;
							Debug.Log(2);

							string text = string.Format("<size=12>{0}\n{1}\n{2}</size>", p.name, p.equipment.asset != null ? p.equipment.asset.itemName : "Fists", Mathf.Round(dist));

							Bounds b = p.GetComponentInChildren<Renderer>().bounds;

							b.size = b.size / 2;
							b.size = new Vector3(b.size.x, b.size.y * 1.25f, b.size.z);

							Vector3[] vectors = DrawUtilities.GetBoxVectors(b);

							if (visual.Rectangle || Input.GetKey(KeyCode.RightAlt))
								DrawUtilities.PrepareRectangleLines(Camera.main, b, c);
							else
								DrawUtilities.PrepareBoxLines(vectors, c);

							Debug.Log(3);
							Vector3 LabelVector = DrawUtilities.GetW2SVector(cam, b, vectors, ll);
							DrawUtilities.DrawLabel(ll, LabelVector, text, Color.black, c, 4);

							Debug.Log(4);
							break;
						}
					#endregion
					#region Items
					case ESPTarget.Items:
						{
							Debug.Log(0);
							InteractableItem item = (InteractableItem)obj.Object;

							if (item == null)
								continue;

							Vector3 position = item.transform.position;
							Vector3 cpos = cam.WorldToScreenPoint(position);

							Debug.Log(1);
							if (cpos.z <= 0)
								continue;

							Debug.Log(2);
							Bounds b = item.GetComponentInChildren<Renderer>().bounds;
							Vector3[] vectors = DrawUtilities.GetBoxVectors(b);

							string text = string.Format("<size=12>{0}\n{1}</size>", item.asset.itemName, Mathf.Round(VectorUtilities.GetDistance(Player.player.transform.position, position)));

							if (visual.Rectangle || Input.GetKey(KeyCode.RightAlt))
								DrawUtilities.PrepareRectangleLines(Camera.main, b, c);
							else
								DrawUtilities.PrepareBoxLines(vectors, c);

							Debug.Log(3);
							Vector3 LabelVector = DrawUtilities.GetW2SVector(cam, b, vectors, ll);
							DrawUtilities.DrawLabel(ll, LabelVector, text, Color.black, c, 4);

							Debug.Log(4);
							break;
						}
						#endregion
				}
			}

			GL.PushMatrix();
			GL.LoadProjectionMatrix(cam.projectionMatrix);
			GL.modelview = cam.worldToCameraMatrix;
			AssetVariables.GLMaterial.SetPass(0);
			GL.Begin(GL.LINES);

			for (int i = 0; i < ESPVariables.DrawBuffer.Count; i++)
			{
				ESPBox box = ESPVariables.DrawBuffer[i];

				GL.Color(box.Color);

				Vector3[] vertices = box.Vertices;
				for (int j = 0; j < vertices.Length; j++)
					GL.Vertex(vertices[j]);

			}
			GL.End();
			GL.PopMatrix();

			GL.PushMatrix();
			AssetVariables.GLMaterial.SetPass(0);
			GL.Begin(GL.LINES);

			for (int i = 0; i < ESPVariables.DrawBuffer2.Count; i++)
			{
				ESPBox2 box = ESPVariables.DrawBuffer2[i];

				GL.Color(box.Color);

				Vector2[] vertices = box.Vertices;
				for (int j = 0; j < vertices.Length; j++)
					GL.Vertex3(vertices[j].x, vertices[j].y, 0);

			}
			GL.End();
			GL.PopMatrix();


			ESPVariables.DrawBuffer.Clear();
			ESPVariables.DrawBuffer2.Clear();
		}
	}
}
