using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Misc.Enums;

namespace Thanking.Options.AimOptions
{
    public static class AimbotOptions
    {
        [Save] public static bool Enabled = true;
        [Save] public static bool UseGunDistance = false;
        [Save] public static bool Smooth = true;
        [Save] public static bool OnKey = true;
        [Save] public static bool UseReleaseAimKey = false;

        public static float MaxSpeed = 20f;
        [Save] public static float AimSpeed = 5f;
        [Save] public static bool AimThroughWalls = true;
        [Save] public static float Distance = 300f;
        [Save] public static float FOV = 15;
        [Save] public static bool ClosestInFOV = true;

        [Save] public static ELimb TargetLimb = ELimb.SKULL;
        [Save] public static TargetMode TargetMode = TargetMode.Distance;
    }
}
