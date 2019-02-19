using Thinking.Attributes;
using UnityEngine;

namespace Thinking.Options.AimOptions
{
	public static class TriggerbotOptions
	{
		[Save] public static bool Enabled = false;
		public static bool IsFiring = false;
	}
}
