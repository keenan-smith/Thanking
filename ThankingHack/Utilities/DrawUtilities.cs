using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Thanking.Utilities
{
	public static class DrawUtilities
	{
		public static void DrawOutline(Bounds b, Material mat)
		{
			Vector3[] pts = new Vector3[8];
			Camera cam = Camera.main;
			float margin = 0;

			pts[0] = cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y,
				b.center.z + b.extents.z));
			pts[1] = cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y,
				b.center.z - b.extents.z));
			pts[2] = cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y,
				b.center.z + b.extents.z));
			pts[3] = cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y,
				b.center.z - b.extents.z));
			pts[4] = cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y,
				b.center.z + b.extents.z));
			pts[5] = cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y,
				b.center.z - b.extents.z));
			pts[6] = cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y,
				b.center.z + b.extents.z));
			pts[7] = cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y,
				b.center.z - b.extents.z));

			for (int i = 0; i < pts.Length; i++)
				pts[i].y = Screen.height - pts[i].y;

			Vector3 min = pts[0];
			Vector3 max = pts[0];
			for (int i = 1; i < pts.Length; i++)
			{
				min = Vector3.Min(min, pts[i]);
				max = Vector3.Max(max, pts[i]);
			}

			Rect r = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
			r.xMin -= margin;
			r.xMax += margin;
			r.yMin -= margin;
			r.yMax += margin;

			GL.PushMatrix();
			GL.Begin(1);
			mat.SetPass(0);

			GL.Color(Color.cyan);

			GL.Vertex3(r.center.x - (r.size.x / 2), r.center.y + (r.size.y / 2), 0);
			GL.Vertex3(r.center.x + (r.size.x / 2), r.center.y + (r.size.y / 2), 0);
			GL.Vertex3(r.center.x + (r.size.x / 2), r.center.y + (r.size.y / 2), 0);
			GL.Vertex3(r.center.x + (r.size.x / 2), r.center.y - (r.size.y / 2), 0);
			GL.Vertex3(r.center.x + (r.size.x / 2), r.center.y - (r.size.y / 2), 0);
			GL.Vertex3(r.center.x - (r.size.x / 2), r.center.y - (r.size.y / 2), 0);
			GL.Vertex3(r.center.x - (r.size.x / 2), r.center.y - (r.size.y / 2), 0);
			GL.Vertex3(r.center.x - (r.size.x / 2), r.center.y + (r.size.y / 2), 0);

			GL.End();
			GL.PopMatrix();
		}
	}
}
