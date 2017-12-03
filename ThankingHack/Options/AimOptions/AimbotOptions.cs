using SDG.Unturned;
using Thanking.Attributes;
using UnityEngine;

namespace Thanking.Options.AimOptions
{
    public enum TargetMode
    {
        Distance,
        FOV
    }

    public static class AimbotOptions
    {
        [Save] public static bool Enabled = true;
        [Save] public static bool UseGunDistance = false;
        [Save] public static bool FOV_Mode = false;
        [Save] public static bool Smooth = true;
        [Save] public static bool OnKey = false;
        //[Save] public static KeyCode Key = KeyCode.F;

        public static float MaxSpeed = 20f;
        [Save] public static float AimSpeed = 5f;
        [Save] public static float Distance = 300f;
        [Save] public static float FOV = 15;

        [Save] public static ELimb TargetLimb = ELimb.SKULL;
        [Save] public static TargetMode TargetMode = TargetMode.Distance;
    }
}
