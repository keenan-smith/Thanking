﻿using SDG.Unturned;
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
                
                OptimizationVariables.MainPlayer.look.orbitPosition += new Vector3(0, 3, 0);
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