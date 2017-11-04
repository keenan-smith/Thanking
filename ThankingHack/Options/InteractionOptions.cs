using Thanking.Attributes;

namespace Thanking.Options
{
	public static class InteractionOptions
	{
		[Save] public static bool HitStructures = false;
		[Save] public static bool HitBarricades = false;
		[Save] public static bool HitItems = true;
		[Save] public static bool HitVehicles = false;
		[Save] public static bool HitResources = true;
	}
}
