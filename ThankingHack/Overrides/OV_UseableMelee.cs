using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using Thanking.Variables;

namespace Thanking.Overrides
{
    public class OvPlayerEquipment
    {
        [Override(typeof(UseableMelee), "fire", BindingFlags.NonPublic | BindingFlags.Instance)]
        public static void OV_fire()
        {
            OV_DamageTool.OVType = RaycastOptions.Enabled ? OverrideType.SilentAim :
                MiscOptions.ExtendMeleeRange ? OverrideType.Extended : OverrideType.None;

            OverrideUtilities.CallOriginal(OptimizationVariables.MainPlayer.equipment.useable);

            OV_DamageTool.OVType = OverrideType.None;
        }
    }
}