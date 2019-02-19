using System.Reflection;
using Thinking.Attributes;
using Thinking.Components.UI.Menu;
using Thinking.Coroutines;
using Thinking.Utilities;
using UnityEngine;

namespace Thinking.Overrides
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