using HighlightingSystem;
using SDG.Unturned;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Thinking.Attributes;
using Thinking.Options.AimOptions;
using Thinking.Options.UIVariables;
using Thinking.Utilities;
using Thinking.Variables;
using UnityEngine;

namespace Thinking.Components.UI
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

                    if (!newHighlight.highlighted)
                    {
                        newHighlight.OccluderOn();
                        newHighlight.SeeThroughOn();
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

            ESPComponent.GLMat.SetPass(0);
            GL.PushMatrix();
            GL.LoadProjectionMatrix(OptimizationVariables.MainCam.projectionMatrix);
            GL.modelview = OptimizationVariables.MainCam.worldToCameraMatrix;
            GL.Begin(GL.LINES);

            GL.Color(outOfRange ? outOfRangeColor : inRangeColor);

            foreach (var x in traj)
                GL.Vertex(x);

            GL.End();
            GL.PopMatrix();
        }

        private static void RemoveHighlight(Highlighter h)
        {
            if (h == null)
                return;

            h.OccluderOff();
            h.SeeThroughOff();
            h.ConstantOffImmediate();
        }

        public static List<Vector3> PlotTrajectory(UseableGun gun, out RaycastHit hit, int maxSteps = 255)
        {
            hit = default;

            var pos = OptimizationVariables.MainPlayer.look.aim.position;
            var vec = OptimizationVariables.MainPlayer.look.aim.forward;

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
