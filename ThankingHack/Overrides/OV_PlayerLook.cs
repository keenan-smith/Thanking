using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Overrides
{
    public class OV_PlayerLook
    {
        [Override(typeof(PlayerLook), "updateAim", BindingFlags.Public | BindingFlags.Instance)]
        public void updateAim(float delta)
        {
            if (WeaponOptions.NoSway)
                return;

            OverrideUtilities.CallOriginal(OptimizationVariables.MainPlayer.look, delta);
        }
    }
}