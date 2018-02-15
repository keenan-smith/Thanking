using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Misc;

namespace Thanking.Options.AimOptions
{
	public enum TargetPriority
	{
		Players,
		Zombies,
		Sentries,
		Beds,
		ClaimFlags,
		Storage,
		Vehicles
	}

	public static class RaycastOptions
	{
		[Save] public static bool Enabled = true;
		[Save] public static bool UseRandomLimb = false;
		[Save] public static bool UseCustomLimb = false;
		[Save] public static bool UseTargetMaterial = true;
		[Save] public static bool UseModifiedVector = true;
        [Save] public static bool EnablePlayerSelection = true;
        [Save] public static bool OnlyShootAtSelectedPlayer = false;
        [Save] public static float FOV = 10F;

        [Save] public static TargetPriority Target = TargetPriority.Players;
		[Save] public static EPhysicsMaterial TargetMaterial = EPhysicsMaterial.ALIEN_DYNAMIC;
		[Save] public static ELimb TargetLimb = ELimb.SKULL;
		[Save] public static SerializableVector TargetRagdoll = new SerializableVector(0, 10, 0);
	}
}
