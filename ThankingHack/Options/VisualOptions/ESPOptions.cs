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
			new ESPVisual(true, true, true, true, ((Color32)Color.red).ToSerializableColor(), LabelLocation.MiddleRight, 500, 12, 4), //Players
			new ESPVisual(true, false, true, true, ((Color32)Color.cyan).ToSerializableColor(), LabelLocation.MiddleRight, 250, 12, 4), //Items
			new ESPVisual(false, false, true, true, ((Color32)Color.cyan).ToSerializableColor(), LabelLocation.MiddleRight, 250, 12, 4), //Sentries
			new ESPVisual(false, false, true, true, ((Color32)Color.cyan).ToSerializableColor(), LabelLocation.MiddleRight, 250, 12, 4), //Beds
			new ESPVisual(false, false, true, true, ((Color32)Color.cyan).ToSerializableColor(), LabelLocation.MiddleRight, 250, 12, 4), //Claim Flags
			new ESPVisual(false, false, true, true, ((Color32)Color.cyan).ToSerializableColor(), LabelLocation.MiddleRight, 250, 12, 4), //Vehicles
			new ESPVisual(false, false, true, true, ((Color32)Color.cyan).ToSerializableColor(), LabelLocation.MiddleRight, 250, 12, 4), //Storage
			new ESPVisual(false, false, true, true, ((Color32)Color.cyan).ToSerializableColor(), LabelLocation.MiddleRight, 250, 12, 4) //Generators
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
		[Save]
		public static bool ShowPlayerName = true;
		[Save]
		public static bool ShowPlayerDistance = true;
		[Save]
		public static bool ShowPlayerWeapon = true;
		#endregion
	}
}