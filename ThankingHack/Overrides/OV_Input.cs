using SDG.Unturned;
using System.Linq;
using System.Reflection;
using Thanking.Attributes;
using Thanking.Managers.Submanagers;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using Thanking.Variables;
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

            return 
				(bool)OverrideUtilities.CallOriginal(null, key);
		}
    }
}
