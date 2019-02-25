using Thanking.Attributes;

namespace Thanking.Options
{
    public static class InteractionOptions
    {
        [Save] public static bool InteractThroughWalls = true;
        [Save] public static bool NoHitStructures = false;
        [Save] public static bool NoHitBarricades = true;
        [Save] public static bool NoHitItems = false;
        [Save] public static bool NoHitVehicles = true;
        [Save] public static bool NoHitResources = true;
        [Save] public static bool NoHitEnvironment = true;
    }
}