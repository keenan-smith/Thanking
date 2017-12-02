using System.Linq;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Thanking.Options.AimOptions;
using Thanking.Utilities.Mesh_Utilities;
using UnityEngine;
using Thanking.Options;
using System.Collections.Generic;
using Thanking.Components.MultiAttach;
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
	        GetObjects();
	        
            Vector3 aimPos = Player.player.look.aim.position;
            ItemGunAsset currentGun = Player.player.equipment.asset as ItemGunAsset;
			float Range = currentGun?.range ?? (MiscOptions.ExtendMeleeRange ? MiscOptions.MeleeRangeExtension : 1.75f);

			if (!GetClosestObject(Objects, out double Distance, out GameObject Object))
			{
				info = GenerateOriginalRaycast(new Ray(Player.player.look.aim.position, Player.player.look.aim.forward), Range,
					RayMasks.DAMAGE_CLIENT);
				
				return false;
			}

	        if (!SphereUtilities.GetRaycast(Object, aimPos, Range, out Vector3 Point))
	        {
		        info = GenerateOriginalRaycast(new Ray(Player.player.look.aim.position, Player.player.look.aim.forward), Range,
			        RayMasks.DAMAGE_CLIENT);
				
		        return false;
	        }
	        
	        info = new RaycastInfo(Object.transform)
	        {
		        point = Point,
		        direction = RaycastOptions.TargetRagdoll.ToVector(),
		        limb = RaycastOptions.TargetLimb,
		        player = Object.GetComponent<Player>(),
		        material = RaycastOptions.TargetMaterial
	        };

			return true;
        }
	    
		public static bool GetClosestObject(GameObject[] Objects, out double Distance, out GameObject Object)
		{
			Distance = 1337420;
			Object = null;
			
			ItemGunAsset CurrentGun = Player.player.equipment.asset as ItemGunAsset;

			for (int i = 0; i < Objects.Length; i++)
			{
				GameObject go = Objects[i];
				
				if (go == null)
					continue;

				Vector3 AimPos = Player.player.look.aim.position;
				float Range = CurrentGun?.range ?? 15.5f;

				double NewDistance = VectorUtilities.GetDistance(AimPos, go.transform.position);
				
				if (NewDistance > Range)
					continue;
				
				if (NewDistance > Distance)
					continue;
				
				Object = go;
				Distance = NewDistance;
			}

			return Object != null;
		}

	    public static void GetObjects()
	    {
		    if (RaycastOptions.Target == TargetPriority.Players) //only need to do this here 'cause players have specific properties that make it annoying to do shit with them xd
		    {
			    for (int i = 0; i < RaycastUtilities.Objects.Length; i++)
				    RaycastUtilities.Objects[i].AddComponent<VelocityComponent>();
			    
			    RaycastUtilities.Objects = Provider.clients.Where(o => !o.player.life.isDead && o.player != Player.player).Select(o => o.player.gameObject).ToArray();
		    }
	    }
    }
}
