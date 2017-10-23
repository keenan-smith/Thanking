using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
using Thanking.Misc;

namespace Thanking.Options
{
	public static class MiscOptions
	{
		[Save]
		public static float SalvageTime = 1f;

		[Save]
		public static bool SetTimeEnabled = true;
		[Save]
		public static float Time = 0f;

		[Save]
		public static bool SlowFall = false;
		[Save]
		public static bool AirStick = false;
	}
}
