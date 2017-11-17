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
        
        private static Dictionary<OverrideAttribute, OverrideWrapper> _overrides = new Dictionary<OverrideAttribute, OverrideWrapper>(); // Dictionary of detours

        public static Dictionary<OverrideAttribute, OverrideWrapper> Overrides => _overrides; // The public detours

        /// <summary>
        /// Collect and override methods marked with OvererideAttribute
        /// </summary>
        public static void Load()
        {
            #if DEBUG
            DebugUtilities.Log("Initializing OverrideManager");
            #endif

            IEnumerable<Type> Types = Assembly.GetExecutingAssembly().GetTypes().Where(T => T.IsClass);

            foreach (Type T in Types)
            {
                IEnumerable<MethodInfo> Methods = T.GetMethods(ReflectionVariables.Everything)
                    .Where(M => M.IsDefined(typeof(OverrideAttribute), false));

                foreach(MethodInfo M in Methods)
                    LoadOverride(M);
            }
        }
        
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
            }
            catch (Exception ex) { DebugUtilities.LogException(ex); }
        }
    }
}
