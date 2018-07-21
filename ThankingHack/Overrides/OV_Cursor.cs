using System.Reflection;
using Thanking.Attributes;
using Thanking.Components.UI.Menu;
using UnityEngine;

namespace Thanking.Overrides
{
    public static class OV_Cursor
    {
        public static CursorLockMode RealMode;
        
        [Override(typeof(Cursor), "get_lockState", BindingFlags.Public | BindingFlags.Static)]
        public static CursorLockMode OV_get_lockState() => 
            MenuComponent.IsInMenu ? CursorLockMode.None : RealMode;

        [Override(typeof(Cursor), "set_lockState", BindingFlags.Public | BindingFlags.Static)]
        public static void OV_set_lockState(CursorLockMode mode) =>
            RealMode = mode;
    }
}