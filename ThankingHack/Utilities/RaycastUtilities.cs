using System.Linq;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Thanking.Components.Basic;
using Thanking.Coroutines;
using Thanking.Options.AimOptions;
using Thanking.Utilities.Mesh_Utilities;
using UnityEngine;

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

	        SteamPlayer[] Players = Provider.clients.Where(p => p.player != null && p.player != Player.player &&
	                                							!p.player.life.isDead && p.player.transform != null &&
	                                                            !FriendUtilities.IsFriendly(p.player)).ToArray();
	        
	        #if DEBUG
	        DebugUtilities.Log($"Players[] Length: {Players.Length}");
			#endif
	        
	        Player ClosestPlayer = GetClosestPlayer(Players, out double ClosestDistance);
	        
			#if DEBUG
	        DebugUtilities.Log($"Closest Player Name: {ClosestPlayer.name}");
	        DebugUtilities.Log($"Closest Distance: {ClosestDistance}");
			#endif

	        if (ClosestPlayer == null)
		        return GenerateOriginalRaycast(new Ray(Player.player.look.aim.position, Player.player.look.aim.forward),
			        currentGun.range, RayMasks.DAMAGE_CLIENT);
	        
	        if (currentGun != null)
	        { 
		        GameObject go = IcoSphere.Create("HitSphere",
			        ClosestPlayer.movement.getVehicle() != null ? SphereOptions.VehicleSphereRadius : SphereOptions.SphereRadius,
			        SphereOptions.RecursionLevel);

		        go.transform.parent = ClosestPlayer.transform;
		        go.transform.localPosition = new Vector3(0, 0, 0);

		        Vector3 hPos = SphereUtilities.Get(go, aimPos, RayMasks.DAMAGE_CLIENT);
		        Object.Destroy(go);
		        if (hPos != Vector3.zero)
		        {
			        //if (!Provider.modeConfigData.Gameplay.Ballistics)
			        //PlayerUI.hitmark(10, Vector3.zero, false, EPlayerHit.CRITICAL);
					
			        return new RaycastInfo(ClosestPlayer.transform)
			        {
				        point = hPos,
				        direction = RaycastOptions.TargetRagdoll.ToVector(),
				        limb = RaycastOptions.TargetLimb,
				        player = ClosestPlayer,
				        material = RaycastOptions.TargetMaterial
			        };
		        }
	        }

	        if (ClosestDistance > SphereOptions.SphereRadius)
		        return GenerateOriginalRaycast(new Ray(Player.player.look.aim.position, Player.player.look.aim.forward),
			        currentGun.range, RayMasks.DAMAGE_CLIENT);
				
	        //PlayerUI.hitmark(10, Vector3.zero, false, EPlayerHit.CRITICAL);
	        return new RaycastInfo(ClosestPlayer.transform)
	        {
		        point = Player.player.transform.position,
		        direction = RaycastOptions.TargetRagdoll.ToVector(),
		        limb = RaycastOptions.TargetLimb,
		        player = ClosestPlayer,
		        material = RaycastOptions.TargetMaterial
	        };
        }
	    
	    private static Player GetClosestPlayer(SteamPlayer[] Players, out double closestDistance)
	    {
		    Player ClosestPlayer = null;
		    double ClosestDistance = 1337420;
		    ItemGunAsset CurrentGun = Player.player.equipment.asset as ItemGunAsset;
		    

		    for (int i = 0; i < Players.Length; i++)
		    {
			    Player Player = Players[i].player;
			    
			    if (CurrentGun != null)
			    {
				    if (!(VectorUtilities.GetDistance(Player.player.transform.position, Player.transform.position) <=
				          CurrentGun.range + (RaycastOptions.ExtendedRange ? 12 : 0))) continue;

				    if (ClosestPlayer == null)
				    {
					    ClosestPlayer = Player;
					    continue;
				    }

				    double LatestDistance =
					    VectorUtilities.GetDistance(Player.player.transform.position, Player.transform.position);

				    if (ClosestDistance < LatestDistance) continue;
				    
				    ClosestPlayer = Player;
				    ClosestDistance = LatestDistance;
			    }
			    else
			    {
				    if (!(VectorUtilities.GetDistance(Player.player.transform.position, Player.transform.position) <=
				          15.5)) continue;

				    if (ClosestPlayer == null)
				    {
					    ClosestPlayer = Player;
					    continue;
				    }

				    double LatestDistance =
					    VectorUtilities.GetDistance(Player.player.transform.position, Player.transform.position);

				    if (ClosestDistance < LatestDistance) continue;
				    
				    ClosestPlayer = Player;
				    ClosestDistance = LatestDistance;
			    }
		    }

		    closestDistance = ClosestDistance;
		    return ClosestPlayer;
	    }
    }
}
