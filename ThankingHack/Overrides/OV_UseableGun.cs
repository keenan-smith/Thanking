using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Components.Basic;
using Thanking.Coroutines;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Overrides
{
    public class OV_UseableGun
    {
        private static FieldInfo BulletsField;
        
        [Initializer]
        public static void Load()
        {
            BulletsField = typeof(UseableGun).GetField("bullets", ReflectionVariables.PrivateInstance);
        }

        [Override(typeof(UseableGun), "ballistics", BindingFlags.NonPublic | BindingFlags.Instance)]
        public void OV_ballistics()
        {
            if (Time.realtimeSinceStartup - PlayerLifeUI.hitmarkers[0].lastHit > PlayerUI.HIT_TIME)
            {
                PlayerLifeUI.hitmarkers[0].hitBuildImage.isVisible = false;
                PlayerLifeUI.hitmarkers[0].hitCriticalImage.isVisible = false;
                PlayerLifeUI.hitmarkers[0].hitEntitiyImage.isVisible = false;
            }

            Useable PlayerUse = OptimizationVariables.MainPlayer.equipment.useable;
            PlayerLook Look = OptimizationVariables.MainPlayer.look;

            ItemGunAsset PAsset = (ItemGunAsset)OptimizationVariables.MainPlayer.equipment.asset;

            if (PAsset.projectile != null)
                return;

            List<BulletInfo> Bullets = (List<BulletInfo>)BulletsField.GetValue(PlayerUse);

            if (Bullets.Count == 0)
                return;

            RaycastInfo ri = null;
            
            if (RaycastOptions.Enabled)
            {
                RaycastUtilities.GetPlayers();
                RaycastUtilities.GenerateRaycast(out ri);
            }

            if (Provider.modeConfigData.Gameplay.Ballistics)
            {
                if (ri == null)
                {
                    if (AimbotOptions.NoAimbotDrop)
                    {
                        if (AimbotCoroutines.IsAiming && AimbotCoroutines.LockedObject != null)
                        {
                            Vector3 AimPos =
                                AimbotCoroutines.GetAimPosition(AimbotCoroutines.LockedObject.transform, "Skull");
                            Ray AimRay = GetAimRay(Look.aim.position, AimPos);

                            float Dist = (float) VectorUtilities.GetDistance(Look.aim.position, AimPos);

                            if (!Physics.Raycast(AimRay, out RaycastHit hit, Dist, RayMasks.DAMAGE_SERVER))
                                ri = RaycastUtilities.GenerateOriginalRaycast(AimRay, PAsset.range, RayMasks.ENEMY);
                        }
                    }

                    if (WeaponOptions.NoDrop && ri == null)
                    {
                        for (int i = 0; i < Bullets.Count; i++)
                        {
                            BulletInfo bulletInfo = Bullets[i];
                            Ray ray = new Ray(bulletInfo.pos, bulletInfo.dir);
                            RaycastInfo rayInfo =
                                DamageTool.raycast(ray, PAsset.ballisticTravel, RayMasks.DAMAGE_CLIENT);

                            if (Player.player.input.isRaycastInvalid(rayInfo))
                                bulletInfo.pos += bulletInfo.dir * PAsset.ballisticTravel;

                            else
                            {
                                EPlayerHit playerHit = CalcHitMarker(PAsset, ref rayInfo);
                                PlayerUI.hitmark(0, rayInfo.point, false, playerHit);

                                OptimizationVariables.MainPlayer.input.sendRaycast(rayInfo);
                                bulletInfo.steps = 254;
                            }
                        }

                        for (int i = Bullets.Count - 1; i >= 0; i--)
                        {
                            BulletInfo bulletInfo = Bullets[i];
                            bulletInfo.steps += 1;

                            if (bulletInfo.steps >= PAsset.ballisticSteps)
                                Bullets.RemoveAt(i);
                        }

                        return;
                    }

                    if (ri == null)
                    {
                        OverrideUtilities.CallOriginal(PlayerUse);
                        return;
                    }
                }
                for (int i = 0; i < Bullets.Count; i++)
                {
                    BulletInfo bulletInfo = Bullets[i];
                    double distance = VectorUtilities.GetDistance(OptimizationVariables.MainPlayer.transform.position, ri.point);

                    if (bulletInfo.steps * PAsset.ballisticTravel < distance)
                        continue;

                    EPlayerHit eplayerhit = CalcHitMarker(PAsset, ref ri);
                    PlayerUI.hitmark(0, Vector3.zero, false, eplayerhit);
                    OptimizationVariables.MainPlayer.input.sendRaycast(ri);
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
                if (ri != null)
                {
                    for (int i = 0; i < Bullets.Count; i++)
                    {
                        EPlayerHit eplayerhit = CalcHitMarker(PAsset, ref ri);
                        PlayerUI.hitmark(0, Vector3.zero, false, eplayerhit);
                        OptimizationVariables.MainPlayer.input.sendRaycast(ri);
                    }
                    
                    Bullets.Clear();
                }
                else
                    OverrideUtilities.CallOriginal(PlayerUse);
            }
        }

        public static EPlayerHit CalcHitMarker(ItemGunAsset PAsset, ref RaycastInfo ri)
        {
            EPlayerHit eplayerhit = EPlayerHit.NONE;

            if (ri == null || PAsset == null)
            {
                return eplayerhit;
            }

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

                    ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset) Assets.find(EAssetType.ITEM, id);

                    if (itemBarricadeAsset == null || !itemBarricadeAsset.isVulnerable && !PAsset.isInvulnerable)
                        return eplayerhit;

                    if (eplayerhit == EPlayerHit.NONE)
                        eplayerhit = EPlayerHit.BUILD;
                }
                else if (ri.transform.CompareTag("Structure") && PAsset.structureDamage > 1f)
                {
                    if (!ushort.TryParse(ri.transform.name, out ushort id2)) return eplayerhit;

                    ItemStructureAsset itemStructureAsset = (ItemStructureAsset) Assets.find(EAssetType.ITEM, id2);

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

        public static Ray GetAimRay(Vector3 origin, Vector3 pos)
        {
            Vector3 normal = VectorUtilities.Normalize(pos - origin);
            return new Ray(pos, normal);
        }
    }
}
