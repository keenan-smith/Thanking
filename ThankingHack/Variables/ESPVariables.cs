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
		public SerializableColor Color;
		public bool Rectangle;
		public LabelLocation Location;
		public bool InfiniteDistance;
		public float Distance;

		public ESPVisual(bool e, SerializableColor c, bool r, LabelLocation ll, bool id, float d)
		{
			Enabled = e;
			Color = c;
			Rectangle = r;
			Location = ll;
			InfiniteDistance = id;
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
	}

	public class ESPBox
	{
		public Color Color;
		public Vector3[] Vertices;
	}
}
