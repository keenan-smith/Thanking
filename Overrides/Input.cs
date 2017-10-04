using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Thanking.Attributes;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Overrides
{
    public static class Input
    {
        [Override(typeof(UnityEngine.Input), "GetKey", BindingFlags.Public | BindingFlags.Static)]
        public static bool OV_GetKey(KeyCode key)
        {
            if (key == ControlsSettings.primary && TriggerbotOptions.Enabled)
                return true;

            return (bool)typeof(Input)
                    .GetMethod("GetKeyInt", BFlags.PublicStatic)
                    .Invoke(null, new object[] { (int)key });
        }
    }
}
