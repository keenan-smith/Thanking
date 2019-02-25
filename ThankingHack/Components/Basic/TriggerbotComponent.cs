using System.Collections;
using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Coroutines;
using Thanking.Options.AimOptions;
using Thanking.Overrides;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Components.Basic
{
	[Component]
	public class TriggerbotComponent : MonoBehaviour
	{
		public static FieldInfo CurrentFiremode;

		[Initializer]
		public static void Init() =>
			CurrentFiremode = typeof(UseableGun).GetField("firemode", BindingFlags.NonPublic | BindingFlags.Instance);

		public void Start() =>
			StartCoroutine(CheckTrigger());
		
		public static IEnumerator CheckTrigger()
		{
			while (true)
			{
				yield return new WaitForSeconds(0.1f);

				if (!TriggerbotOptions.Enabled || !DrawUtilities.ShouldRun() || 
				    OptimizationVariables.MainPlayer.stance.stance == EPlayerStance.SPRINT || 
				    OptimizationVariables.MainPlayer.stance.stance == EPlayerStance.CLIMB ||
				    OptimizationVariables.MainPlayer.stance.stance == EPlayerStance.DRIVING)
				{
					TriggerbotOptions.IsFiring = false;
					continue;
				}
				
				PlayerLook look = OptimizationVariables.MainPlayer.look;
				Useable u = OptimizationVariables.MainPlayer.equipment.useable;

				switch (u)
				{
					case null:
						TriggerbotOptions.IsFiring = false;
						continue;

					case UseableGun gun:
					{
						ItemGunAsset PAsset = (ItemGunAsset)OptimizationVariables.MainPlayer.equipment.asset;
						RaycastInfo ri = RaycastUtilities.GenerateOriginalRaycast(new Ray(look.aim.position, look.aim.forward), PAsset.range, RayMasks.DAMAGE_CLIENT);
						
						if (AimbotCoroutines.LockedObject != null && AimbotCoroutines.IsAiming)
						{
							Ray r = OV_UseableGun.GetAimRay(look.aim.position, AimbotCoroutines.GetAimPosition(AimbotCoroutines.LockedObject.transform, "Skull"));
							ri = RaycastUtilities.GenerateOriginalRaycast(new Ray(r.origin, r.direction), PAsset.range, RayMasks.DAMAGE_CLIENT);
						}

						bool Valid = ri.player != null;
						
						if (RaycastOptions.Enabled)
							Valid = RaycastUtilities.GenerateRaycast(out ri);

						if (!Valid)
						{
							TriggerbotOptions.IsFiring = false;
							continue;
						}

						EFiremode fire = (EFiremode) CurrentFiremode.GetValue(gun);
						if (fire == EFiremode.AUTO)
						{
							TriggerbotOptions.IsFiring = true;
							continue;
						}

						TriggerbotOptions.IsFiring = !TriggerbotOptions.IsFiring;
						break;
					}

					case UseableMelee _:
					{
						ItemMeleeAsset MAsset = (ItemMeleeAsset)OptimizationVariables.MainPlayer.equipment.asset;
						RaycastInfo ri = RaycastUtilities.GenerateOriginalRaycast(new Ray(look.aim.position, look.aim.forward), MAsset.range, RayMasks.DAMAGE_CLIENT);
						
						if (AimbotCoroutines.LockedObject != null && AimbotCoroutines.IsAiming)
						{
							Ray r = OV_UseableGun.GetAimRay(look.aim.position, AimbotCoroutines.GetAimPosition(AimbotCoroutines.LockedObject.transform, "Skull"));
							ri = RaycastUtilities.GenerateOriginalRaycast(new Ray(r.origin, r.direction), MAsset.range, RayMasks.DAMAGE_CLIENT);
						}

						bool Valid = ri.player != null;
						
						if (RaycastOptions.Enabled)
							Valid = RaycastUtilities.GenerateRaycast(out ri);

						if (!Valid)
						{
							TriggerbotOptions.IsFiring = false;
							continue;
						}
						
						if (MAsset.isRepeated)
						{
							TriggerbotOptions.IsFiring = true;
							continue;
						}

						TriggerbotOptions.IsFiring = !TriggerbotOptions.IsFiring;
						break;
					}
				}
			}
		}
	}
}