using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Overrides
{
    public class UseableGun
    {
        public static List<BulletInfo> Bullets = new List<BulletInfo>();

        [Override(typeof(SDG.Unturned.UseableGun), "ballistics", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)]
        public void OV_ballistics()
        {
            ItemGunAsset PAsset = ((ItemGunAsset)Player.player.equipment.asset);
            if (((ItemGunAsset) Player.player.equipment.asset).projectile != null)
                return;

            if (Provider.modeConfigData.Gameplay.Ballistics)
            {
                //stuff here
            }
            else
            {
                for (int i = 0; i < Bullets.Count; i++)
                {
                    BulletInfo bulletInfo = Bullets[i];
                    RaycastInfo ri = RaycastUtilities.GenerateRaycast();
                    Player.player.input.sendRaycast(ri);
                }

                PlayerUI.hitmark(0, Vector3.zero, false, EPlayerHit.CRITICAL);
            }
        }
    }
}
