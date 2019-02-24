using Thanking.Attributes;

namespace Thanking.Options.VisualOptions
{
    public static class RadarOptions
    {
        [Save] public static bool Enabled = false;
        [Save] public static bool TrackPlayer = false;
        [Save] public static bool ShowPlayers = false;
        [Save] public static bool ShowVehicles = false;
        [Save] public static bool ShowVehiclesUnlocked = false;
        [Save] public static bool ShowDeathPosition = false;
        public static float RadarZoom = 1;
        [Save] public static float RadarSize = 300;
    }
}
