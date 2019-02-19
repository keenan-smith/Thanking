using SDG.Unturned;
using Thinking.Attributes;
using Thinking.Coroutines;
using Thinking.Options;
using Thinking.Utilities;
using Thinking.Variables;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Thinking.Components.Basic
{
    [Component]
    public class SpectatorComponent : MonoBehaviour
    {
        public void FixedUpdate()
        {
            if (!DrawUtilities.ShouldRun())
                return;
            
            if (MiscOptions.SpectatedPlayer != null && !PlayerCoroutines.IsSpying)
            {
                OptimizationVariables.MainPlayer.look.isOrbiting = true;

                OptimizationVariables.MainPlayer.look.orbitPosition =
                    MiscOptions.SpectatedPlayer.transform.position -
                    OptimizationVariables.MainPlayer.transform.position;
                
                OptimizationVariables.MainPlayer.look.orbitPosition += new Vector3(0, 3, 0);
            }
            else
                OptimizationVariables.MainPlayer.look.isOrbiting = MiscOptions.Freecam;
        }
    }
}