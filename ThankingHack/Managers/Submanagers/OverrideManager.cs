using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Thanking.Attributes;
using Thanking.Variables;
using Thanking.Wrappers;
using UnityEngine;

namespace Thanking.Managers.Submanagers
{
    public static class OverrideManager
    {
        #region Variables
        private static Dictionary<OverrideAttribute, OverrideWrapper> _overrides = new Dictionary<OverrideAttribute, OverrideWrapper>(); // Dictionary of detours
        #endregion

        #region Properties
        public static Dictionary<OverrideAttribute, OverrideWrapper> Overrides => _overrides; // The public detours
        #endregion

        public static void Load()
        {
            Type[] Types = Assembly.GetExecutingAssembly().GetTypes().Where(T => T.IsClass).ToArray();

            for (int i = 0; i < Types.Length; i++)
            {
                MethodInfo[] Methods = Types[i].GetMethods(ReflectionVariables.Everything).Where(M => M.IsDefined(typeof(OverrideAttribute), false))
                    .ToArray();

                for (int o = 0; o < Methods.Length; o++)
                    LoadOverride(Methods[o]);
            }
        }
        #region Public Functions
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
#endregion
    }
}
