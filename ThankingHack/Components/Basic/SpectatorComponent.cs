using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Components.Basic
{
    [Component]
    public class SpectatorComponent : MonoBehaviour
    {
        public static bool LastState;
        public static EPlayerPerspective LastPerspective;
        public void FixedUpdate()
        {
            if (MiscOptions.SpectatedPlayer != null)
            {
                OptimizationVariables.MainPlayer.look.orbitPosition =
                    OptimizationVariables.MainPlayer.transform.TransformPoint(MiscOptions.SpectatedPlayer.transform
                        .position);
                OptimizationVariables.MainPlayer.look.orbitPosition += new Vector3(0, 5, 0);
                
                if (LastState) 
                    return;

                OptimizationVariables.MainPlayer.look.isOrbiting = true;
            }
            else
            {
                OptimizationVariables.MainPlayer.look.orbitPosition = Vector3.zero;
                
                if (!LastState) 
                    return;

                OptimizationVariables.MainPlayer.look.isOrbiting = false;
            }
        }
    }
}