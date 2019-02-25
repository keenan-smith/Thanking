using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Misc.Enums;
using Thanking.Options;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using Thanking.Variables;

namespace Thanking.Overrides
{
    public class OV_UseableMelee
    {
        [Override(typeof(UseableMelee), "fire", BindingFlags.NonPublic | BindingFlags.Instance)]
        public static void OV_fire()
        {
            OV_DamageTool.OVType = OverrideType.None;
            
            if (RaycastOptions.Enabled && MiscOptions.ExtendMeleeRange)
                OV_DamageTool.OVType = OverrideType.SilentAimMelee;
            
            else if (RaycastOptions.Enabled)
                OV_DamageTool.OVType = OverrideType.SilentAim;
            
            else if (MiscOptions.ExtendMeleeRange)
                OV_DamageTool.OVType = OverrideType.Extended;

            OverrideUtilities.CallOriginal(OptimizationVariables.MainPlayer.equipment.useable);

            OV_DamageTool.OVType = OverrideType.None;
        }
    }
}