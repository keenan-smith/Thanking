using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Thanking.Utilities
{
	public static class DrawUtilities
	{
		public static void PrepareWorldBounds(Bounds b, Color c, Material mat)
		{
			Vector3 v3Center = b.center;
			Vector3 v3Extents = b.extents;

			Vector3[] vectors = new Vector3[8];

			vectors[0] = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
			vectors[1] = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
			vectors[2] = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
			vectors[3] = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
			vectors[4] = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
			vectors[5] = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
			vectors[6] = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
			vectors[7] = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner

			GL.Color(new Color(0, 0, 0, 0));
			GL.Color(c);

			GL.Vertex(vectors[0]); //front top left to front right
			GL.Vertex(vectors[1]);
			GL.Vertex(vectors[1]); //front top right to front bottom right
			GL.Vertex(vectors[3]);
			GL.Vertex(vectors[3]); //front bottom right to front bottom left
			GL.Vertex(vectors[2]);
			GL.Vertex(vectors[2]); //front bottom left to front top left
			GL.Vertex(vectors[0]);

			GL.Vertex(vectors[4]); //back top left to back top right
			GL.Vertex(vectors[5]);
			GL.Vertex(vectors[5]); //back top right to back bottom right
			GL.Vertex(vectors[7]);
			GL.Vertex(vectors[7]); //back bottom right to back bottom left
			GL.Vertex(vectors[6]);
			GL.Vertex(vectors[6]); //back bottom left to back top left
			GL.Vertex(vectors[4]);

			GL.Vertex(vectors[0]);
			GL.Vertex(vectors[4]);
			GL.Vertex(vectors[1]);
			GL.Vertex(vectors[5]);
			GL.Vertex(vectors[2]);
			GL.Vertex(vectors[6]);
			GL.Vertex(vectors[3]);
			GL.Vertex(vectors[7]);

			GL.Color(new Color(0, 0, 0, 0));
		}
	}
}
