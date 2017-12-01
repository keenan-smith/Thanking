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
			
	        #if DEBUG
	        DebugUtilities.Log($"Players[] Length: {Players.Length}");
			#endif
	        
	        GameObject ClosestObject = GetClosestHittableObject(Objects, out double ClosestDistance);

			Debug.Log(ClosestObject.name);
#if DEBUG
	        DebugUtilities.Log($"Closest Player Name: {ClosestPlayer.name}");
	        DebugUtilities.Log($"Closest Distance: {ClosestDistance}");
#endif

			float range = currentGun != null ? currentGun.range : MiscOptions.ExtendMeleeRange ? MiscOptions.MeleeRangeExtension : 1.75f;


			if (ClosestObject == null)
			{
				info = GenerateOriginalRaycast(new Ray(aimPos, Player.player.look.aim.forward),
				   range, RayMasks.DAMAGE_CLIENT);

				return false;
			}

	        if (currentGun != null)
	        {
				Player p = ClosestObject.GetComponent<Player>();

				if (p == null)
				{
					RaycastHit hPos = SphereUtilities.Get(ClosestObject, aimPos, SphereOptions.SphereRadius);
					info = new RaycastInfo(hPos)
					{
						direction = RaycastOptions.TargetRagdoll.ToVector(),
						material = RaycastOptions.TargetMaterial
					};
				}
				else
				{
					RaycastHit hPos = SphereUtilities.Get(p, aimPos);
					info = new RaycastInfo(hPos)
					{
						direction = RaycastOptions.TargetRagdoll.ToVector(),
						limb = RaycastOptions.TargetLimb,
						player = p,
						material = RaycastOptions.TargetMaterial
					};
				}

				return true;
			}

	        if (ClosestDistance > range)
			{
				info = GenerateOriginalRaycast(new Ray(aimPos, Player.player.look.aim.forward),
				   range, RayMasks.DAMAGE_CLIENT);

				return false;
			}
				
	        //PlayerUI.hitmark(10, Vector3.zero, false, EPlayerHit.CRITICAL);
	        info = new RaycastInfo(ClosestObject.transform)
	        {
		        point = Player.player.transform.position,
		        direction = RaycastOptions.TargetRagdoll.ToVector(),
		        limb = RaycastOptions.TargetLimb,
		        player = ClosestObject.GetComponent<Player>(),
		        material = RaycastOptions.TargetMaterial
	        };

			return true;
        }
	    
		public static GameObject GetClosestHittableObject(GameObject[] Objects, out double closestDistance)
		{
			GameObject ClosestObject = null;
			double ClosestDistance = 1337420;
			ItemGunAsset CurrentGun = Player.player.equipment.asset as ItemGunAsset;

			for (int i = 0; i < Objects.Length; i++)
			{
				GameObject go = Objects[i];
				
				if (go == null)
					continue;
				
				if (VectorUtilities.GetDistance(Player.player.transform.position, go.transform.position) > (CurrentGun != null ? CurrentGun.range : 15.5f))
					continue;
				
				if (SphereUtilities.Get(go, Player.player.look.aim.position, SphereOptions.SphereRadius).point == Vector3.zero)
					continue;
				
				if (ClosestObject == null)
				{
					ClosestObject = go;
					continue;
				}

				double LatestDistance =
					VectorUtilities.GetDistance(Player.player.transform.position, go.transform.position);

				if (ClosestDistance < LatestDistance) continue;

				ClosestObject = go;
				ClosestDistance = LatestDistance;
			}

			closestDistance = ClosestDistance;
			return ClosestObject;
		}
    }
}
