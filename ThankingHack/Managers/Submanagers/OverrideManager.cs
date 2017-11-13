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

        public static void Load()
        {
            #if DEBUG
            DebugUtilities.Log("Initializing OverrideManager");
            #endif
            
            Type[] Types = Assembly.GetExecutingAssembly().GetTypes().Where(T => T.IsClass).ToArray();

            for (int i = 0; i < Types.Length; i++)
            {
                MethodInfo[] Methods = Types[i].GetMethods(ReflectionVariables.Everything).Where(M => M.IsDefined(typeof(OverrideAttribute), false))
                    .ToArray();

                for (int o = 0; o < Methods.Length; o++)
                    LoadOverride(Methods[o]);
            }
        }
        
        public static void LoadOverride(MethodInfo method)
        {
            // Setup variables
            OverrideAttribute attribute = (OverrideAttribute)Attribute.GetCustomAttribute(method, typeof(OverrideAttribute));
			
			// Do checks
            
			if (Overrides.Count(a => a.Key.Method == attribute.Method) > 0)
                return;

			try
            {
                OverrideWrapper wrapper = new OverrideWrapper(attribute.Method, method, attribute);

                wrapper.Override();

                Overrides.Add(attribute, wrapper);
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }
    }
}
