using System;
using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Components.UI;
using Thanking.Utilities;
using Thanking.Variables;

namespace Thanking.Overrides
{
    public class OV_PlayerInput
    {
        //[Override(typeof(PlayerInput), "sendRaycast", BindingFlags.Public | BindingFlags.Instance)]
        public void OV_sendRaycast(RaycastInfo ri)
        {
            TracerLine tl = new TracerLine
            {
                StartPosition = OptimizationVariables.MainPlayer.transform.position,
                EndPosition = ri.point,
                Hit = !OV_UseableGun.IsRaycastInvalid(ri),
                CreationTime = DateTime.Now
            };
            WeaponComponent.Tracers.Add(tl);
            OverrideUtilities.CallOriginal(OptimizationVariables.MainPlayer.input, ri);
        }
    }
}