using SDG.Unturned;
using System.Collections.Generic;
using Thanking.Attributes;
using Thanking.Utilities;
using UnityEngine;
using Thanking.Options.AimOptions;
using Thanking.Variables;
using static Thanking.Utilities.ReflectionUtilities;

namespace Thanking.Overrides
{
    public class OV_UseableGun : Useable
    {
        private List<BulletInfo> bullets;
        private int hitmarkerIndex;

        [Override(typeof(UseableGun), "ballistics", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)]
        public void OV_ballistics()
        {
            UseableGun UG = (UseableGun)player.equipment.useable;
            bullets = UG.GetField<List<BulletInfo>>("bullets", FieldType.Private);
            //(List<BulletInfo>)typeof(UseableGun).GetField("bullets", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Player.player.equipment.useable);
            if (((ItemGunAsset)base.player.equipment.asset).projectile != null || this.bullets == null)
                return;

            if (true)
            {
                if (base.channel.isOwner)
                {
                    for (int i = 0; i < this.bullets.Count; i++)
                    {
                        BulletInfo bulletInfo = this.bullets[i];
                        byte pellets = bulletInfo.magazineAsset.pellets;
                        if (base.channel.isOwner)
                        {
                            EPlayerHit eplayerHit = EPlayerHit.NONE;
                            if (pellets > 1)
                            {
                                this.hitmarkerIndex = (int)bulletInfo.pellet;
                            }
                            else if (OptionsSettings.hitmarker)
                            {
                                this.hitmarkerIndex++;
                                if (this.hitmarkerIndex >= PlayerLifeUI.hitmarkers.Length)
                                {
                                    this.hitmarkerIndex = 0;
                                }
                            }
                            else
                            {
                                this.hitmarkerIndex = 0;
                            }
                            Ray ray = new Ray(bulletInfo.pos, bulletInfo.dir);
                            RaycastInfo raycastInfo = DamageTool.raycast(ray, (!Provider.modeConfigData.Gameplay.Ballistics) ? ((ItemGunAsset)base.player.equipment.asset).range : ((ItemGunAsset)base.player.equipment.asset).ballisticTravel, RayMasks.DAMAGE_CLIENT);
                            if (RaycastOptions.Enabled) raycastInfo = RaycastUtilities.GenerateRaycast();
                            if (raycastInfo.player != null && ((ItemGunAsset)base.player.equipment.asset).playerDamageMultiplier.damage > 1f && !base.player.quests.isMemberOfSameGroupAs(raycastInfo.player) && Provider.isPvP)
                            {
                                if (eplayerHit != EPlayerHit.CRITICAL)
                                {
                                    eplayerHit = ((raycastInfo.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
                                }
                                PlayerUI.hitmark(this.hitmarkerIndex, raycastInfo.point, pellets > 1, (raycastInfo.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
                            }
                            else if ((raycastInfo.zombie != null && ((ItemGunAsset)base.player.equipment.asset).zombieDamageMultiplier.damage > 1f) || (raycastInfo.animal != null && ((ItemGunAsset)base.player.equipment.asset).animalDamageMultiplier.damage > 1f))
                            {
                                if (eplayerHit != EPlayerHit.CRITICAL)
                                {
                                    eplayerHit = ((raycastInfo.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
                                }
                                PlayerUI.hitmark(this.hitmarkerIndex, raycastInfo.point, pellets > 1, (raycastInfo.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
                            }
                            else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Barricade") && ((ItemGunAsset)base.player.equipment.asset).barricadeDamage > 1f)
                            {
                                InteractableDoorHinge component = raycastInfo.transform.GetComponent<InteractableDoorHinge>();
                                if (component != null)
                                {
                                    raycastInfo.transform = component.transform.parent.parent;
                                }
                                ushort id;
                                if (ushort.TryParse(raycastInfo.transform.name, out id))
                                {
                                    ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset)Assets.find(EAssetType.ITEM, id);
                                    if (itemBarricadeAsset != null && (itemBarricadeAsset.isVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
                                    {
                                        if (eplayerHit == EPlayerHit.NONE)
                                        {
                                            eplayerHit = EPlayerHit.BUILD;
                                        }
                                        PlayerUI.hitmark(this.hitmarkerIndex, raycastInfo.point, pellets > 1, EPlayerHit.BUILD);
                                    }
                                }
                            }
                            else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Structure") && ((ItemGunAsset)base.player.equipment.asset).structureDamage > 1f)
                            {
                                ushort id2;
                                if (ushort.TryParse(raycastInfo.transform.name, out id2))
                                {
                                    ItemStructureAsset itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, id2);
                                    if (itemStructureAsset != null && (itemStructureAsset.isVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
                                    {
                                        if (eplayerHit == EPlayerHit.NONE)
                                        {
                                            eplayerHit = EPlayerHit.BUILD;
                                        }
                                        PlayerUI.hitmark(this.hitmarkerIndex, raycastInfo.point, pellets > 1, EPlayerHit.BUILD);
                                    }
                                }
                            }
                            else if (raycastInfo.vehicle != null && !raycastInfo.vehicle.isDead && ((ItemGunAsset)base.player.equipment.asset).vehicleDamage > 1f)
                            {
                                if (raycastInfo.vehicle.asset != null && (raycastInfo.vehicle.asset.isVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
                                {
                                    if (eplayerHit == EPlayerHit.NONE)
                                    {
                                        eplayerHit = EPlayerHit.BUILD;
                                    }
                                    PlayerUI.hitmark(this.hitmarkerIndex, raycastInfo.point, pellets > 1, EPlayerHit.BUILD);
                                }
                            }
                            else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Resource") && ((ItemGunAsset)base.player.equipment.asset).resourceDamage > 1f)
                            {
                                byte x;
                                byte y;
                                ushort index;
                                if (ResourceManager.tryGetRegion(raycastInfo.transform, out x, out y, out index))
                                {
                                    ResourceSpawnpoint resourceSpawnpoint = ResourceManager.getResourceSpawnpoint(x, y, index);
                                    if (resourceSpawnpoint != null && !resourceSpawnpoint.isDead && resourceSpawnpoint.asset.bladeID == ((ItemWeaponAsset)base.player.equipment.asset).bladeID)
                                    {
                                        if (eplayerHit == EPlayerHit.NONE)
                                        {
                                            eplayerHit = EPlayerHit.BUILD;
                                        }
                                        PlayerUI.hitmark(this.hitmarkerIndex, raycastInfo.point, pellets > 1, EPlayerHit.BUILD);
                                    }
                                }
                            }
                            else if (raycastInfo.transform != null && ((ItemGunAsset)base.player.equipment.asset).objectDamage > 1f)
                            {
                                InteractableObjectRubble component2 = raycastInfo.transform.GetComponent<InteractableObjectRubble>();
                                if (component2 != null)
                                {
                                    raycastInfo.section = component2.getSection(raycastInfo.collider.transform);
                                    if (!component2.isSectionDead(raycastInfo.section) && (component2.asset.rubbleIsVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
                                    {
                                        if (eplayerHit == EPlayerHit.NONE)
                                        {
                                            eplayerHit = EPlayerHit.BUILD;
                                        }
                                        PlayerUI.hitmark(this.hitmarkerIndex, raycastInfo.point, pellets > 1, EPlayerHit.BUILD);
                                    }
                                }
                            }
                            if (Provider.modeConfigData.Gameplay.Ballistics)
                            {
                                if (bulletInfo.steps > 0 || ((ItemGunAsset)base.player.equipment.asset).ballisticSteps <= 1)
                                {
                                    if (((ItemGunAsset)base.player.equipment.asset).ballisticTravel < 32f)
                                    {
                                        this.GetPrivateFunction("trace").Invoke(this, new object[] { bulletInfo.pos + bulletInfo.dir * 32f, bulletInfo.dir });
                                    }
                                    else
                                    {
                                        this.GetPrivateFunction("trace").Invoke(this, new object[] { bulletInfo.pos + bulletInfo.dir * UnityEngine.Random.Range(32f, ((ItemGunAsset)base.player.equipment.asset).ballisticTravel), bulletInfo.dir });
                                    }
                                }
                            }
                            else if (((ItemGunAsset)base.player.equipment.asset).range < 32f)
                            {
                                this.GetPrivateFunction("trace").Invoke(this, new object[] { ray.origin + ray.direction * 32f, ray.direction });
                            }
                            else
                            {
                                this.GetPrivateFunction("trace").Invoke(this, new object[] { ray.origin + ray.direction * UnityEngine.Random.Range(32f, Mathf.Min(64f, ((ItemGunAsset)base.player.equipment.asset).range)), ray.direction });
                            }
                            if (base.player.input.isRaycastInvalid(raycastInfo))
                            {
                                float num = ((ItemGunAsset)base.player.equipment.asset).ballisticDrop;
                                if (bulletInfo.barrelAsset != null)
                                {
                                    num *= bulletInfo.barrelAsset.ballisticDrop;
                                }
                                bulletInfo.pos += bulletInfo.dir * ((ItemGunAsset)base.player.equipment.asset).ballisticTravel;
                                BulletInfo bulletInfo2 = bulletInfo;
                                bulletInfo2.dir.y = bulletInfo2.dir.y - num;
                                bulletInfo.dir.Normalize();
                            }
                            else
                            {
                                if (eplayerHit != EPlayerHit.NONE)
                                {
                                    int num2;
                                    if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num2))
                                    {
                                        Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", num2 + 1);
                                    }
                                    if (eplayerHit == EPlayerHit.CRITICAL && Provider.provider.statisticsService.userStatisticsService.getStatistic("Headshots", out num2))
                                    {
                                        Provider.provider.statisticsService.userStatisticsService.setStatistic("Headshots", num2 + 1);
                                    }
                                }
                                base.player.input.sendRaycast(raycastInfo);
                                bulletInfo.steps = 254;
                            }
                        }
                    }
                }
                if (Provider.isServer)
                {
                    while (this.bullets.Count > 0)
                    {
                        BulletInfo bulletInfo3 = this.bullets[0];
                        byte pellets2 = bulletInfo3.magazineAsset.pellets;
                        if (!base.player.input.hasInputs())
                        {
                            break;
                        }
                        InputInfo input = base.player.input.getInput(true);
                        if (input == null)
                        {
                            break;
                        }
                        if (!base.channel.isOwner)
                        {
                            if (Provider.modeConfigData.Gameplay.Ballistics)
                            {
                                if ((input.point - bulletInfo3.pos).magnitude > ((ItemGunAsset)base.player.equipment.asset).ballisticTravel * (float)((long)(bulletInfo3.steps + 1) + (long)((ulong)PlayerInput.SAMPLES)) + 4f)
                                {
                                    this.bullets.RemoveAt(0);
                                    continue;
                                }
                            }
                            else if ((input.point - base.player.look.aim.position).sqrMagnitude > Mathf.Pow(((ItemGunAsset)base.player.equipment.asset).range + 4f, 2f))
                            {
                                break;
                            }
                        }
                        if (input.material != EPhysicsMaterial.NONE)
                        {
                            if (bulletInfo3.magazineAsset != null && bulletInfo3.magazineAsset.impact != 0)
                            {
                                DamageTool.impact(input.point, input.normal, bulletInfo3.magazineAsset.impact, base.channel.owner.playerID.steamID, base.transform.position);
                            }
                            else
                            {
                                DamageTool.impact(input.point, input.normal, input.material, input.type != ERaycastInfoType.NONE && input.type != ERaycastInfoType.OBJECT, base.channel.owner.playerID.steamID, base.transform.position);
                            }
                        }
                        EPlayerKill eplayerKill = EPlayerKill.NONE;
                        uint num3 = 0u;
                        float num4 = 1f;
                        num4 *= ((bulletInfo3.quality >= 0.5f) ? 1f : (0.5f + bulletInfo3.quality));
                        if (input.type == ERaycastInfoType.PLAYER)
                        {
                            if (input.player != null && !base.player.quests.isMemberOfSameGroupAs(input.player) && Provider.isPvP)
                            {
                                DamageTool.damage(input.player, EDeathCause.GUN, input.limb, base.channel.owner.playerID.steamID, input.direction * Mathf.Ceil((float)pellets2 / 2f), ((ItemGunAsset)base.player.equipment.asset).playerDamageMultiplier, num4, true, out eplayerKill);
                            }
                        }
                        else if (input.type == ERaycastInfoType.ZOMBIE)
                        {
                            if (input.zombie != null)
                            {
                                DamageTool.damage(input.zombie, input.limb, input.direction * Mathf.Ceil((float)pellets2 / 2f), ((ItemGunAsset)base.player.equipment.asset).zombieDamageMultiplier, num4, true, out eplayerKill, out num3);
                                if (base.player.movement.nav != 255)
                                {
                                    input.zombie.alert(base.transform.position, true);
                                }
                            }
                        }
                        else if (input.type == ERaycastInfoType.ANIMAL)
                        {
                            if (input.animal != null)
                            {
                                DamageTool.damage(input.animal, input.limb, input.direction * Mathf.Ceil((float)pellets2 / 2f), ((ItemGunAsset)base.player.equipment.asset).animalDamageMultiplier, num4, out eplayerKill, out num3);
                                input.animal.alertPoint(base.transform.position, true);
                            }
                        }
                        else if (input.type == ERaycastInfoType.VEHICLE)
                        {
                            if (input.vehicle != null && input.vehicle.asset != null && (input.vehicle.asset.isVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
                            {
                                DamageTool.damage(input.vehicle, true, input.point, false, ((ItemGunAsset)base.player.equipment.asset).vehicleDamage, num4, true, out eplayerKill);
                            }
                        }
                        else if (input.type == ERaycastInfoType.BARRICADE)
                        {
                            ushort id3;
                            if (input.transform != null && input.transform.CompareTag("Barricade") && ushort.TryParse(input.transform.name, out id3))
                            {
                                ItemBarricadeAsset itemBarricadeAsset2 = (ItemBarricadeAsset)Assets.find(EAssetType.ITEM, id3);
                                if (itemBarricadeAsset2 != null && (itemBarricadeAsset2.isVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
                                {
                                    DamageTool.damage(input.transform, false, ((ItemGunAsset)base.player.equipment.asset).barricadeDamage, num4, out eplayerKill);
                                }
                            }
                        }
                        else if (input.type == ERaycastInfoType.STRUCTURE)
                        {
                            ushort id4;
                            if (input.transform != null && input.transform.CompareTag("Structure") && ushort.TryParse(input.transform.name, out id4))
                            {
                                ItemStructureAsset itemStructureAsset2 = (ItemStructureAsset)Assets.find(EAssetType.ITEM, id4);
                                if (itemStructureAsset2 != null && (itemStructureAsset2.isVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
                                {
                                    DamageTool.damage(input.transform, false, input.direction * Mathf.Ceil((float)pellets2 / 2f), ((ItemGunAsset)base.player.equipment.asset).structureDamage, num4, out eplayerKill);
                                }
                            }
                        }
                        else if (input.type == ERaycastInfoType.RESOURCE)
                        {
                            byte x2;
                            byte y2;
                            ushort index2;
                            if (input.transform != null && input.transform.CompareTag("Resource") && ResourceManager.tryGetRegion(input.transform, out x2, out y2, out index2))
                            {
                                ResourceSpawnpoint resourceSpawnpoint2 = ResourceManager.getResourceSpawnpoint(x2, y2, index2);
                                if (resourceSpawnpoint2 != null && !resourceSpawnpoint2.isDead && resourceSpawnpoint2.asset.bladeID == ((ItemWeaponAsset)base.player.equipment.asset).bladeID)
                                {
                                    DamageTool.damage(input.transform, input.direction * Mathf.Ceil((float)pellets2 / 2f), ((ItemGunAsset)base.player.equipment.asset).resourceDamage, num4, 1f, out eplayerKill, out num3);
                                }
                            }
                        }
                        else if (input.type == ERaycastInfoType.OBJECT && input.transform != null && input.section < 255)
                        {
                            InteractableObjectRubble component3 = input.transform.GetComponent<InteractableObjectRubble>();
                            if (component3 != null && !component3.isSectionDead(input.section) && (component3.asset.rubbleIsVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
                            {
                                DamageTool.damage(input.transform, input.direction, input.section, ((ItemGunAsset)base.player.equipment.asset).objectDamage, num4, out eplayerKill, out num3);
                            }
                        }
                        if (input.type != ERaycastInfoType.PLAYER && input.type != ERaycastInfoType.ZOMBIE && input.type != ERaycastInfoType.ANIMAL && !base.player.life.isAggressor)
                        {
                            float num5 = ((ItemGunAsset)base.player.equipment.asset).range + Provider.modeConfigData.Players.Ray_Aggressor_Distance;
                            num5 *= num5;
                            float num6 = Provider.modeConfigData.Players.Ray_Aggressor_Distance;
                            num6 *= num6;
                            Vector3 normalized = (bulletInfo3.pos - base.player.look.aim.position).normalized;
                            for (int j = 0; j < Provider.clients.Count; j++)
                            {
                                if (Provider.clients[j] != base.channel.owner)
                                {
                                    Player player = Provider.clients[j].player;
                                    if (!(player == null))
                                    {
                                        Vector3 vector = player.look.aim.position - base.player.look.aim.position;
                                        Vector3 a = Vector3.Project(vector, normalized);
                                        if (a.sqrMagnitude < num5 && (a - vector).sqrMagnitude < num6)
                                        {
                                            base.player.life.markAggressive(false, true);
                                        }
                                    }
                                }
                            }
                        }
                        if (Level.info.type == ELevelType.HORDE)
                        {
                            if (input.zombie != null)
                            {
                                if (input.limb == ELimb.SKULL)
                                {
                                    base.player.skills.askPay(10u);
                                }
                                else
                                {
                                    base.player.skills.askPay(5u);
                                }
                            }
                            if (eplayerKill == EPlayerKill.ZOMBIE)
                            {
                                if (input.limb == ELimb.SKULL)
                                {
                                    base.player.skills.askPay(50u);
                                }
                                else
                                {
                                    base.player.skills.askPay(25u);
                                }
                            }
                        }
                        else
                        {
                            if (eplayerKill == EPlayerKill.PLAYER)
                            {
                                base.player.sendStat(EPlayerStat.KILLS_PLAYERS);
                                if (Level.info.type == ELevelType.ARENA)
                                {
                                    base.player.skills.askPay(100u);
                                }
                            }
                            else if (eplayerKill == EPlayerKill.ZOMBIE)
                            {
                                base.player.sendStat(EPlayerStat.KILLS_ZOMBIES_NORMAL);
                            }
                            else if (eplayerKill == EPlayerKill.MEGA)
                            {
                                base.player.sendStat(EPlayerStat.KILLS_ZOMBIES_MEGA);
                            }
                            else if (eplayerKill == EPlayerKill.ANIMAL)
                            {
                                base.player.sendStat(EPlayerStat.KILLS_ANIMALS);
                            }
                            else if (eplayerKill == EPlayerKill.RESOURCE)
                            {
                                base.player.sendStat(EPlayerStat.FOUND_RESOURCES);
                            }
                            if (num3 > 0u)
                            {
                                base.player.skills.askPay(num3);
                            }
                        }
                        Vector3 point = input.point + input.normal * 0.25f;
                        if (bulletInfo3.magazineAsset != null && bulletInfo3.magazineAsset.isExplosive)
                        {
                            EffectManager.sendEffect(bulletInfo3.magazineAsset.explosion, EffectManager.MEDIUM, point);
                            DamageTool.explode(point, bulletInfo3.magazineAsset.range, EDeathCause.SPLASH, base.channel.owner.playerID.steamID, bulletInfo3.magazineAsset.playerDamage, bulletInfo3.magazineAsset.zombieDamage, bulletInfo3.magazineAsset.animalDamage, bulletInfo3.magazineAsset.barricadeDamage, bulletInfo3.magazineAsset.structureDamage, bulletInfo3.magazineAsset.vehicleDamage, bulletInfo3.magazineAsset.resourceDamage, bulletInfo3.magazineAsset.objectDamage, EExplosionDamageType.CONVENTIONAL, 32f, true, false);
                        }
                        if (bulletInfo3.dropID != 0)
                        {
                            ItemManager.dropItem(new Item(bulletInfo3.dropID, bulletInfo3.dropAmount, bulletInfo3.dropQuality), point, false, Dedicator.isDedicated, false);
                        }
                        this.bullets.RemoveAt(0);
                    }
                }
            }
        }
    }
}
