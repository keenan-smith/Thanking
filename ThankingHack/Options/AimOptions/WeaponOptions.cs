using Thanking.Attributes;
using Thanking.Misc;
using Thanking.Misc.Serializables;

namespace Thanking.Options.AimOptions
{
	public static class WeaponOptions
	{
		[Save] public static bool ShowWeaponInfo = true;
		[Save] public static bool CustomCrosshair = false;
		[Save] public static SerializableColor CrosshairColor = new SerializableColor(255, 0, 0);

		[Save] public static bool NoRecoil = false;
		[Save] public static bool NoSpread = false;
		[Save] public static bool NoSway = false;
        [Save] public static bool NoDrop = true;
        [Save] public static bool OofOnDeath = true;
		[Save] public static bool AutoReload = true;
		[Save] public static bool Tracers = true;
        [Save] public static bool EnableBulletDropPrediction = true;
        [Save] public static bool HighlightBulletDropPredictionTarget = true;
        [Save] public static bool FastSemiAuto;
    }
}
