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
using UnityEngine.PostProcessing;

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
			
			if (RaycastOptions.UseCustomLimb)
				raycastInfo.limb = RaycastOptions.TargetLimb;
			
			else if (RaycastOptions.UseRandomLimb)
			{
				ELimb[] Limbs = (ELimb[]) Enum.GetValues(typeof(ELimb));
				raycastInfo.limb = Limbs[MathUtilities.Random.Next(0, Limbs.Length)];
			}
			
			if (hit.transform.CompareTag("Vehicle"))
				raycastInfo.vehicle = DamageTool.getVehicle(raycastInfo.transform);

			if (raycastInfo.zombie != null && raycastInfo.zombie.isRadioactive)
				raycastInfo.material = EPhysicsMaterial.ALIEN_DYNAMIC;

			else
				raycastInfo.material = DamageTool.getMaterial(hit.point, hit.transform, hit.collider);

			if (RaycastOptions.UseTargetMaterial)
				raycastInfo.material = RaycastOptions.TargetMaterial;
			
			return raycastInfo;
		}

	    public static bool GenerateRaycast(out RaycastInfo info, bool DecreaseRange = false)
	    {
		    ItemGunAsset currentGun = OptimizationVariables.MainPlayer.equipment.asset as ItemGunAsset;
		    float Range = currentGun?.range ?? 15.5f;

		    if (DecreaseRange)
			    Range -= 10;
		    
		    info = GenerateOriginalRaycast(new Ray(OptimizationVariables.MainPlayer.look.aim.position, OptimizationVariables.MainPlayer.look.aim.forward), Range,
			    RayMasks.DAMAGE_CLIENT);
		    
		    GetPlayers();
		    
		    if (!GetClosestObject(Objects, out double Distance, out GameObject Object, out Vector3 Point, Range)) 
			    return false;
		    
		    info = GenerateRaycast(Object, Point);
		    return true;
	    }
	    
        public static RaycastInfo GenerateRaycast(GameObject Object, Vector3 Point)
        {
	        ELimb Limb = RaycastOptions.TargetLimb;

	        if (RaycastOptions.UseRandomLimb)
	        {
		        ELimb[] Limbs = (ELimb[]) Enum.GetValues(typeof(ELimb));
		        Limb = Limbs[MathUtilities.Random.Next(0, Limbs.Length)];
	        }
	        
	        return new RaycastInfo(Object.transform)
	        {
		        point = Point,
		        direction = RaycastOptions.TargetRagdoll.ToVector(),
		        limb = Limb,
		        material = RaycastOptions.TargetMaterial,
		        player = Object.GetComponent<Player>(),
		        zombie = Object.GetComponent<Zombie>(),
		        vehicle = Object.GetComponent<InteractableVehicle>()
	        };
        }
	    
		public static bool GetClosestObject(GameObject[] Objects, out double Distance, out GameObject Object, out Vector3 Point, float Range = -1)
		{
			Distance = 1337420;
			Object = null;
			Point = Vector3.zero;
			
			ItemGunAsset CurrentGun = OptimizationVariables.MainPlayer.equipment.asset as ItemGunAsset;
			Vector3 AimPos = OptimizationVariables.MainPlayer.look.aim.position;

			if (Range == -1)
				Range = CurrentGun?.range ?? 15.5f;

			for (int i = 0; i < Objects.Length; i++)
			{
				GameObject go = Objects[i];
				
				if (go == null)
					continue;
				
				if (go.GetComponent<RaycastComponent>() == null)
				{
					go.AddComponent<RaycastComponent>();
					continue;
				}

				RaycastComponent Component = go.GetComponent<RaycastComponent>();

				if (Component.Radius <= 0)
					continue;
				
				double NewDistance = VectorUtilities.GetDistance(AimPos, go.transform.position);

				if (NewDistance > Range)
					continue;
				
				if (NewDistance > Distance)
					continue;
				
				if (!SphereUtilities.GetRaycast(go, AimPos, Range, out Vector3 _Point))
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
		    if (RaycastOptions.Target != TargetPriority.Players) 
			    return;
		    
		    Objects = Provider.clients
			    .Where(o => !o.player.life.isDead && o.player != OptimizationVariables.MainPlayer && !FriendUtilities.IsFriendly(o.player))
			    .Select(o => o.player.gameObject).ToArray();
	    }
    }
}
