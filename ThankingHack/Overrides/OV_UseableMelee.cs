using System.Reflection;
using SDG.Unturned;
using Thinking.Attributes;
using Thinking.Options;
using Thinking.Options.AimOptions;
using Thinking.Utilities;
using Thinking.Variables;

namespace Thinking.Overrides
{
    public class OvPlayerEquipment
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