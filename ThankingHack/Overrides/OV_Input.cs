using SDG.Unturned;
using System.Reflection;
using Thanking.Attributes;
using Thanking.Options.UtilityOptions;
using Thanking.Options.BotOptions;
using UnityEngine;

namespace Thanking.Overrides
{
    public static class OV_Input
    {
        [Override(typeof(Input), "GetKey", BindingFlags.Public | BindingFlags.Static)]
        public static bool OV_GetKey(KeyCode key)
        {
            if (key == ControlsSettings.primary && TriggerbotOptions.Enabled)
                return true;

            return (bool)typeof(OV_Input)
                    .GetMethod("GetKeyInt", BFlags.PublicStatic)
                    .Invoke(null, new object[] { (int)key });
        }
    }
}
