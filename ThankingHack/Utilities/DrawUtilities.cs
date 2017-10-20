using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Utilities
{
	public static class DrawUtilities
	{
		public static Vector2 InvertScreenSpace(Vector2 dim) =>
			new Vector2(dim.x, Screen.height - dim.y);

		public static string ColorToHex(Color32 color)
		{
			string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
			return hex;
		}

		public static void DrawLabel(LabelLocation location, Vector2 W2SVector, string content)
		{
			GUIContent gcontent = new GUIContent(content);
			GUIStyle LabelStyle = new GUIStyle();

			LabelStyle.font = AssetVariables.Roboto;
			LabelStyle.fontSize = 12;
			
			Vector2 dim = LabelStyle.CalcSize(gcontent);
			float width = dim.x;
			float height = dim.y;
			Rect rect = new Rect(0, 0, width, height);

			switch (location)
			{
				case LabelLocation.BottomLeft:
					rect.x = W2SVector.x - width;
					rect.y = W2SVector.y - height;
					LabelStyle.alignment = TextAnchor.LowerRight;
					break;
				case LabelLocation.BottomMiddle:
					rect.x = W2SVector.x - width / 2;
					rect.y = W2SVector.y;
					LabelStyle.alignment = TextAnchor.UpperCenter;
					break;
				case LabelLocation.BottomRight:
					rect.x = W2SVector.x;
					rect.y = W2SVector.y - height;
					LabelStyle.alignment = TextAnchor.LowerLeft;
					break;
				case LabelLocation.Center:
					rect.x = W2SVector.x - width / 2;
					rect.y = W2SVector.y - height / 2;
					LabelStyle.alignment = TextAnchor.MiddleCenter;
					break;
				case LabelLocation.MiddleLeft:
					rect.x = W2SVector.x - width;
					rect.y = W2SVector.y - height / 2;
					LabelStyle.alignment = TextAnchor.MiddleRight;
					break;
				case LabelLocation.MiddleRight:
					rect.x = W2SVector.x;
					rect.y = W2SVector.y - height / 2;
					LabelStyle.alignment = TextAnchor.MiddleLeft;
					break;
				case LabelLocation.TopLeft:
					rect.x = W2SVector.x - width;
					rect.y = W2SVector.y;
					LabelStyle.alignment = TextAnchor.UpperRight;
					break;
				case LabelLocation.TopMiddle:
					rect.x = W2SVector.x - width / 2;
					rect.y = W2SVector.y - height;
					LabelStyle.alignment = TextAnchor.LowerCenter;
					break;
				case LabelLocation.TopRight:
					rect.x = W2SVector.x;
					rect.y = W2SVector.y;
					LabelStyle.alignment = TextAnchor.UpperLeft;
					break;
			}

			GUI.Label(rect, gcontent, LabelStyle);
		}

		public static Vector2 GetW2SVector(Camera cam, Bounds b, Vector3[] vectors, LabelLocation location)
		{
			Vector2 vec = Vector2.zero;
			switch (location)
			{
				case LabelLocation.BottomLeft:
					vec = cam.WorldToScreenPoint(vectors[2]);
					break;
				case LabelLocation.BottomMiddle:
					vec = cam.WorldToScreenPoint(new Vector3(b.center.x, vectors[3].y, vectors[3].z));
					break;
				case LabelLocation.BottomRight:
					vec = cam.WorldToScreenPoint(vectors[3]);
					break;
				case LabelLocation.Center:
					vec = cam.WorldToScreenPoint(b.center);
					break;
				case LabelLocation.MiddleLeft:
					vec = cam.WorldToScreenPoint(new Vector3(vectors[0].x, b.center.y, vectors[0].z));
					break;
				case LabelLocation.MiddleRight:
					vec = cam.WorldToScreenPoint(new Vector3(vectors[1].x, b.center.y, vectors[1].z));
					break;
				case LabelLocation.TopLeft:
					vec = cam.WorldToScreenPoint(vectors[0]);
					break;
				case LabelLocation.TopMiddle:
					vec = cam.WorldToScreenPoint(new Vector3(b.center.x, vectors[1].y, vectors[1].z));
					break;
				case LabelLocation.TopRight:
					vec = cam.WorldToScreenPoint(vectors[1]);
					break;
			}

			return InvertScreenSpace(vec);
		}

		public static Vector3[] GetRectangleVectors(Transform t, Bounds b)
		{
			Vector3 v3Center = b.center;
			Vector3 v3Extents = b.extents;

			Vector3[] vectors = new Vector3[4];

			vectors[0] = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
			vectors[1] = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
			vectors[2] = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
			vectors[3] = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner

			return vectors;
		}

		public static Vector3[] GetBoxVectors(Bounds b)
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

			return vectors;
		}

		public static void PrepareRectangleLines(Vector3[] nvectors, Color c)
		{
			ESPVariables.DrawBuffer.Add(new ESPBox()
			{
				Color = c,
				Vertices = new Vector3[8]
				{
					nvectors[0],
					nvectors[1],
					nvectors[1],
					nvectors[3],
					nvectors[3],
					nvectors[2],
					nvectors[2],
					nvectors[0]
				}
			});
		}

		public static void PrepareBoxLines(Vector3[] vectors, Color c)
		{
			ESPVariables.DrawBuffer.Add(new ESPBox()
			{
				Color = c,
				Vertices = new Vector3[24]
				{
					vectors[0], //front top left to front right
					vectors[1],
					vectors[1], //front top right to front bottom right
					vectors[3],
					vectors[3], //front bottom right to front bottom left
					vectors[2],
					vectors[2], //front bottom left to front top left
					vectors[0],
					vectors[4], //back top left to back top right
					vectors[5],
					vectors[5],//back top right to back bottom right
					vectors[7],
					vectors[7], //back bottom right to back bottom left
					vectors[6],
					vectors[6], //front top left to back top left
					vectors[4],
					vectors[0], //front top right to back top right
					vectors[4],
					vectors[1], //front bottom left to back bottom left
					vectors[5],
					vectors[2], //front bottom left to back bottom left
					vectors[6],
					vectors[3], //front bottom right to back bottom right
					vectors[7]
				}
			});
		}
	}
}
