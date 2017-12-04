using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Thanking.Attributes;
using Thanking.Coroutines;
using Thanking.Options;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Overrides
{
	public static class OV_LevelLighting
	{
		public static FieldInfo Time;

		[Initializer]
		public static void Init() =>
			Time = typeof(LevelLighting).GetField("_time", BindingFlags.NonPublic | BindingFlags.Static);

		[Override(typeof(LevelLighting), "updateLighting", BindingFlags.Public | BindingFlags.Static)]
		public static void OV_updateLighting()
		{
			float TBackup = LevelLighting.time;
			
			if (!DrawUtilities.ShouldRun() || PlayerCoroutines.IsSpying || !MiscOptions.SetTimeEnabled)
			{
				OverrideUtilities.CallOriginal();
				return;
			}

			Time.SetValue(null, MiscOptions.Time);
			OverrideUtilities.CallOriginal();
			Time.SetValue(null, TBackup);
		}
	}
}