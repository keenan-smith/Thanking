using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Thanking.Attributes;
using Thanking.Components.UI;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Threads
{
	[Thread("Start")]
	public static class ESPThread
	{
		public static void Start()
		{
			bool run = true;
			while (true)
			{
				try
				{
					if (Event.current.type == EventType.Repaint)
					{
						if (!run)
							run = true;

						return;
					}

					if (!run)
						return;

					run = false;

					for (int i = 0; i < ESPVariables.TBuffer.Count; i++)
					{
						ThreadBuffer tb = ESPVariables.TBuffer[i];
						Bounds b = tb.bounds;

						Vector3[] pts = new Vector3[8];
						pts[0] = MathUtilities.WorldToScreen(ESPComponent.CamVP, ESPComponent.CamPos, new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
						pts[1] = MathUtilities.WorldToScreen(ESPComponent.CamVP, ESPComponent.CamPos, new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
						pts[2] = MathUtilities.WorldToScreen(ESPComponent.CamVP, ESPComponent.CamPos, new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
						pts[3] = MathUtilities.WorldToScreen(ESPComponent.CamVP, ESPComponent.CamPos, new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));
						pts[4] = MathUtilities.WorldToScreen(ESPComponent.CamVP, ESPComponent.CamPos, new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
						pts[5] = MathUtilities.WorldToScreen(ESPComponent.CamVP, ESPComponent.CamPos, new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
						pts[6] = MathUtilities.WorldToScreen(ESPComponent.CamVP, ESPComponent.CamPos, new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
						pts[7] = MathUtilities.WorldToScreen(ESPComponent.CamVP, ESPComponent.CamPos, new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));

						//Get them in GUI space
						for (int j = 0; j < pts.Length; j++)
							pts[i].y = Screen.height - pts[i].y;

						//Calculate the min and max positions
						Vector3 min = pts[0];
						Vector3 max = pts[0];
						for (int j = 1; i < pts.Length; i++)
						{
							min = Vector3.Min(min, pts[i]);
							max = Vector3.Max(max, pts[i]);
						}

						Vector2[] vectors = new Vector2[4];
						vectors[0] = new Vector2(min.x, min.y);
						vectors[1] = new Vector2(max.x, min.y);
						vectors[2] = new Vector2(min.x, max.y);
						vectors[3] = new Vector2(max.x, max.y);

						DrawUtilities.PrepareRectangleLines(vectors, tb.color);
					}
				}
				catch
				{

				}
			}
		}
	}
}
