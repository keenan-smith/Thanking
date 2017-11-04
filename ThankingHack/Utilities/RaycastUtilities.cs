using SDG.Framework.Utilities;
using SDG.Unturned;
using System.Linq;
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
			RaycastHit hit;
			PhysicsUtility.raycast(ray, out hit, range, mask, QueryTriggerInteraction.UseGlobal);
			RaycastInfo raycastInfo = new RaycastInfo(hit);
			raycastInfo.direction = ray.direction;
			if (hit.transform != null)
			{
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
			}
			return raycastInfo;
		}

        public static RaycastInfo GenerateRaycast()
        {
            Vector3 aimPos = Player.player.look.aim.position;
            Vector3 pPos = Player.player.transform.position;
            ItemGunAsset currentGun = Player.player.equipment.asset as ItemGunAsset;

			Player closestPlayer = null;
			for (int i = 0; i < Provider.clients.Count(); i++)
			{
				Player p = Provider.clients[i].player;

				if (p == null || p == Player.player || p.life.isDead ||
					p.transform == null || p.quests.isMemberOfSameGroupAs(Player.player)) continue;

				if (currentGun != null)
				{
					if (VectorUtilities.GetDistance(Player.player.transform.position, p.transform.position) <= currentGun.range + (RaycastOptions.ExtendedRange ? 12 : 0))
					{
						if (closestPlayer == null)
							closestPlayer = p;
						else if (VectorUtilities.GetDistance(Player.player.transform.position, p.transform.position) < VectorUtilities.GetDistance(Player.player.transform.position, closestPlayer.transform.position))
							closestPlayer = p;
					}
				}
				else
				{
					if (VectorUtilities.GetDistance(Player.player.transform.position, p.transform.position) <= 15.5f)
					{
						if (closestPlayer == null)
							closestPlayer = p;
						else if (VectorUtilities.GetDistance(Player.player.transform.position, p.transform.position) < VectorUtilities.GetDistance(Player.player.transform.position, closestPlayer.transform.position))
							closestPlayer = p;
					}
				}
			}

			if (closestPlayer != null)
			{
				if (currentGun != null)
				{ 
					GameObject go = null;

					if (closestPlayer.movement.getVehicle() != null)
						go = IcoSphere.Create("HitSphere", SphereOptions.VehicleSphereRadius, SphereOptions.RecursionLevel);
					else
						go = IcoSphere.Create("HitSphere", SphereOptions.SphereRadius, SphereOptions.RecursionLevel);

					go.transform.parent = closestPlayer.transform;
					go.transform.localPosition = new Vector3(0, 0, 0);

					Vector3 hPos = SphereUtilities.Get(go, aimPos, RayMasks.DAMAGE_CLIENT);
					Object.Destroy(go);
					if (hPos != Vector3.zero)
					{
						if (!Provider.modeConfigData.Gameplay.Ballistics)
							//PlayerUI.hitmark(10, Vector3.zero, false, EPlayerHit.CRITICAL);

						Loader.HookObject.GetComponent<CoroutineComponent>().StartCoroutine(WeaponCoroutines.CheckForDeath(closestPlayer));

						return new RaycastInfo(closestPlayer.transform)
						{
							point = hPos,
							direction = RaycastOptions.TargetRagdoll.ToVector(),
							limb = RaycastOptions.TargetLimb,
							player = closestPlayer,
							material = RaycastOptions.TargetMaterial
						};
					}
				}

				if (VectorUtilities.GetDistance(Player.player.transform.position, closestPlayer.transform.position) <= SphereOptions.SphereRadius)
				{
					//PlayerUI.hitmark(10, Vector3.zero, false, EPlayerHit.CRITICAL);
					Loader.HookObject.GetComponent<CoroutineComponent>().StartCoroutine(WeaponCoroutines.CheckForDeath(closestPlayer));
					return new RaycastInfo(closestPlayer.transform)
					{
						point = Player.player.transform.position,
						direction = RaycastOptions.TargetRagdoll.ToVector(),
						limb = RaycastOptions.TargetLimb,
						player = closestPlayer,
						material = RaycastOptions.TargetMaterial
					};
				}
			}


			RaycastInfo ri = GenerateOriginalRaycast(new Ray(Player.player.look.aim.position, Player.player.look.aim.forward), currentGun.range, RayMasks.DAMAGE_CLIENT);

			return ri;
        }
    }
}
