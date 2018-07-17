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
            if (MiscOptions.ExtendMeleeRange && !(OptimizationVariables.MainPlayer.equipment.useable is UseableGun))
                OV_DamageTool.OVType = OverrideType.Extended;
            else if (RaycastOptions.Enabled)
                OV_DamageTool.OVType = OverrideType.SilentAim;
            else
                OV_DamageTool.OVType = OverrideType.None;

            OverrideUtilities.CallOriginal(OptimizationVariables.MainPlayer.equipment.useable);

            OV_DamageTool.OVType = OverrideType.None;
        }
    }
}