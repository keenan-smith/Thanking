using SDG.Unturned;
using System.Collections.Generic;
using Thanking.Attributes;
using Thanking.Utilities;
using Thanking.Options.UtilityOptions;
using UnityEngine;

namespace Thanking.Overrides
{
    public class OV_UseableGun
    {
        [Override(typeof(UseableGun), "ballistics", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)]
        public void OV_ballistics()
        {
            ItemGunAsset PAsset = ((ItemGunAsset)Player.player.equipment.asset);
            if (((ItemGunAsset) Player.player.equipment.asset).projectile != null)
                return;

            List<BulletInfo> Bullets = (List<BulletInfo>)typeof(UseableGun).GetField("bullets", BFlags.PrivateInstance).GetValue(Player.player.equipment.useable);

			if (Provider.modeConfigData.Gameplay.Ballistics)
			{
				for (int i = 0; i < Bullets.Count; i++)
				{
					BulletInfo bulletInfo = Bullets[i];
					RaycastInfo ri = RaycastUtilities.GenerateRaycast();
					float distance = VectorUtilities.GetDistance(Player.player.transform.position, ri.point);
					if (bulletInfo.steps * PAsset.ballisticTravel >= distance)
					{
						PlayerUI.hitmark(0, Vector3.zero, false, EPlayerHit.CRITICAL);
						Player.player.input.sendRaycast(ri);
						bulletInfo.steps = 254;
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
				for (int i = 0; i < Bullets.Count; i++)
				{
					BulletInfo bulletInfo = Bullets[i];
					RaycastInfo ri = RaycastUtilities.GenerateRaycast();
					Player.player.input.sendRaycast(ri);
				}

				Bullets.Clear();
			}
        }
    }
}
