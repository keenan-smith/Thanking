using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HighlightingSystem;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Components.Basic;
using Thanking.Coroutines;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using Thanking.Variables;
using Thanking.Variables.UIVariables;
using UnityEngine;

namespace Thanking.Components.UI
{
    [Component]
    public class TrajectoryComponent : MonoBehaviour
    {
        private static readonly FieldInfo thirdAttachmentsField = typeof(UseableGun).GetField("thirdAttachments", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo swingModeField = typeof(UseableThrowable).GetField("swingMode", BindingFlags.Instance | BindingFlags.NonPublic);
        private static Highlighter highlighted;
        public static Highlighter Highlighted
        {
            get => highlighted;
            private set
            {
                if (highlighted != null)
                    RemoveHighlight(highlighted);

                highlighted = value;
            }
        }
        public static HashSet<GameObject> BodiesInMotion { get; } = new HashSet<GameObject>();
        private static bool spying;

        private static Color InRangeColor { get => ColorUtilities.getColor("_TrajectoryPredictionInRange"); }
        private static Color OutOfRangeColor { get => ColorUtilities.getColor("_TrajectoryPredictionOutOfRange"); }

        [Initializer]
        public static void Initialize()
        {
            ColorUtilities.addColor(new ColorVariable("_TrajectoryPredictionInRange", "B.D. Predict (In Range)", Color.cyan));
            ColorUtilities.addColor(new ColorVariable("_TrajectoryPredictionOutOfRange", "B.D. Predict (Out of Range)", Color.red));
        }

        public void Start()
        {
            CoroutineComponent.TrajectoryCoroutine = StartCoroutine(TrajectoryCoroutines.UpdateBodiesInMotionSet());
        }

        private void HighlightForTrajectory(RaycastHit hit, Color color)
        {
            if (!WeaponOptions.HighlightBulletDropPredictionTarget)
            {
                Highlighted = null;
                return;
            }

            if (WeaponOptions.HighlightBulletDropPredictionTarget && hit.collider != null)
            {
                var t = hit.transform;

                GameObject go = null;

                if (DamageTool.getPlayer(t) != null)
                    go = DamageTool.getPlayer(t).gameObject;
                else if (DamageTool.getZombie(t) != null)
                    go = DamageTool.getZombie(t).gameObject;
                else if (DamageTool.getAnimal(t) != null)
                    go = DamageTool.getAnimal(t).gameObject;
                else if (DamageTool.getVehicle(t) != null)
                    go = DamageTool.getVehicle(t).gameObject;

                if (go != null)
                {
                    var newHighlight = go.GetComponent<Highlighter>() ?? go.AddComponent<Highlighter>();

                    if (!newHighlight.enabled)
                    {
                        //newHighlight.OccluderOn();
                        //newHighlight.SeeThroughOn();
                        newHighlight.occluder = true;
                        newHighlight.overlay = true;

                        newHighlight.ConstantOnImmediate(color);
                    }

                    Highlighted = newHighlight;
                    return;
                }
                Highlighted = null;
            }
        }

        public void OnGUI()
        {
            if (!DrawUtilities.ShouldRun() || spying || !WeaponOptions.EnableBulletDropPrediction)
            {
                Highlighted = null;
                return;
            }

            BodiesInMotion.RemoveWhere(x =>
            {
                if (x == null)
                    return true;

                var sticky = x.GetComponent<StickyGrenade>();
                if (sticky != null && sticky.GetComponent<Rigidbody>()?.useGravity == false)
                    return true;

                return false;
            });

            foreach (var body in BodiesInMotion)
            {
                if (body.GetComponent<Rigidbody>()?.velocity != Vector3.zero)
                    DrawTrajectory(PlotTrajectoryRigidBodyInMotion(body, 50 * 30), OutOfRangeColor);
            }

            Useable item = OptimizationVariables.MainPlayer?.equipment?.useable;

            if (item == null || ((item as UseableGun)?.equippedGunAsset?.action != EAction.Rocket && !Provider.modeConfigData.Gameplay.Ballistics))
            {
                Highlighted = null;
                return;
            }

            bool outOfRange;
            List<Vector3> trajectory;

            if (item is UseableGun gun)
            {
                var action = gun.equippedGunAsset.action;
                trajectory = action == EAction.Rocket ? PlotTrajectoryRocket(gun, out var hit, 50 * 30) : PlotTrajectoryGun(gun, out hit);
                outOfRange = action == EAction.Rocket ? false :
                    Vector3.Distance(trajectory.Last(), OptimizationVariables.MainPlayer.look.aim.position) > gun.equippedGunAsset.range;

                if (action != EAction.Rocket)
                    HighlightForTrajectory(hit, outOfRange ? OutOfRangeColor : InRangeColor);
            }
            else if (item is UseableThrowable grenade)
            {
                outOfRange = false;
                trajectory = PlotTrajectoryGrenade(grenade, 50 * (int)grenade.equippedThrowableAsset.fuseLength);
            }
            else
            {
                return;
            }

            DrawTrajectory(trajectory, outOfRange ? OutOfRangeColor : InRangeColor);
        }

        private static void DrawTrajectory(List<Vector3> trajectory, Color color)
        {
            ESPComponent.GLMat.SetPass(0);
            GL.PushMatrix();
            GL.LoadProjectionMatrix(OptimizationVariables.MainCam.projectionMatrix);
            GL.modelview = OptimizationVariables.MainCam.worldToCameraMatrix;
            GL.Begin(GL.LINE_STRIP);

            GL.Color(color);

            foreach (var x in trajectory)
                GL.Vertex(x);

            GL.End();
            GL.PopMatrix();
        }

        private static void RemoveHighlight(Highlighter h)
        {
            if (h == null)
                return;

            h.occluder = false;
            h.overlay = false;
            
            //h.OccluderOff();
            //h.SeeThroughOff();
            h.ConstantOffImmediate();
        }

        public static List<Vector3> PlotTrajectoryGrenade(UseableThrowable grenade, int maxSteps)
        {
            var pos = OptimizationVariables.MainPlayer.look.aim.position;
            var forceMultiplier = grenade.equippedThrowableAsset.strongThrowForce;

            if (OptimizationVariables.MainPlayer.skills.boost == EPlayerBoost.OLYMPIC)
                forceMultiplier *= grenade.equippedThrowableAsset.boostForceMultiplier;

            var force = OptimizationVariables.MainPlayer.look.aim.forward * forceMultiplier;
            var mass = grenade.equippedThrowableAsset.throwable.GetComponent<Rigidbody>().mass;
            var vel = (force / mass) * Time.fixedDeltaTime;

            var points = new List<Vector3>()
            {
                pos
            };

            if (!PhysicsUtility.raycast(new Ray(pos, OptimizationVariables.MainPlayer.look.aim.forward),
                out _, 1f, RayMasks.DAMAGE_SERVER, QueryTriggerInteraction.UseGlobal))
            {
                pos += OptimizationVariables.MainPlayer.look.aim.forward;
                points.Add(pos);
            }

            var deltaTime = Time.fixedDeltaTime;

            // collision and bounce calculations are too big brain for me
            for (int step = 1; step < maxSteps; step++)
            {
                pos += vel * deltaTime + 0.5f * Physics.gravity * deltaTime * deltaTime;
                vel += Physics.gravity * deltaTime;

                if (Physics.Linecast(points[step - 1], pos, out var hit, RayMasks.DAMAGE_CLIENT))
                {
                    points.Add(hit.point);
                    break;
                }

                 points.Add(pos);
            }

            return points;
        }

        public static List<Vector3> PlotTrajectoryRigidBodyInMotion(GameObject obj, int maxSteps)
        {
            var pos = obj.transform.position;
            var vel = obj.GetComponent<Rigidbody>().velocity;

            var points = new List<Vector3>()
            {
                obj.transform.position
            };

            var deltaTime = Time.fixedDeltaTime;

            for (int step = 1; step < maxSteps; step++)
            {
                pos += vel * deltaTime + 0.5f * Physics.gravity * deltaTime * deltaTime;
                vel += Physics.gravity * deltaTime;

                if (Physics.Linecast(points[step - 1], pos, out var hit, RayMasks.DAMAGE_CLIENT))
                {
                    points.Add(hit.point);
                    break;
                }

                points.Add(pos);
            }

            return points;
        }

        public static List<Vector3> PlotTrajectoryRocket(UseableGun gun, out RaycastHit hit, int maxSteps)
        {
            hit = default;

            var pos = OptimizationVariables.MainPlayer.look.aim.position;
            var force = OptimizationVariables.MainPlayer.look.aim.forward * gun.equippedGunAsset.ballisticForce;
            var mass = gun.equippedGunAsset.projectile.GetComponent<Rigidbody>().mass;
            var vel = (force / mass) * Time.fixedDeltaTime;

            var points = new List<Vector3>()
            {
                pos
            };

            if (!PhysicsUtility.raycast(new Ray(pos, OptimizationVariables.MainPlayer.look.aim.forward),
                out _, 1f, RayMasks.DAMAGE_SERVER, QueryTriggerInteraction.UseGlobal))
            {
                pos += OptimizationVariables.MainPlayer.look.aim.forward;
                points.Add(pos);
            }

            // tbh this is kinda pasted because im bad at physics
            var deltaTime = Time.fixedDeltaTime;

            for (int step = 1; step < maxSteps; step++)
            {
                pos += vel * deltaTime + 0.5f * Physics.gravity * deltaTime * deltaTime;
                vel += Physics.gravity * deltaTime;

                if (Physics.Linecast(points[step - 1], pos, out hit, RayMasks.DAMAGE_CLIENT))
                {
                    points.Add(hit.point);
                    break;
                }

                points.Add(pos);
            }

            return points;
        }

        public static List<Vector3> PlotTrajectoryGun(UseableGun gun, out RaycastHit hit, int maxSteps = 255)
        {
            hit = default;

            Transform transform = OptimizationVariables.MainPlayer.look.perspective == EPlayerPerspective.FIRST ? OptimizationVariables.MainPlayer.look.aim : OptimizationVariables.MainCam.transform;
            var pos = transform.position;
            var vec = transform.forward;

            var asset = gun.equippedGunAsset;
            float drop = asset.ballisticDrop;
            var attachments = (Attachments)thirdAttachmentsField.GetValue(gun);

            var points = new List<Vector3>
            {
                pos
            };

            if (attachments?.barrelAsset != null)
                drop *= attachments.barrelAsset.ballisticDrop;

            for (int steps = 1; steps < maxSteps; steps++)
            {
                pos += vec * asset.ballisticTravel;
                vec.y -= drop;

                vec.Normalize();

                if (Physics.Linecast(points[steps - 1], pos, out hit, RayMasks.DAMAGE_CLIENT))
                {
                    points.Add(hit.point);
                    break;
                }

                points.Add(pos);
            }

            return points;
        }

        [OnSpy]
        public static void OnSpy()
        {
            if (Highlighted != null)
                RemoveHighlight(Highlighted);

            spying = true;
        }

        [OffSpy]
        public static void OffSpy() => spying = false;
    }
}
