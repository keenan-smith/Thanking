using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using SDG.Unturned;
using Thinking.Attributes;
using Thinking.Components.Basic;
using Thinking.Options;
using Thinking.Utilities;
using Thinking.Variables;

namespace Thinking.Overrides
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