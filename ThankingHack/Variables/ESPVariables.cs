using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Misc;
using UnityEngine;

namespace Thanking.Variables
{
	public enum ESPTarget
	{
		Players,
		Items,
		Sentries,
		Beds,
		ClaimFlags,
		Vehicles,
		Storage,
		Generators
	}

	public enum LabelLocation
	{
		TopRight,
		TopMiddle,
		TopLeft,
		MiddleRight,
		Center,
		MiddleLeft,
		BottomRight,
		BottomMiddle,
		BottomLeft
	}

	public class ESPVisual
	{
		public bool Enabled;
		public bool Rectangle;
		public bool InfiniteDistance;
		public bool LineToObject;
		public SerializableColor Color;
		public LabelLocation Location;
		public float Distance;

		public ESPVisual(bool e, bool r, bool id, bool lto, SerializableColor c, LabelLocation ll,  float d)
		{
			Enabled = e;
			Rectangle = r;
			InfiniteDistance = id;
			LineToObject = lto;
			Color = c;
			Location = ll;
			Distance = d;
		}
	}

	public class ESPObject
	{
		public ESPTarget Target;
		public object Object;

		public ESPObject(ESPTarget t, object o)
		{
			Target = t;
			Object = o;
		}
	}

	public class LocalBounds
	{
		public Vector3 PosOffset;
		public Vector3 Extents;

		public LocalBounds(Vector3 po, Vector3 e)
		{
			PosOffset = po;
			Extents = e;
		}
	}

	public class ESPVariables
	{
		public static List<ESPObject> Objects = new List<ESPObject>();

		public static List<ESPBox> DrawBuffer = new List<ESPBox>();
		public static List<ESPBox2> DrawBuffer2 = new List<ESPBox2>();
	}

	public class ESPBox
	{
		public Color Color;
		public Vector3[] Vertices;
	}

	public class ESPBox2
	{
		public Color Color;
		public Vector2[] Vertices;
	}
}
