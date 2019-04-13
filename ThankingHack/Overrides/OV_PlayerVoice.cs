using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Thanking.Attributes;
using Thanking.Options;
using Thanking.Utilities;
using Thanking.Variables;

namespace Thanking.Overrides
{
    public static class OV_PlayerVoice
    {
        private static PropertyInfo propInputWantsToRecord = typeof(PlayerVoice).GetProperty("inputWantsToRecord", BindingFlags.NonPublic | BindingFlags.Instance);
        private static bool InputWantsToRecord
        {
            get => (bool)propInputWantsToRecord.GetValue(OptimizationVariables.MainPlayer.voice);
            set => propInputWantsToRecord.SetValue(OptimizationVariables.MainPlayer.voice, value);
        }

        [Override(typeof(PlayerVoice), "updateInput", BindingFlags.NonPublic | BindingFlags.Instance)]
        public static void OV_updateInput()
        {
            if (MiscOptions.PerpetualVoiceChat)
                InputWantsToRecord = true;
            else
                OverrideUtilities.CallOriginal(instance: OptimizationVariables.MainPlayer.voice);
        }
    }
}
