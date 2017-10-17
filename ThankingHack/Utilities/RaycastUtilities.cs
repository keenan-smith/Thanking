using SDG.Unturned;
using System.Linq;
using Thanking.Utilities.Mesh_Utilities;
using UnityEngine;

namespace Thanking.Utilities
{
	public static class RaycastUtilities
    {
        public static RaycastInfo GenerateRaycast()
        {
            Vector3 aimPos = Player.player.look.aim.position;
            Vector3 pPos = Player.player.transform.position;
            ItemGunAsset currentGun = Player.player.equipment.asset as ItemGunAsset;

			if (currentGun != null)
			{
				Player closestPlayer = null;
				for (int i = 0; i < Provider.clients.Count(); i++)
				{
					Player p = Provider.clients[i].player;
					
					if (p == null || p == Player.player || p.life.isDead ||
						p.transform == null || p.quests.isMemberOfSameGroupAs(Player.player)) continue;

					if (VectorUtilities.GetDistance(Player.player.transform.position, p.transform.position) <= currentGun.range + 12)
						if (closestPlayer == null)
							closestPlayer = p;
						else
							if (VectorUtilities.GetDistance(Player.player.transform.position, p.transform.position) < VectorUtilities.GetDistance(Player.player.transform.position, closestPlayer.transform.position))
							closestPlayer = p;
				}

				if (closestPlayer != null)
				{
					GameObject go = null;

					if (closestPlayer.movement.getVehicle() != null)
						go = IcoSphere.Create("HitSphere", 5, 3);
					else
						go = IcoSphere.Create("HitSphere", 15f, 3);

					go.transform.parent = closestPlayer.transform;
					go.transform.localPosition = new Vector3(0, 0, 0);

					Vector3 hPos = SphereUtilities.Get(go, aimPos, RayMasks.DAMAGE_CLIENT);
					Object.Destroy(go);
					if (hPos != Vector3.zero)
					{
						Debug.Log("Found a meme!");
						if (!Provider.modeConfigData.Gameplay.Ballistics)
							PlayerUI.hitmark(10, hPos, true, EPlayerHit.CRITICAL);

						return new RaycastInfo(closestPlayer.transform)
						{
							point = hPos,
							direction = Vector3.up * 10,
							limb = ELimb.SKULL,
							player = closestPlayer,
							material = EPhysicsMaterial.ALIEN_DYNAMIC
						};
					}

					if (VectorUtilities.GetDistance(Player.player.transform.position, closestPlayer.transform.position) <= 15.5f)
						return new RaycastInfo(closestPlayer.transform)
						{
							point = Player.player.transform.position,
							direction = Vector3.up * 10,
							limb = ELimb.SKULL,
							player = closestPlayer,
							material = EPhysicsMaterial.ALIEN_DYNAMIC
						};
				}
			}

			return new RaycastInfo(Player.player.transform)
			{
				point = Vector3.zero
			};
        }
    }
}
