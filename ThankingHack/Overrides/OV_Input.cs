using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

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
				return Invoke(key);

			if (key == ControlsSettings.primary && TriggerbotOptions.IsFiring)
				return true;
			
			return Invoke(key);
		}
		
		public static bool Invoke(KeyCode key) =>
			(bool)GetKeyInt.Invoke(null, new object[] { (int)key });
    }
}
