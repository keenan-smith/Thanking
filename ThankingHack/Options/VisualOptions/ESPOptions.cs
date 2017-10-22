using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
using Thanking.Misc;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Options.VisualOptions
{
	public static class ESPOptions
	{
		#region Global Options
		[Save]
		public static bool Enabled = true;
		[Save]
		public static KeyCode Toggle = KeyCode.LeftBracket;

		[Save]
		public static ESPVisual[] VisualOptions =
		{
			new ESPVisual(true, ((Color32)Color.red).ToSerializableColor(), false, LabelLocation.MiddleRight, true, 500f), //Players
			new ESPVisual(true, ((Color32)Color.cyan).ToSerializableColor(), false, LabelLocation.MiddleRight, false, 250f), //Items
			new ESPVisual(true, ((Color32)Color.cyan).ToSerializableColor(), false, LabelLocation.MiddleRight, false, 250f), //Sentries
			new ESPVisual(true, ((Color32)Color.cyan).ToSerializableColor(), false, LabelLocation.MiddleRight, false, 250f), //Beds
			new ESPVisual(true, ((Color32)Color.cyan).ToSerializableColor(), false, LabelLocation.MiddleRight, false, 250f), //Claim Flags
			new ESPVisual(true, ((Color32)Color.cyan).ToSerializableColor(), false, LabelLocation.MiddleRight, false, 250f), //Vehicles
			new ESPVisual(true, ((Color32)Color.cyan).ToSerializableColor(), false, LabelLocation.MiddleRight, false, 250f), //Storage
			new ESPVisual(true, ((Color32)Color.cyan).ToSerializableColor(), false, LabelLocation.MiddleRight, false, 250f) //Generators
		};

		[Save]
		public static Dictionary<ESPTarget, int> PriorityTable = new Dictionary<ESPTarget, int>()
		{
			{ ESPTarget.Players, 0 },
			{ ESPTarget.Items, 1 },
			{ ESPTarget.Sentries, 2 },
			{ ESPTarget.Beds, 3 },
			{ ESPTarget.ClaimFlags, 4 },
			{ ESPTarget.Vehicles, 5 },
			{ ESPTarget.Storage, 6 },
			{ ESPTarget.Generators, 7 }
		};
		#endregion

		#region Player Options
		public static bool ShowPlayerName = true;
		public static bool ShowPlayerDistance = true;
		public static bool ShowPlayerWeapon = true;
#endregion
	}
}