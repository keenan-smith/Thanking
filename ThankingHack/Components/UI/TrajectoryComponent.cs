using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HighlightingSystem;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Thanking.Attributes;
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
        private static readonly FieldInfo thirdAttachments = typeof(UseableGun).GetField("thirdAttachments", BindingFlags.Instance | BindingFlags.NonPublic);

        public static Highlighter Highlighted { get; private set; }
        private static bool spying;

        [Initializer]
        public static void Initialize()
        {
            ColorUtilities.addColor(new ColorVariable("_TrajectoryPredictionInRange", "B.D. Predict (In Range)", Color.cyan));
            ColorUtilities.addColor(new ColorVariable("_TrajectoryPredictionOutOfRange", "B.D. Predict (Out of Range)", Color.red));
        }

        private void DoNormalTrajectory(UseableGun item)
        {
            var traj = PlotTrajectory(item, out var hit);
            var outOfRange = Vector3.Distance(traj.Last(), OptimizationVariables.MainPlayer.look.aim.position) > item.equippedGunAsset.range;
            var inRangeColor = ColorUtilities.getColor("_TrajectoryPredictionInRange");
            var outOfRangeColor = ColorUtilities.getColor("_TrajectoryPredictionOutOfRange");

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

                        newHighlight.ConstantOnImmediate(outOfRange ? outOfRangeColor : inRangeColor);
                    }

                    if (Highlighted != null && newHighlight != Highlighted)
                        RemoveHighlight(Highlighted);

                    Highlighted = newHighlight;
                }
                else if (Highlighted != null)
                {
                    RemoveHighlight(Highlighted);
                    Highlighted = null;
                }
            }
            else if (!WeaponOptions.HighlightBulletDropPredictionTarget && Highlighted != null)
            {
                RemoveHighlight(Highlighted);
                Highlighted = null;
            }

            DrawLine(traj, outOfRange ? outOfRangeColor : inRangeColor);
        }

        private void DoRocketTrajectory(UseableGun item)
        {
            DrawLine(PlotTrajectoryRocket(item, out var hit, 255), ColorUtilities.getColor("_TrajectoryPredictionInRange"));
        }

        private void DrawLine(List<Vector3> points, Color color)
        {
            ESPComponent.GLMat.SetPass(0);
            GL.PushMatrix();
            GL.LoadProjectionMatrix(OptimizationVariables.MainCam.projectionMatrix);
            GL.modelview = OptimizationVariables.MainCam.worldToCameraMatrix;
            GL.Begin(GL.LINE_STRIP);

            GL.Color(color);

            foreach (var x in points)
                GL.Vertex(x);

            GL.End();
            GL.PopMatrix();
        }

        public void OnGUI()
        {
            var item = OptimizationVariables.MainPlayer?.equipment?.useable as UseableGun;

            if (item == null || spying || !WeaponOptions.EnableBulletDropPrediction || !Provider.modeConfigData.Gameplay.Ballistics)
            {
                if (Highlighted != null)
                {
                    RemoveHighlight(Highlighted);
                    Highlighted = null;
                }

                return;
            }

            if (item.equippedGunAsset.action != EAction.Rocket)
            {
                DoNormalTrajectory(item);
            }
            else
            {
                DoRocketTrajectory(item);
            }
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

        public static List<Vector3> PlotTrajectoryRocket(UseableGun gun, out RaycastHit hit, int maxSteps)
        {
            hit = default;

            var pos = OptimizationVariables.MainPlayer.look.aim.position;
            var vel = OptimizationVariables.MainPlayer.look.aim.forward * gun.equippedGunAsset.ballisticForce;

            if (!PhysicsUtility.raycast(new Ray(pos, OptimizationVariables.MainPlayer.look.aim.forward),
                out var rchit, 1f, RayMasks.DAMAGE_SERVER, QueryTriggerInteraction.UseGlobal))
                pos += vel;

            var t = Time.fixedDeltaTime;

            var points = new List<Vector3>();

            DebugUtilities.Log("dab " + Physics.gravity);

            for (int step = 0; step < maxSteps; step++)
            {
                pos = pos + t * vel + t * t * Physics.gravity;

                vel = vel + t * Physics.gravity;
                DebugUtilities.Log("xd " + pos + " | " + vel);
            }

            return points;
        }

        public static List<Vector3> PlotTrajectory(UseableGun gun, out RaycastHit hit, int maxSteps = 255)
        {
            hit = default;

            Transform transform = OptimizationVariables.MainPlayer.look.perspective == EPlayerPerspective.FIRST ? OptimizationVariables.MainPlayer.look.aim : OptimizationVariables.MainCam.transform;
            var pos = transform.position;
            var vec = transform.forward;

            var asset = gun.equippedGunAsset;
            float drop = asset.ballisticDrop;
            var attachments = (Attachments)thirdAttachments.GetValue(gun);

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
