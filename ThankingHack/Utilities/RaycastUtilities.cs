using System;
using System.Linq;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Thanking.Options.AimOptions;
using Thanking.Utilities.Mesh_Utilities;
using UnityEngine;
using Thanking.Options;
using System.Collections.Generic;
using Thanking.Components.Basic;
using Thanking.Variables;
using Thanking.Coroutines;
using UnityEngine.PostProcessing;

namespace Thanking.Utilities
{
    public static class RaycastUtilities
    {
        public static GameObject[] Objects = new GameObject[0];
        public static List<GameObject> AttachedObjects = new List<GameObject>();
        public static Player TargetedPlayer;

        public static RaycastInfo GenerateOriginalRaycast(Ray ray, float range, int mask)
        {
            PhysicsUtility.raycast(ray, out RaycastHit hit, range, mask);
            RaycastInfo raycastInfo = new RaycastInfo(hit) { direction = ray.direction };

            if (hit.transform == null) return raycastInfo;

            if (hit.transform.CompareTag("Enemy"))
                raycastInfo.player = DamageTool.getPlayer(raycastInfo.transform);

            if (hit.transform.CompareTag("Zombie"))
                raycastInfo.zombie = DamageTool.getZombie(raycastInfo.transform);

            if (hit.transform.CompareTag("Animal"))
                raycastInfo.animal = DamageTool.getAnimal(raycastInfo.transform);

            raycastInfo.limb = DamageTool.getLimb(raycastInfo.transform);

            if (RaycastOptions.UseCustomLimb)
                raycastInfo.limb = RaycastOptions.TargetLimb;

            else if (RaycastOptions.UseRandomLimb)
            {
                ELimb[] Limbs = (ELimb[])Enum.GetValues(typeof(ELimb));
                raycastInfo.limb = Limbs[MathUtilities.Random.Next(0, Limbs.Length)];
            }

            if (hit.transform.CompareTag("Vehicle"))
                raycastInfo.vehicle = DamageTool.getVehicle(raycastInfo.transform);

            if (RaycastOptions.UseTargetMaterial && RaycastOptions.TargetMaterial != EPhysicsMaterial.NONE)
                raycastInfo.material = RaycastOptions.TargetMaterial;

            else if (raycastInfo.zombie != null && raycastInfo.zombie.isRadioactive)
                raycastInfo.material = EPhysicsMaterial.ALIEN_DYNAMIC;

            else
                raycastInfo.material = DamageTool.getMaterial(hit.point, hit.transform, hit.collider);

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
        
	    public static bool GetTargetObject(GameObject[] Objects, out GameObject Object, out Vector3 Point, float Range)
        {
            double Distance = Range + 1;
            double FOV = 180;
            
            Object = null;
            Point = Vector3.zero;

            Vector3 AimPos = OptimizationVariables.MainPlayer.look.aim.position;
            Vector3 AimForward = OptimizationVariables.MainPlayer.look.aim.forward;
            
            for (int i = 0; i < Objects.Length; i++)
            {
                GameObject go = Objects[i];

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
