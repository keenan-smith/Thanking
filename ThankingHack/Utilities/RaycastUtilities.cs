using System.Linq;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Thanking.Options.AimOptions;
using Thanking.Utilities.Mesh_Utilities;
using UnityEngine;
using Thanking.Options;
using System.Collections.Generic;
using Thanking.Variables;
using Thanking.Coroutines;

namespace Thanking.Utilities
{
	public static class RaycastUtilities
    {
		public static GameObject[] Objects = new GameObject[0];

		public static RaycastInfo GenerateOriginalRaycast(Ray ray, float range, int mask)
		{
			PhysicsUtility.raycast(ray, out RaycastHit hit, range, mask);
			RaycastInfo raycastInfo = new RaycastInfo(hit) {direction = ray.direction};

			if (hit.transform == null) return raycastInfo;
			
			if (hit.transform.CompareTag("Enemy"))
				raycastInfo.player = DamageTool.getPlayer(raycastInfo.transform);
				
			if (hit.transform.CompareTag("Zombie"))
				raycastInfo.zombie = DamageTool.getZombie(raycastInfo.transform);
			if (hit.transform.CompareTag("Animal"))
				raycastInfo.animal = DamageTool.getAnimal(raycastInfo.transform);

			raycastInfo.limb = DamageTool.getLimb(raycastInfo.transform);

			if (hit.transform.CompareTag("Vehicle"))
				raycastInfo.vehicle = DamageTool.getVehicle(raycastInfo.transform);

			if (raycastInfo.zombie != null && raycastInfo.zombie.isRadioactive)
				raycastInfo.material = EPhysicsMaterial.ALIEN_DYNAMIC;

			else
				raycastInfo.material = DamageTool.getMaterial(hit.point, hit.transform, hit.collider);
			
			return raycastInfo;
		}

		/* TODO
		public static RaycastInfo GenerateAimbotRaycast()
		{
			GameObject Locked = AimbotCoroutines.LockedObject;
			ItemGunAsset currentGun = Player.player.equipment.asset as ItemGunAsset;
			float range = currentGun != null ? currentGun.range : MiscOptions.ExtendMeleeRange ? MiscOptions.MeleeRangeExtension : 1.75f;

			if (Locked == null)
				return GenerateOriginalRaycast(new Ray(Player.player.look.aim.position, Player.player.look.aim.forward),
				   range, RayMasks.DAMAGE_CLIENT);


		}
		*/

        public static bool GenerateRaycast(out RaycastInfo info)
        {
            Vector3 aimPos = Player.player.look.aim.position;
            ItemGunAsset currentGun = Player.player.equipment.asset as ItemGunAsset;
			float Range = currentGun != null ? currentGun.range : MiscOptions.ExtendMeleeRange ? MiscOptions.MeleeRangeExtension : 1.75f;

			if (!GetClosestHit(Objects, out double Distance, out RaycastHit Hit) || Distance > Range)
			{
				info = GenerateOriginalRaycast(new Ray(aimPos, Player.player.look.aim.forward),
				   Range, RayMasks.DAMAGE_CLIENT);

				return false;
			}
				
	        info = new RaycastInfo(Hit)
	        {
		        point = Player.player.transform.position,
		        direction = RaycastOptions.TargetRagdoll.ToVector(),
		        limb = RaycastOptions.TargetLimb,
		        player = Hit.transform.GetComponent<Player>(),
		        material = RaycastOptions.TargetMaterial
	        };

			return true;
        }
	    
		public static bool GetClosestHit(GameObject[] Objects, out double closestDistance, out RaycastHit Hit)
		{
			GameObject ClosestObject = null;
			double ClosestDistance = 1337420;
			ItemGunAsset CurrentGun = Player.player.equipment.asset as ItemGunAsset;

			Hit = new RaycastHit();

			for (int i = 0; i < Objects.Length; i++)
			{
				GameObject go = Objects[i];
				
				if (go == null)
					continue;

				Vector3 AimPos = Player.player.transform.position;
				float Range = CurrentGun != null ? CurrentGun.range : 15.5f;

				if (VectorUtilities.GetDistance(AimPos, go.transform.position) > Range)
					continue;
				
				if (!SphereUtilities.GetRaycast(go, AimPos, Range, out RaycastHit _Hit))
					continue;
				
				if (ClosestObject == null)
				{
					ClosestObject = go;
					Hit = _Hit;
					continue;
				}

				double LatestDistance =
					VectorUtilities.GetDistance(Player.player.transform.position, go.transform.position);

				if (ClosestDistance < LatestDistance) continue;

				ClosestObject = go;
				Hit = _Hit;
				ClosestDistance = LatestDistance;
			}

			closestDistance = ClosestDistance;
			return ClosestObject;
		}
    }
}
