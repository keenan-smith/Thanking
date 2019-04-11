using SDG.Unturned;
using Thanking.Components.UI.Menu;
using Thanking.Misc.Classes.ESP;
using Thanking.Misc.Enums;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Utilities
{
	public static class DrawUtilities
	{
		public static bool ShouldRun() =>
			!(!Provider.isConnected || Provider.isLoading || LoadingUI.isBlocked ||
			  OptimizationVariables.MainPlayer == null);

		public static int GetTextSize(ESPVisual vis, double dist)
		{
			if (!vis.TextScaling)
				return vis.FixedTextSize;

			if (dist > vis.MinTextSizeDistance)
				return vis.MinTextSize;

			int variable = vis.MaxTextSize - vis.MinTextSize; //17 - 8 = 9
			double ratio = vis.MinTextSizeDistance / variable; //900 / 9 = 100

			return vis.MaxTextSize - (int)(dist / ratio); // 400 / 100 = 4 -> 17 - 4 = 13
		}

		public static Vector2[] GetRectangleLines(Camera cam, Bounds b, Color c)
		{
			Vector3[] pts = new Vector3[8]
			{
				cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z)),
				cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z)),
				cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z)),
				cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z)),
				cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z)),
				cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z)),
				cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z)),
				cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z))
			};
			
			//Get them in GUI space
			for (int i = 0; i < pts.Length; i++)
				pts[i].y = Screen.height - pts[i].y;

			//Calculate the min and max positions
			Vector3 min = pts[0];
			Vector3 max = pts[0];
			for (int i = 1; i < pts.Length; i++)
			{
				min = Vector3.Min(min, pts[i]);
				max = Vector3.Max(max, pts[i]);
			}

			return new Vector2[4]
			{
				new Vector2(min.x, min.y), //Top left
				new Vector2(max.x, min.y), //Top right
				new Vector2(min.x, max.y), //Bottom left
				new Vector2(max.x, max.y) //Bottom right
			};;
		}

		public static Bounds GetBoundsRecursively(GameObject go)
		{
			Bounds b = new Bounds();
			
			Collider[] cols = go.GetComponentsInChildren<Collider>();
			
			for (int i = 0; i < cols.Length; i++)
				b.Encapsulate(cols[i].bounds);
			
			return b;
		}

		public static Bounds TransformBounds(Transform _transform, Bounds _localBounds)
		{
			Vector3 center = _transform.TransformPoint(_localBounds.center);

			// transform the local extents' axes
			Vector3 extents = _localBounds.extents;
			Vector3 axisX = _transform.TransformVector(extents.x, 0, 0);
			Vector3 axisY = _transform.TransformVector(0, extents.y, 0);
			Vector3 axisZ = _transform.TransformVector(0, 0, extents.z);

			// sum their absolute value to get the world extents
			extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
			extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
			extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

			return new Bounds { center = center, extents = extents };
		}

		public static void DrawTextWithOutline(Rect centerRect, string text, GUIStyle style, Color innerColor, Color borderColor, int borderWidth, string outlineText = null)
		{
			if (outlineText == null)
				outlineText = text;

			// assign the border color
			style.normal.textColor = borderColor;

			// draw an outline color copy to the left and up from original
			Rect modRect = centerRect;
			modRect.x -= borderWidth;
			modRect.y -= borderWidth;
			GUI.Label(modRect, text, style);


			// stamp copies from the top left corner to the top right corner
			while (modRect.x <= centerRect.x + borderWidth)
			{
				modRect.x++;
				GUI.Label(modRect, outlineText, style);
			}

			// stamp copies from the top right corner to the bottom right corner
			while (modRect.y <= centerRect.y + borderWidth)
			{
				modRect.y++;
				GUI.Label(modRect, outlineText, style);
			}

			// stamp copies from the bottom right corner to the bottom left corner
			while (modRect.x >= centerRect.x - borderWidth)
			{
				modRect.x--;
				GUI.Label(modRect, outlineText, style);
			}

			// stamp copies from the bottom left corner to the top left corner
			while (modRect.y >= centerRect.y - borderWidth)
			{
				modRect.y--;
				GUI.Label(modRect, outlineText, style);
			}

			// draw the inner color version in the center
			style.normal.textColor = innerColor;
			GUI.Label(centerRect, text, style);
		}

		public static Vector2 InvertScreenSpace(Vector2 dim) =>
			new Vector2(dim.x, Screen.height - dim.y);

		public static string ColorToHex(Color32 color) =>
			color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");

		public static void DrawLabel(Font Font, LabelLocation Location, Vector2 W2SVector, string Content, Color BorderColor, Color InnerColor, int BorderWidth, string outerContent = null, int fontSize = 12)
		{
			GUIContent gcontent = new GUIContent(Content);
			GUIStyle LabelStyle = new GUIStyle
			{
				font = Font,
				fontSize = fontSize
			};
			
			Vector2 dim = LabelStyle.CalcSize(gcontent);
			float width = dim.x;
			float height = dim.y;
			Rect rect = new Rect(0, 0, width, height);

			switch (Location)
			{
				case LabelLocation.BottomLeft:
					rect.x = W2SVector.x - width;
					rect.y = W2SVector.y;
					LabelStyle.alignment = TextAnchor.LowerRight;
					break;
				case LabelLocation.BottomMiddle:
					rect.x = W2SVector.x - width / 2;
					rect.y = W2SVector.y;
					LabelStyle.alignment = TextAnchor.UpperCenter;
					break;
				case LabelLocation.BottomRight:
					rect.x = W2SVector.x;
					rect.y = W2SVector.y;
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
					rect.y = W2SVector.y - height;
					LabelStyle.alignment = TextAnchor.UpperRight;
					break;
				case LabelLocation.TopMiddle:
					rect.x = W2SVector.x - width / 2;
					rect.y = W2SVector.y - height;
					LabelStyle.alignment = TextAnchor.LowerCenter;
					break;
				case LabelLocation.TopRight:
					rect.x = W2SVector.x;
					rect.y = W2SVector.y - height;
					LabelStyle.alignment = TextAnchor.UpperLeft;
					break;
			}

			if (rect.x - 10 < 0 || rect.y - 10 < 0)
				return;

			if (rect.x + 10 > Screen.width || rect.y + 10 > Screen.height)
				return;
			
			DrawTextWithOutline(rect, gcontent.text, LabelStyle, BorderColor, InnerColor, BorderWidth, outerContent);
		}

		public static Vector2 Get3DW2SVector(Camera cam, Bounds b, LabelLocation location)
		{
			switch (location)
			{
				case LabelLocation.BottomLeft:
				case LabelLocation.BottomMiddle:
				case LabelLocation.BottomRight:
					return InvertScreenSpace(cam.WorldToScreenPoint(new Vector3(b.center.x, b.center.y - b.extents.y, b.center.z)));
				case LabelLocation.Center:
				case LabelLocation.MiddleLeft:
				case LabelLocation.MiddleRight:
					return InvertScreenSpace(cam.WorldToScreenPoint(b.center));
				case LabelLocation.TopLeft:
				case LabelLocation.TopMiddle:
				case LabelLocation.TopRight:
					return InvertScreenSpace(cam.WorldToScreenPoint(new Vector3(b.center.x, b.center.y + b.extents.y, b.center.z)));
				default:
					return Vector2.zero;
			}
		}	

		public static Vector2 Get2DW2SVector(Camera cam, Vector2[] Corners, LabelLocation location)
		{
			switch (location)
			{
				case LabelLocation.BottomLeft:
					return Corners[3];
				case LabelLocation.BottomMiddle:
					return new Vector2((Corners[2].x + Corners[3].x) / 2, Corners[2].y);
				case LabelLocation.BottomRight:
					return Corners[2];
				case LabelLocation.Center:
					return new Vector2(Corners[2].x, (Corners[1].y + Corners[2].y) / 2);
				case LabelLocation.MiddleLeft:
					return new Vector2((Corners[2].x + Corners[3].x) / 2, (Corners[1].y + Corners[2].y) / 2);
				case LabelLocation.MiddleRight:
					return new Vector2(Corners[0].x, (Corners[1].y + Corners[2].y) / 2);
				case LabelLocation.TopLeft:
					return Corners[0];
				case LabelLocation.TopMiddle:
					return new Vector2((Corners[0].x + Corners[1].x) / 2, Corners[0].y);
				case LabelLocation.TopRight:
					return Corners[1];
				default:
					return Vector2.zero;
			}
		}	
		
		public static Vector3[] GetBoxVectors(Bounds b)
		{
			Vector3 v3Center = b.center;
			Vector3 v3Extents = b.extents;

			Vector3[] vectors = new Vector3[8]
			{
				new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z),  // Front top left corner; 2 to 0
				new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z),  // Front top right corner; 3 to 1
				new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z),  // Front bottom left corner
				new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z),  // Front bottom right corner
				new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z),  // Back top left corner; 6 to 4
				new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z),  // Back top right corner; 7 to 5
				new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z),  // Back bottom left corner
				new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z)  // Back bottom right corner
			};

			return vectors;
		}

		public static void PrepareRectangleLines(Vector2[] nvectors, Color c)
		{
			ESPVariables.DrawBuffer2.Enqueue(new ESPBox2
			{
				Color = c,
				Vertices = new []
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
			ESPVariables.DrawBuffer.Enqueue(new ESPBox
			{
				Color = c,
				Vertices = new []
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

		public static void DrawCircle(Material Mat, Color Col, Vector2 Center, float Radius)
		{
			GL.PushMatrix();
			Mat.SetPass(0);
			GL.Begin(GL.LINES);
			GL.Color(Col);
			//float degRad = Mathf.PI / 180;
			for(float theta = 0.0f; theta < (2*Mathf.PI); theta += 0.01f)
			{
				Vector2 ci = (new Vector3(Mathf.Cos(theta) * Radius + Center.x, Mathf.Sin(theta) * Radius + Center.y));
				GL.Vertex(ci);
			}
			GL.End();
			GL.PopMatrix();
		}

        public static void DrawMenuRect(float x, float y, float width, float height, Color fillcolor)
        {
            Color bordercolor = Color.black;

            Drawing.DrawRect(new Rect(x, y, width, 5), bordercolor); // top line
            Drawing.DrawRect(new Rect(x, y, 5, height), bordercolor);
            Drawing.DrawRect(new Rect(x, y + (height - 5), width, 5), bordercolor);
            Drawing.DrawRect(new Rect(x + (width - 5), 0, 5, height), bordercolor);
            Drawing.DrawRect(new Rect(5, 5, width - 10, height - 10), fillcolor); //bg
        }
	}
}
