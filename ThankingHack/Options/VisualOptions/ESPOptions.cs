using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
using Thanking.Misc;
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
		public static bool[] EnabledOptions =
		{
			true,
			true,
			false,
			false,
			false,
			false,
			false,
			false
		};
		[Save]
		public static SerializableColor[] ESPColors =
		{
			((Color32)Color.red).ToSerializableColor(),
			((Color32)Color.cyan).ToSerializableColor(),
			((Color32)Color.cyan).ToSerializableColor(),
			((Color32)Color.cyan).ToSerializableColor(),
			((Color32)Color.cyan).ToSerializableColor(),
			((Color32)Color.cyan).ToSerializableColor(),
			((Color32)Color.cyan).ToSerializableColor(),
			((Color32)Color.cyan).ToSerializableColor(),
		};
	}
}
