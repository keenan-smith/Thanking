using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Misc.Enums;
using Thanking.Options;
using Thanking.Utilities;
using Thanking.Variables;

namespace Thanking.Overrides
{
    public class OV_PlayerEquipment
    {
        public static bool WasPunching;
        public static uint CurrSim;
        
        [Override(typeof(PlayerEquipment), "punch", BindingFlags.NonPublic | BindingFlags.Instance)]
        public void OV_punch(EPlayerPunch p)
        {
            if (MiscOptions.PunchSilentAim)
                OV_DamageTool.OVType = OverrideType.PlayerHit;
            
            OverrideUtilities.CallOriginal(OptimizationVariables.MainPlayer.equipment, p);
            
            OV_DamageTool.OVType = OverrideType.None;
        }
     }
 }