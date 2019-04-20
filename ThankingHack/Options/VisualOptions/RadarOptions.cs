using Thanking.Attributes;
using Thanking.Misc.Serializables;
using UnityEngine;

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
        [Save] public static SerializableRect vew = new Rect(Screen.width - RadarOptions.RadarSize - 20, 10, RadarOptions.RadarSize + 10, RadarOptions.RadarSize + 10); //Viewport of the mirror camera

    }
}
