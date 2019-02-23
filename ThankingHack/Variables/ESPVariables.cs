using System.Collections.Generic;
using SDG.Unturned;
using Thinking.Misc;
using UnityEngine;

namespace Thinking.Variables
{
	public enum ESPTarget
	{
		Players,
        Zombies,
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
        public bool Boxes;
        public bool Labels;
		public bool ShowName;
		public bool ShowDistance;
		public bool ShowAngle;
        public bool TwoDimensional;
        public bool Glow;
        public bool InfiniteDistance;
		public bool LineToObject;
		public bool TextScaling;
		public bool UseObjectCap;
        public bool InlineText;
	
		public LabelLocation Location;
		public float Distance;
		public float MinTextSizeDistance;

		public int BorderStrength;
		public int FixedTextSize;
		public int MinTextSize;
		public int MaxTextSize;
		public int ObjectCap;
	}

	public class ESPObject
	{
		public ESPTarget Target;
		public object Object;
		public GameObject GObject;

		public ESPObject(ESPTarget t, object o, GameObject go)
		{
			Target = t;
			Object = o;
			GObject = go;
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

		public static Queue<ESPBox> DrawBuffer = new Queue<ESPBox>();
		public static Queue<ESPBox2> DrawBuffer2 = new Queue<ESPBox2>();
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
