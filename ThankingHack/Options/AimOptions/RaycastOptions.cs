using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Misc;

namespace Thanking.Options.AimOptions
{
	public enum TargetPriority
	{
		Player,
		Zombie,
		Sentry,
		Bed,
		ClaimFlag,
		Storage
	}

	public static class RaycastOptions
	{
		[Save] public static bool Enabled = true;
		[Save] public static bool UseRandomLimb = false;
		[Save] public static bool UseTargetMaterial = true;
		[Save] public static bool UseModifiedVector = true;
		[Save] public static bool ExtendedRange = true;

		[Save] public static TargetPriority Target = TargetPriority.Player;
		[Save] public static EPhysicsMaterial TargetMaterial = EPhysicsMaterial.ALIEN_DYNAMIC;
		[Save] public static ELimb TargetLimb = ELimb.SKULL;
		[Save] public static SerializableVector TargetRagdoll = new SerializableVector(0, 10, 0);
	}
}
