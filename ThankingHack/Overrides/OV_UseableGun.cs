using System.Collections.Generic;
using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Overrides
{
    public class OV_UseableGun
    {
		private static FieldInfo BulletsField;
		private static MethodInfo Trace;

		[Initializer]
		public static void Load()
		{
			BulletsField = typeof(UseableGun).GetField("bullets", ReflectionVariables.PrivateInstance);
			Trace = typeof(UseableGun).GetMethod("trace", ReflectionVariables.PrivateInstance);
		}

		[Override(typeof(UseableGun), "ballistics", BindingFlags.NonPublic | BindingFlags.Instance)]
        public void OV_ballistics()
        {
	        Useable PlayerUse = Player.player.equipment.useable;
            if (RaycastOptions.Enabled)
            {
                ItemGunAsset PAsset = (ItemGunAsset)Player.player.equipment.asset;
                if (((ItemGunAsset)Player.player.equipment.asset).projectile != null)
                    return;

				List<BulletInfo> Bullets = (List<BulletInfo>)BulletsField.GetValue(PlayerUse);

				if (Provider.modeConfigData.Gameplay.Ballistics)
                {
	                RaycastInfo ri = RaycastUtilities.GenerateRaycast();
	                if (ri.player == null)
		                OverrideUtilities.CallOriginal(PlayerUse);
	                
                    for (int i = 0; i < Bullets.Count; i++)
                    {
                        BulletInfo bulletInfo = Bullets[i];
                        double distance = VectorUtilities.GetDistance(Player.player.transform.position, ri.point);

                        if (bulletInfo.steps > 0 || PAsset.ballisticSteps <= 1)
                        {
	                        Trace.Invoke(PlayerUse,
		                        PAsset.ballisticTravel < 32f
			                        ? new object[] {bulletInfo.pos + bulletInfo.dir * 32f, bulletInfo.dir}
			                        : new object[]
			                        {
				                        bulletInfo.pos + bulletInfo.dir * Random.Range(32f, PAsset.ballisticTravel), bulletInfo.dir
			                        });
                        }

	                    if (bulletInfo.steps * PAsset.ballisticTravel < distance) 
		                    continue;
	                    
	                    EPlayerHit eplayerhit = CalcHitMarker(PAsset, ref ri);
	                    PlayerUI.hitmark(0, Vector3.zero, false, eplayerhit);
	                    Player.player.input.sendRaycast(ri);
	                    bulletInfo.steps = 254;
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
                        EPlayerHit eplayerhit = CalcHitMarker(PAsset, ref ri);
                        PlayerUI.hitmark(0, Vector3.zero, false, eplayerhit);
                        Player.player.input.sendRaycast(ri);
                    }
                    
                    Bullets.Clear();
                }
            }
            else
				OverrideUtilities.CallOriginal(PlayerUse);
		}

        public static EPlayerHit CalcHitMarker(ItemGunAsset PAsset, ref RaycastInfo ri)
        {
            EPlayerHit eplayerhit = EPlayerHit.NONE;
            if (ri.animal || ri.player || ri.zombie)
            {
                eplayerhit = EPlayerHit.ENTITIY;
                if (ri.limb == ELimb.SKULL)
                    eplayerhit = EPlayerHit.CRITICAL;
            }
            else if (ri.transform)
            {
                if (ri.transform.CompareTag("Barricade") && PAsset.barricadeDamage > 1f)
                {
                    InteractableDoorHinge component = ri.transform.GetComponent<InteractableDoorHinge>();
                    if (component != null)
						ri.transform = component.transform.parent.parent;

	                if (!ushort.TryParse(ri.transform.name, out ushort id)) return eplayerhit;
	                
	                ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset)Assets.find(EAssetType.ITEM, id);
	                
	                if (itemBarricadeAsset == null || !itemBarricadeAsset.isVulnerable && !PAsset.isInvulnerable)
		                return eplayerhit;
	                
	                if (eplayerhit == EPlayerHit.NONE)
		                eplayerhit = EPlayerHit.BUILD;
                }
                else if (ri.transform.CompareTag("Structure") && PAsset.structureDamage > 1f)
                {
	                if (!ushort.TryParse(ri.transform.name, out ushort id2)) return eplayerhit;
	                
	                ItemStructureAsset itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, id2);

	                if (itemStructureAsset == null || !itemStructureAsset.isVulnerable && !PAsset.isInvulnerable)
		                return eplayerhit;
	                
	                if (eplayerhit == EPlayerHit.NONE)
		                eplayerhit = EPlayerHit.BUILD;
                }
                else if (ri.transform.CompareTag("Resource") && PAsset.resourceDamage > 1f)
                {
	                if (!ResourceManager.tryGetRegion(ri.transform, out byte x, out byte y, out ushort index))
		                return eplayerhit;
	                
	                ResourceSpawnpoint resourceSpawnpoint = ResourceManager.getResourceSpawnpoint(x, y, index);

	                if (resourceSpawnpoint == null || resourceSpawnpoint.isDead ||
	                    resourceSpawnpoint.asset.bladeID != PAsset.bladeID) return eplayerhit;
	                
	                if (eplayerhit == EPlayerHit.NONE)
		                eplayerhit = EPlayerHit.BUILD;
                }
                else if (PAsset.objectDamage > 1f)
                {
                    InteractableObjectRubble component2 = ri.transform.GetComponent<InteractableObjectRubble>();
	                
	                if (component2 == null) return eplayerhit;
	                
	                ri.section = component2.getSection(ri.collider.transform);

	                if (component2.isSectionDead(ri.section) ||
	                    !component2.asset.rubbleIsVulnerable && !PAsset.isInvulnerable) return eplayerhit;
	                
	                if (eplayerhit == EPlayerHit.NONE)
		                eplayerhit = EPlayerHit.BUILD;
                }
            }
            else if (ri.vehicle && !ri.vehicle.isDead && PAsset.vehicleDamage > 1f)
				if (ri.vehicle.asset != null && (ri.vehicle.asset.isVulnerable || PAsset.isInvulnerable))
					if (eplayerhit == EPlayerHit.NONE)
						eplayerhit = EPlayerHit.BUILD;

			return eplayerhit;
        }
    }
}
