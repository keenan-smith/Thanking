using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		public static Dictionary<GameObject, LocalBounds> LBounds = new Dictionary<GameObject, LocalBounds>();
	}
}
