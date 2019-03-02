using System;
using System.Collections.Generic;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Thanking.Components.Basic;
using Thanking.Options.AimOptions;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Utilities
{
    public static class RaycastUtilities
    {
        public static HashSet<GameObject> Objects = new HashSet<GameObject>();
        public static List<GameObject> AttachedObjects = new List<GameObject>();
        public static Player TargetedPlayer;

        public static RaycastInfo GenerateOriginalRaycast(Ray ray, float range, int mask)
        {
            PhysicsUtility.raycast(ray, out RaycastHit hit, range, mask, QueryTriggerInteraction.UseGlobal);
            RaycastInfo raycastInfo = new RaycastInfo(hit) { direction = ray.direction };

            if (hit.transform == null) return raycastInfo;

            if (raycastInfo.transform.CompareTag("Barricade"))
                raycastInfo.transform = DamageTool.getBarricadeRootTransform(raycastInfo.transform);

            else if (raycastInfo.transform.CompareTag("Structure"))
                raycastInfo.transform = DamageTool.getStructureRootTransform(raycastInfo.transform);

            if (raycastInfo.transform.CompareTag("Enemy"))
                raycastInfo.player = DamageTool.getPlayer(raycastInfo.transform);

            if (raycastInfo.transform.CompareTag("Zombie"))
                raycastInfo.zombie = DamageTool.getZombie(raycastInfo.transform);

            if (raycastInfo.transform.CompareTag("Animal"))
                raycastInfo.animal = DamageTool.getAnimal(raycastInfo.transform);

            raycastInfo.limb = DamageTool.getLimb(raycastInfo.transform);

            if (RaycastOptions.UseCustomLimb)
                raycastInfo.limb = RaycastOptions.TargetLimb;

            else if (RaycastOptions.UseRandomLimb)
            {
                ELimb[] Limbs = (ELimb[])Enum.GetValues(typeof(ELimb));
                raycastInfo.limb = Limbs[MathUtilities.Random.Next(0, Limbs.Length)];
            }

            if (raycastInfo.transform.CompareTag("Vehicle"))
                raycastInfo.vehicle = DamageTool.getVehicle(raycastInfo.transform);

            if (raycastInfo.zombie != null && raycastInfo.zombie.isRadioactive)
                raycastInfo.material = EPhysicsMaterial.ALIEN_DYNAMIC;
            else
                raycastInfo.material = DamageTool.getMaterial(hit.point, raycastInfo.transform, raycastInfo.collider);

            return raycastInfo;
        }

        public static bool GenerateRaycast(out RaycastInfo info)
        {
            ItemGunAsset currentGun = OptimizationVariables.MainPlayer.equipment.asset as ItemGunAsset;
            
            float Range = currentGun?.range ?? 15.5f;

            info = GenerateOriginalRaycast(new Ray(OptimizationVariables.MainPlayer.look.aim.position, OptimizationVariables.MainPlayer.look.aim.forward), Range,
                RayMasks.DAMAGE_CLIENT);
            
            if (RaycastOptions.EnablePlayerSelection && TargetedPlayer != null)
            {
                GameObject p = TargetedPlayer.gameObject;
                bool shouldFire = true;

                Vector3 aimPos = OptimizationVariables.MainPlayer.look.aim.position;

                if (Vector3.Distance(aimPos, p.transform.position) > Range)
                    shouldFire = false;

                if (!SphereUtilities.GetRaycast(p, aimPos, out Vector3 point))
                    shouldFire = false;

                if (shouldFire)
                {
                    info = GenerateRaycast(p, point, info.collider);
                    return true;
                }
                
                if (RaycastOptions.OnlyShootAtSelectedPlayer)
                    return false;
            }

            if (!GetTargetObject(Objects, out GameObject Object, out Vector3 Point, Range))
                return false;

            info = GenerateRaycast(Object, Point, info.collider);
            return true;
        }

        public static RaycastInfo GenerateRaycast(GameObject Object, Vector3 Point, Collider col)
        {
            ELimb Limb = RaycastOptions.TargetLimb;

            if (RaycastOptions.UseRandomLimb)
            {
                ELimb[] Limbs = (ELimb[])Enum.GetValues(typeof(ELimb));
                Limb = Limbs[MathUtilities.Random.Next(0, Limbs.Length)];
            }

            EPhysicsMaterial mat = col == null ? EPhysicsMaterial.NONE : DamageTool.getMaterial(Point, Object.transform, col);

            if (RaycastOptions.UseTargetMaterial)
                mat = RaycastOptions.TargetMaterial;

            return new RaycastInfo(Object.transform)
            {
                point = Point,
                direction = RaycastOptions.UseModifiedVector ? RaycastOptions.TargetRagdoll.ToVector() : OptimizationVariables.MainPlayer.look.aim.forward,
                limb = Limb,
                material = mat,
                player = Object.GetComponent<Player>(),
                zombie = Object.GetComponent<Zombie>(),
                vehicle = Object.GetComponent<InteractableVehicle>()
            };
        }
        
	    public static bool GetTargetObject(HashSet<GameObject> Objects, out GameObject Object, out Vector3 Point, float Range)
        {
            double Distance = Range + 1;
            double FOV = 180;
            
            Object = null;
            Point = Vector3.zero;

            Vector3 AimPos = OptimizationVariables.MainPlayer.look.aim.position;
            Vector3 AimForward = OptimizationVariables.MainPlayer.look.aim.forward;
            
            foreach (GameObject go in Objects)
            {
                if (go == null)
                    continue;

                Vector3 TargetPos = go.transform.position;

                Player p = go.GetComponent<Player>();
                if (p && (p.life.isDead || FriendUtilities.IsFriendly(p)))
                    continue;
                
                Zombie z = go.GetComponent<Zombie>();
                if (z && z.isDead)
                    continue;

                RaycastComponent Component = go.GetComponent<RaycastComponent>();

                if (Component == null)
                {
                    go.AddComponent<RaycastComponent>();
                    continue;
                }

                double NewDistance = VectorUtilities.GetDistance(AimPos, TargetPos);

                if (NewDistance > Range)
                    continue;
                
                if (RaycastOptions.SilentAimUseFOV)
                {
                    double _FOV = VectorUtilities.GetAngleDelta(AimPos, AimForward, TargetPos);
                    if (_FOV > RaycastOptions.SilentAimFOV)
                        continue;

                    if (_FOV > FOV)
                        continue;

                    FOV = _FOV;
                }
                
                else if (NewDistance > Distance)
                    continue;

                if (!SphereUtilities.GetRaycast(go, AimPos, out Vector3 _Point))
                    continue;

                Object = go;
                Distance = NewDistance;
                Point = _Point;
            }

            return Object != null;
        }
    }
}
