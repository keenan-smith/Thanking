using System.Linq;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Thanking.Options.AimOptions;
using Thanking.Utilities.Mesh_Utilities;
using UnityEngine;
using Thanking.Options;
using System.Collections.Generic;
using Thanking.Variables;

namespace Thanking.Utilities
{
	public static class RaycastUtilities
    {
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

        public static RaycastInfo GenerateRaycast()
        {
            Vector3 aimPos = Player.player.look.aim.position;
            ItemGunAsset currentGun = Player.player.equipment.asset as ItemGunAsset;

			GameObject[] Objects = new GameObject[0];

			switch (RaycastOptions.Target)
			{
				case TargetPriority.Beds:
					Objects = ESPVariables.Objects.Where(o => o.Target == ESPTarget.Beds).Select(o => o.GObject).ToArray();
					break;
				case TargetPriority.ClaimFlags:
					Objects = ESPVariables.Objects.Where(o => o.Target == ESPTarget.ClaimFlags).Select(o => o.GObject).ToArray();
					break;
				case TargetPriority.Players:
					Objects = ESPVariables.Objects.Where(o => o.Target == ESPTarget.Players).Where(o => o.Object != null && !((Player)o.Object).life.isDead).Select(o => o.GObject).ToArray();
					break;
				case TargetPriority.Sentries:
					Objects = ESPVariables.Objects.Where(o => o.Target == ESPTarget.Sentries).Select(o => o.GObject).ToArray();
					break;
				case TargetPriority.Storage:
					Objects = ESPVariables.Objects.Where(o => o.Target == ESPTarget.Storage).Select(o => o.GObject).ToArray();
					break;
				case TargetPriority.Zombies:
					Objects = ESPVariables.Objects.Where(o => o.Target == ESPTarget.Zombies).Select(o => o.GObject).ToArray();
					break;
			}                             
	        
	        #if DEBUG
	        DebugUtilities.Log($"Players[] Length: {Players.Length}");
			#endif
	        
	        GameObject ClosestObject = GetClosestHittableObject(Objects, out double ClosestDistance);

#if DEBUG
	        DebugUtilities.Log($"Closest Player Name: {ClosestPlayer.name}");
	        DebugUtilities.Log($"Closest Distance: {ClosestDistance}");
#endif

			float range = currentGun != null ? currentGun.range : MiscOptions.ExtendMeleeRange ? MiscOptions.MeleeRangeExtension : 1.75f;


			if (ClosestObject == null)
		        return GenerateOriginalRaycast(new Ray(Player.player.look.aim.position, Player.player.look.aim.forward),
			       range, RayMasks.DAMAGE_CLIENT);
	        
	        if (currentGun != null)
	        {
				Player p = ClosestObject.GetComponent<Player>();

				if (p == null)
				{
					Vector3 hPos = SphereUtilities.Get(ClosestObject, aimPos, SphereOptions.SphereRadius, RayMasks.DAMAGE_CLIENT);
					if (hPos != Vector3.zero)
					{
						return new RaycastInfo(ClosestObject.transform)
						{
							point = hPos,
							direction = RaycastOptions.TargetRagdoll.ToVector(),
							limb = RaycastOptions.TargetLimb,
							material = RaycastOptions.TargetMaterial
						};
					}
				}
				else
				{
					Vector3 hPos = SphereUtilities.Get(p, aimPos, RayMasks.DAMAGE_CLIENT);
					if (hPos != Vector3.zero)
					{
						return new RaycastInfo(ClosestObject.transform)
						{
							point = hPos,
							direction = RaycastOptions.TargetRagdoll.ToVector(),
							limb = RaycastOptions.TargetLimb,
							player = p,
							material = RaycastOptions.TargetMaterial
						};
					}
				}
	        }

	        if (ClosestDistance > SphereOptions.SphereRadius)
		        return GenerateOriginalRaycast(new Ray(Player.player.look.aim.position, Player.player.look.aim.forward),
			        range, RayMasks.DAMAGE_CLIENT);
				
	        //PlayerUI.hitmark(10, Vector3.zero, false, EPlayerHit.CRITICAL);
	        return new RaycastInfo(ClosestObject.transform)
	        {
		        point = Player.player.transform.position,
		        direction = RaycastOptions.TargetRagdoll.ToVector(),
		        limb = RaycastOptions.TargetLimb,
		        player = ClosestObject.GetComponent<Player>(),
		        material = RaycastOptions.TargetMaterial
	        };
        }
	    
		private static GameObject GetClosestHittableObject(GameObject[] Objects, out double closestDistance)
		{
			GameObject ClosestObject = null;
			double ClosestDistance = 1337420;
			ItemGunAsset CurrentGun = Player.player.equipment.asset as ItemGunAsset;

			for (int i = 0; i < Objects.Length; i++)
			{
				GameObject go = Objects[i];
				if (go == null)
					continue;

				if (VectorUtilities.GetDistance(Player.player.transform.position, go.transform.position) > (CurrentGun != null ? CurrentGun.range : 15.5f)) continue;

				if (SphereUtilities.Get(go, Player.player.transform.position, SphereOptions.SphereRadius, RayMasks.DAMAGE_CLIENT) == Vector3.zero)
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
