using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
using Thanking.Misc;
using UnityEngine;

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

		[Save]
		public static bool LogoEnabled = true;
		[Save]
		public static KeyCode LogoToggle = KeyCode.Slash;

		[Save]
		public static KeyCode ReloadConfig = KeyCode.Period;
		[Save]
		public static KeyCode SaveConfig = KeyCode.Comma;
	}
}
