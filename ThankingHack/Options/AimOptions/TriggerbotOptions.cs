using Thanking.Attributes;
using UnityEngine;

namespace Thanking.Options.AimOptions
{
    public static class TriggerbotOptions
	{
		[Save]
		public static bool Enabled = false;
		public static bool IsFiring = false;

		[Save]
		public static KeyCode Toggle = KeyCode.Backslash;
    }
}
