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
		[Save]
		public static bool Enabled = true;
		[Save]
		public static KeyCode Toggle = KeyCode.LeftBracket;

		[Save]
		public static ESPVisual[] VisualOptions =
		{
			new ESPVisual(true, ((Color32)Color.red).ToSerializableColor(), true, LabelLocation.MiddleRight, true, 500f), //Players
			new ESPVisual(true, ((Color32)Color.cyan).ToSerializableColor(), true, LabelLocation.MiddleRight, false, 250f), //Items
			new ESPVisual(false, ((Color32)Color.cyan).ToSerializableColor(), true, LabelLocation.MiddleRight, false, 250f), //Sentries
			new ESPVisual(false, ((Color32)Color.cyan).ToSerializableColor(), true, LabelLocation.MiddleRight, false, 250f), //Beds
			new ESPVisual(false, ((Color32)Color.cyan).ToSerializableColor(), true, LabelLocation.MiddleRight, false, 250f), //Claim Flags
			new ESPVisual(false, ((Color32)Color.cyan).ToSerializableColor(), true, LabelLocation.MiddleRight, false, 250f), //Vehicles
			new ESPVisual(false, ((Color32)Color.cyan).ToSerializableColor(), true, LabelLocation.MiddleRight, false, 250f), //Storage
			new ESPVisual(false, ((Color32)Color.cyan).ToSerializableColor(), true, LabelLocation.MiddleRight, false, 250f) //Generators
		};
	}
}