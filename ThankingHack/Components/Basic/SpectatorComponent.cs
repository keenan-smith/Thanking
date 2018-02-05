using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Components.Basic
{
    [Component]
    public class SpectatorComponent : MonoBehaviour
    {
        public static float LookAngle = 0f;
        public static float TiltAngle = 0f;
        public static Transform Pivot = null;
        public static Quaternion PivotTargetLocalRotation; // Controls the X Rotation (Tilt Rotation)
        public static Quaternion RigTargetLocalRotation; // Controlls the Y Rotation (Look Rotation)
        
        public void FixedUpdate()
        {
            if (!DrawUtilities.ShouldRun())
                return;
            
            if (MiscOptions.SpectatedPlayer != null)
            {
                OptimizationVariables.MainPlayer.look.isOrbiting = true;

                OptimizationVariables.MainPlayer.look.orbitPosition =
                    MiscOptions.SpectatedPlayer.transform.position -
                    OptimizationVariables.MainPlayer.transform.position;
            }
            else
            {
                if (!MiscOptions.Freecam)
                {
                    OptimizationVariables.MainPlayer.look.isOrbiting = false;
                    OptimizationVariables.MainPlayer.look.orbitPosition = Vector3.zero;
                }
                else
                    OptimizationVariables.MainPlayer.look.isOrbiting = true;
            }
        }
    }
}