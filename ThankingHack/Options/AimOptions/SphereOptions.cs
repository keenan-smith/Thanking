using Thinking.Attributes;
using UnityEngine;

namespace Thinking.Options.AimOptions
{
	public static class SphereOptions
	{
		[Save] public static float SphereRadius = 15f;
		[Save] public static int RecursionLevel = 2;
		[Save] public static bool SpherePrediction = true;

		//[Save] public static KeyCode Toggle = KeyCode.RightBracket;
	}
}
