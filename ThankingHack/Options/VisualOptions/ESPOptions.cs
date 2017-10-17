using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
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
		public static float Distance = Mathf.Infinity;

		/* until you can find a better way of doing this we're sticking with a bool array
		 * 0: players
		 * 1: items
		 * 2: sentries
		 * 3: beds
		 * 4: claim flags
		 * 5: vehicles
		 * 6: storage
		 * 7: generator
		 */

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
	}
}
