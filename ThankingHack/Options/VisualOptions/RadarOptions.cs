using Thanking.Attributes;

namespace Thanking.Options.VisualOptions
{
    public static class RadarOptions
    {
        [Save] public static bool Enabled = false;
        [Save] public static int type = 3;
        [Save] public static bool DetialedPlayers = false;
        [Save] public static bool ShowPlayers = false;
        [Save] public static bool ShowVehicles = false;
        [Save] public static bool ShowVehiclesUnlocked = false;
        [Save] public static bool ShowDeathPosition = false;
        [Save] public static float RadarZoom = 1;
        [Save] public static float RadarSize = 300;
    }
}
