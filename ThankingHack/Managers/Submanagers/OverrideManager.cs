using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Thanking.Attributes;
using Thanking.Variables;
using Thanking.Wrappers;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Managers.Submanagers
{
    public static class OverrideManager
    {
        //TODO:Don't make regions for one line, or for one method

        // Dictionary of detours
        private static Dictionary<OverrideAttribute, OverrideWrapper> _overrides =
            new Dictionary<OverrideAttribute, OverrideWrapper>();

        // The public detours
        public static Dictionary<OverrideAttribute, OverrideWrapper> Overrides => _overrides;

		// String dictionary of detours for easier accessing
		public static Dictionary<string, OverrideWrapper> Wrappers = 
			new Dictionary<string, OverrideWrapper>();

		public static void LoadOverride(MethodInfo method)
        {
            // Get attribute related variables
            OverrideAttribute attribute = (OverrideAttribute)Attribute.GetCustomAttribute(method, typeof(OverrideAttribute));
			
			// Check if method has been overrided before
			if (Overrides.Count(a => a.Key.Method == attribute.Method) > 0)
                return;

			try
            {
                OverrideWrapper wrapper = new OverrideWrapper(attribute.Method, method, attribute);

                wrapper.Override();

                Overrides.Add(attribute, wrapper);
				Wrappers.Add(attribute.Class.Name + "_" + attribute.MethodName, wrapper);
            }
            catch (Exception ex) { DebugUtilities.LogException(ex); }
        }
    }
}
