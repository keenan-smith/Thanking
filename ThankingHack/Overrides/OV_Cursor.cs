using System.Reflection;
using Thanking.Attributes;
using Thanking.Components.UI.Menu;
using Thanking.Coroutines;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Overrides
{
    public static class OV_Cursor
    {
        [Override(typeof(Cursor), "set_lockState", BindingFlags.Public | BindingFlags.Static)]
        public static void OV_set_lockState(CursorLockMode rMode)
        {
            if (MenuComponent.IsInMenu && !PlayerCoroutines.IsSpying &&
                (rMode == CursorLockMode.Confined || rMode == CursorLockMode.Locked))
                return;
            
            OverrideUtilities.CallOriginal(null, rMode);
        }
    }
}