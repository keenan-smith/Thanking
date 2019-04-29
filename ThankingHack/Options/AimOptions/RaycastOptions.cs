using System.Collections.Generic;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Misc;
using Thanking.Misc.Enums;
using Thanking.Misc.Serializables;

namespace Thanking.Options.AimOptions
{
	public static class RaycastOptions
	{
		[Save] public static bool Enabled = true;
        [Save] public static bool HoldKey = true;
		[Save] public static bool UseRandomLimb = false;
		[Save] public static bool UseCustomLimb = false;
		[Save] public static bool UseTargetMaterial = true;
		[Save] public static bool UseModifiedVector = true;
        [Save] public static bool EnablePlayerSelection = false;
        [Save] public static bool OnlyShootAtSelectedPlayer = false;
        [Save] public static float SelectedFOV = 10F;
		[Save] public static bool SilentAimUseFOV = false;
		[Save] public static float SilentAimFOV = 10F;

        [Save] public static HashSet<TargetPriority> Targets = new HashSet<TargetPriority> {TargetPriority.Players, TargetPriority.Zombies};
		[Save] public static EPhysicsMaterial TargetMaterial = EPhysicsMaterial.ALIEN_DYNAMIC;
		[Save] public static ELimb TargetLimb = ELimb.SKULL;
		[Save] public static SerializableVector TargetRagdoll = new SerializableVector(0, 10, 0);
	}
}
