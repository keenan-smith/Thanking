using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            SteamPlayer[] players = Provider.clients.Where(p => VectorUtilities.GetDistance(p.player.transform.position, pPos) <= currentGun.range + 18).ToArray();

            if (players.Length > 0)
            {
                players.OrderBy(p => VectorUtilities.GetDistance(p.player.transform.position, pPos));
                for (int i = 0; i < players.Length; i++)
                {
                    Player p = players[i].player;
                    GameObject go = IcoSphere.Create("HitSphere", 15, 3);

                    go.transform.parent = p.transform;
                    go.layer = LayerMasks.ENEMY;

                    if (SphereUtilities.Get(go, aimPos, RayMasks.DAMAGE_SERVER) != Vector3.zero)
					{
						PlayerUI.hitmark(0, Vector3.zero, false, EPlayerHit.CRITICAL);
						Physics.Raycast(aimPos, (p.transform.position - aimPos).normalized, out RaycastHit hit, currentGun.range + 18, RayMasks.ENEMY);
                        return new RaycastInfo(hit)
                        {
                            direction = Vector3.up * 10,
                            limb = ELimb.SKULL,
                            player = p,
                            material = EPhysicsMaterial.ALIEN_DYNAMIC
                        };
					}
				}
				PlayerUI.hitmark(0, Vector3.zero, false, EPlayerHit.CRITICAL);
				return new RaycastInfo(Player.player.transform)
                {
                    direction = Vector3.up * 10,
                    limb = ELimb.SKULL,
                    player = players[0].player,
                    material = EPhysicsMaterial.ALIEN_DYNAMIC
                };
			}

            return new RaycastInfo(Player.player.transform);
        }
    }
}
