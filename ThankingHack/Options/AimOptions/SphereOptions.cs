using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
using UnityEngine;

namespace Thanking.Options.AimOptions
{
	public static class SphereOptions
	{
		[Save]
		public static bool Enabled = true;
		
		[Save]
		public static float SphereRadius = 15f;
		[Save]
		public static float VehicleSphereRadius = 12f;
		[Save]
		public static int RecursionLevel = 2;

		[Save]
		public static KeyCode Toggle = KeyCode.RightBracket;
	}
}
