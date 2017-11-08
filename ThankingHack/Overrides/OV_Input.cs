using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options.AimOptions;
using Thanking.Variables;
using UnityEngine;
using Thanking.Utilities;

namespace Thanking.Overrides
{
	public static class OV_Input
	{
		private static MethodInfo GetKeyInt;

		[Initializer]
		public static void Load() =>
			GetKeyInt = typeof(Input).GetMethod("GetKeyInt", ReflectionVariables.PrivateStatic);

		[Override(typeof(Input), "GetKey", BindingFlags.Public | BindingFlags.Static, 1)]
        public static bool OV_GetKey(KeyCode key)
		{
			if (!DrawUtilities.ShouldRun())
				return (bool)GetKeyInt.Invoke(null, new object[] { (int)key });

			if (key == ControlsSettings.primary && TriggerbotOptions.IsFiring)
				return true;
			
			return (bool)GetKeyInt.Invoke(null, new object[] { (int)key });
		}
    }
}
