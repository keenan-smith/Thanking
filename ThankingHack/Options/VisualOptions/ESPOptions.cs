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
		public static bool InfiniteDistance = true;
		[Save]
		public static float Distance = 500f;

		[Save]
		public static ESPVisual[] VisualOptions =
		{
			new ESPVisual(true, ((Color32)Color.red).ToSerializableColor(), true, LabelLocation.MiddleRight),
			new ESPVisual(true, ((Color32)Color.cyan).ToSerializableColor(), true, LabelLocation.MiddleRight),
			new ESPVisual(false, ((Color32)Color.cyan).ToSerializableColor(), true, LabelLocation.MiddleRight),
			new ESPVisual(false, ((Color32)Color.cyan).ToSerializableColor(), true, LabelLocation.MiddleRight),
			new ESPVisual(false, ((Color32)Color.cyan).ToSerializableColor(), true, LabelLocation.MiddleRight),
			new ESPVisual(false, ((Color32)Color.cyan).ToSerializableColor(), true, LabelLocation.MiddleRight),
			new ESPVisual(false, ((Color32)Color.cyan).ToSerializableColor(), true, LabelLocation.MiddleRight),
			new ESPVisual(false, ((Color32)Color.cyan).ToSerializableColor(), true, LabelLocation.MiddleRight)
		};
	}
}
