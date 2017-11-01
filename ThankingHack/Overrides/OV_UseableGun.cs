using SDG.Unturned;
using System.Collections.Generic;
using Thanking.Attributes;
using Thanking.Utilities;
using UnityEngine;
using Thanking.Options.AimOptions;
using Thanking.Variables;

namespace Thanking.Overrides
{
    public class OV_UseableGun
    {
		[Override(typeof(UseableGun), "ballistics", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)]
        public void OV_ballistics()
        {
			ItemGunAsset PAsset = ((ItemGunAsset)Player.player.equipment.asset);
			if (((ItemGunAsset)Player.player.equipment.asset).projectile != null)
					return;

			List<BulletInfo> Bullets = Player.player.equipment.useable.GetField<List<BulletInfo>>("bullets", ReflectionUtilities.FieldType.Private);

			if (Provider.modeConfigData.Gameplay.Ballistics)
			{
				if (RaycastOptions.Enabled)
				{
					for (int i = 0; i < Bullets.Count; i++)
					{
						BulletInfo bulletInfo = Bullets[i];
						RaycastInfo ri = RaycastUtilities.GenerateRaycast();
						float distance = VectorUtilities.GetDistance(Player.player.transform.position, ri.point);
						if (bulletInfo.steps * PAsset.ballisticTravel >= distance && ri.point != Vector3.zero)
						{
                            if (ri.animal || ri.player || ri.zombie)
							    PlayerUI.hitmark(0, Vector3.zero, false, EPlayerHit.CRITICAL);
							Player.player.input.sendRaycast(ri);
							bulletInfo.steps = 254;
						}
					}
				}
				else
				{
                    for (int i = 0; i < Bullets.Count; i++)
                    {
                        BulletInfo bulletInfo = Bullets[i];
                        RaycastInfo ri = RaycastUtilities.GenerateOriginalRaycast(new Ray(Player.player.look.aim.position, Player.player.look.aim.forward), PAsset.range, RayMasks.DAMAGE_CLIENT);
                        float distance = VectorUtilities.GetDistance(Player.player.transform.position, ri.point);
                        if (bulletInfo.steps * PAsset.ballisticTravel >= distance && ri.point != Vector3.zero)
                        {
                            if (ri.animal || ri.player || ri.zombie)
                                PlayerUI.hitmark(0, Vector3.zero, false, EPlayerHit.CRITICAL);
                            Player.player.input.sendRaycast(ri);
                            bulletInfo.steps = 254;
                        }
                        
                    }
                }

				for (int k = Bullets.Count - 1; k >= 0; k--)
				{
					BulletInfo bulletInfo = Bullets[k];
					bulletInfo.steps += 1;
					if (bulletInfo.steps >= PAsset.ballisticSteps)
						Bullets.RemoveAt(k);
				}
			}
			else
			{
				if (RaycastOptions.Enabled)
				{
					for (int i = 0; i < Bullets.Count; i++)
					{
						BulletInfo bulletInfo = Bullets[i];
						RaycastInfo ri = RaycastUtilities.GenerateRaycast();
						Player.player.input.sendRaycast(ri);
					}
				}
				else
                {
                    for (int i = 0; i < Bullets.Count; i++)
                    {
                        BulletInfo bulletInfo = Bullets[i];
                        RaycastInfo ri = RaycastUtilities.GenerateOriginalRaycast(new Ray(Player.player.look.aim.position, Player.player.look.aim.forward), PAsset.range, RayMasks.DAMAGE_CLIENT);
                        Player.player.input.sendRaycast(ri);
                    }
                }

				Bullets.Clear();
			}
        }
    }
}
