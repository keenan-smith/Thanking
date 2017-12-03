using System;
using System.Linq;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Thanking.Options.AimOptions;
using Thanking.Utilities.Mesh_Utilities;
using UnityEngine;
using Thanking.Options;
using System.Collections.Generic;
using Thanking.Components.Basic;
using Thanking.Variables;
using Thanking.Coroutines;

namespace Thanking.Utilities
{
	public static class RaycastUtilities
    {
		public static GameObject[] Objects = new GameObject[0];
		public static List<GameObject> AttachedObjects = new List<GameObject>();
	    
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

	    public static bool GenerateRaycast(out RaycastInfo info)
	    {
		    ItemGunAsset currentGun = Player.player.equipment.asset as ItemGunAsset;
		    float Range = currentGun?.range ?? (MiscOptions.ExtendMeleeRange ? MiscOptions.MeleeRangeExtension : 1.75f);
		    
		    info = GenerateOriginalRaycast(new Ray(Player.player.look.aim.position, Player.player.look.aim.forward), Range,
			    RayMasks.DAMAGE_CLIENT);
		    
		    GetPlayers();
		    
		    if (!GetClosestObject(Objects, out double Distance, out GameObject Object, out Vector3 Point))
			    return false;

		    if (Object.GetComponent<VelocityComponent>() != null) 
			    return GenerateRaycast(Object, Point, out info);
		    
		    Object.AddComponent<VelocityComponent>();
		    return false;
	    }
	    
        public static bool GenerateRaycast(GameObject go, Vector3 Point, out RaycastInfo info)
        {
            Vector3 aimPos = Player.player.look.aim.position;
            ItemGunAsset currentGun = Player.player.equipment.asset as ItemGunAsset;
			float Range = currentGun?.range ?? (MiscOptions.ExtendMeleeRange ? MiscOptions.MeleeRangeExtension : 1.75f);

	        info = GenerateOriginalRaycast(new Ray(Player.player.look.aim.position, Player.player.look.aim.forward), Range,
		        RayMasks.DAMAGE_CLIENT);

	        ELimb Limb = RaycastOptions.TargetLimb;

	        if (RaycastOptions.UseRandomLimb)
	        {
		        ELimb[] Limbs = (ELimb[]) Enum.GetValues(typeof(ELimb));
		        Limb = Limbs[MathUtilities.Random.Next(0, Limbs.Length)];
	        }
	        
	        info = new RaycastInfo(go.transform)
	        {
		        point = Point,
		        direction = RaycastOptions.TargetRagdoll.ToVector(),
		        limb = Limb,
		        player = go.GetComponent<Player>(),
		        material = RaycastOptions.TargetMaterial
	        };

			return true;
        }
	    
		public static bool GetClosestObject(GameObject[] Objects, out double Distance, out GameObject Object, out Vector3 Point)
		{
			Distance = 1337420;
			Object = null;
			Point = new Vector3();
			
			ItemGunAsset CurrentGun = Player.player.equipment.asset as ItemGunAsset;

			for (int i = 0; i < Objects.Length; i++)
			{
				GameObject go = Objects[i];
				
				if (go == null)
					continue;

				Vector3 AimPos = Player.player.look.aim.position;
				float Range = CurrentGun?.range ?? 15.5f;

				if (!SphereUtilities.GetRaycast(go, AimPos, Range, out Vector3 _Point))
					continue;
				
				double NewDistance = VectorUtilities.GetDistance(AimPos, go.transform.position);
				
				if (NewDistance > Range)
					continue;
				
				if (NewDistance > Distance)
					continue;
				
				Object = go;
				Distance = NewDistance;
				Point = _Point;
			}

			return Object != null;
		}

	    //only need to do this here 'cause players have specific properties that make it annoying to do shit with them xd
	    public static void GetPlayers()
	    {
		    if (RaycastOptions.Target == TargetPriority.Players)
			    RaycastUtilities.Objects = Provider.clients
				    .Where(o => !o.player.life.isDead && o.player != Player.player && !FriendUtilities.IsFriendly(o.player))
				    .Select(o => o.player.gameObject).ToArray();
	    }
    }
}
