using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Thanking.Options.AimOptions
{
    public static class TriggerbotOptions
	{
		[Save]
		public static bool Enabled = false;
		[Save]
		public static bool IsFiring = false;

		[Save]
		public static KeyCode Toggle = KeyCode.Backslash;
    }
}
