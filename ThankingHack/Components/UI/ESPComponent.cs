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
			mat = new Material(Shader.Find("Hidden/Internal-Colored")) { hideFlags = HideFlags.HideAndDontSave };
			mat.SetInt("_SrcBlend", 5);
			mat.SetInt("_DstBlend", 10);
			mat.SetInt("_Cull", 0);
			mat.SetInt("_ZWrite", 0);

			CoroutineComponent.ESPCoroutine = StartCoroutine(ESPCoroutines.UpdateObjectList());
		}

		public void OnGUI()
		{
			if (Event.current.type != EventType.Repaint || !ESPOptions.Enabled)
				return;

			int lines = (int)Mathf.Ceil(ESPVariables.Objects.Count / 16);

			for (int i = 0; i < lines; i++)
			{
				int k = i * 16;
				GL.PushMatrix();
				GL.Begin(GL.LINES);
				mat.SetPass(0);
				GL.LoadIdentity();

				Camera cam = Camera.main;
				GL.LoadProjectionMatrix(cam.projectionMatrix);
				GL.modelview = cam.worldToCameraMatrix;
				GL.Color(new Color(0, 0, 0, 0));

				for (int j = 0; j < 16; j++)
				{
					int l = k + j;

					if (ESPVariables.Objects.Count <= l)
						return;

					ESPObject obj = ESPVariables.Objects[l];
					int targ = (int)obj.Target;

					if (ESPOptions.EnabledOptions[targ])
					{
						switch (obj.Target)
						{
							case ESPTarget.Players:
								{
									GameObject go = ((Player)obj.Object).gameObject;
									Renderer re = go.GetComponentInChildren<Renderer>();
									DrawUtilities.PrepareWorldBounds(re.bounds, ESPOptions.ESPColors[targ].ToColor(), mat);
									break;
								}
							case ESPTarget.Items:
								{
									GameObject go = ((InteractableItem)obj.Object).gameObject;
									Renderer re = go.GetComponentInChildren<Renderer>();
									DrawUtilities.PrepareWorldBounds(re.bounds, ESPOptions.ESPColors[targ].ToColor(), mat);
									break;
								}
						}
					}
				}

				GL.End();
				GL.PopMatrix();
			}

		}
	}
}
