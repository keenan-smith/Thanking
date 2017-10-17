using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Thanking.Wrappers;
using Thanking.Attributes;
using UnityEngine;
using Thanking.Options.UtilityOptions;

namespace Thanking.Managers
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
            // Main code
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                foreach (Type tClass in asm.GetTypes())
                    if (tClass.IsClass)
                        foreach (MethodInfo method in tClass.GetMethods(BFlags.Everything))
                            LoadOverride(method);

            // Set the variables
        }
        #region Public Functions
        public static void LoadOverride(MethodInfo method)
        {
            // Setup variables
            OverrideAttribute attribute = (OverrideAttribute)Attribute.GetCustomAttribute(method, typeof(OverrideAttribute));

            // Do checks
            if (attribute == null)
                return;
            if (!attribute.MethodFound)
                return;
            if (Overrides.Count(a => a.Key.Method == attribute.Method) > 0)
                return;

            try
            {
                OverrideWrapper wrapper = new OverrideWrapper(attribute.Method, method, attribute);

                wrapper.Override();

                Overrides.Add(attribute, wrapper);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
#endregion
    }
}
