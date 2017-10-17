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
        [Override(typeof(Input), "GetKey", BindingFlags.Public | BindingFlags.Static, 1)]
        public static bool OV_GetKey(KeyCode key)
        {
			if (key == ControlsSettings.primary && TriggerbotOptions.IsFiring)
				return true;

            return (bool)typeof(Input)
                    .GetMethod("GetKeyInt", BFlags.PrivateStatic)
                    .Invoke(null, new object[] { (int)key });
        }
    }
}
