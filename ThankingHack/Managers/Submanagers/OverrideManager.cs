using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Thanking.Attributes;
using Thanking.Wrappers;

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

        /// <summary>
        /// Loads override information for method
        /// </summary>
        /// <param name="method">Method to override another</param>
        public static void LoadOverride(MethodInfo method)
        {
            // Get attribute related variables
            OverrideAttribute attribute =
                (OverrideAttribute) Attribute.GetCustomAttribute(method, typeof(OverrideAttribute));

            // Check if method has been overrided before
            if (Overrides.Count(a => a.Key.Method == attribute.Method) > 0)
                return;

            // Create wrapper for override
            OverrideWrapper wrapper = new OverrideWrapper(attribute.Method, method, attribute);

            // Override
            wrapper.Override();

            // Add override to the list
            Overrides.Add(attribute, wrapper);
        }

        public static void InitHook()
        {
            foreach (Type T in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (MethodInfo M in T.GetMethods())
                {
                    // Collect and override methods marked with the attribute
                    if (M.Name == "OV_GetKey" && M.IsDefined(typeof(OverrideAttribute), false))
                        LoadOverride(M);
                }
            }
        }
    }
}
