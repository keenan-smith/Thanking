using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Thanking.Wrappers;
using Thanking.Attributes;
using UnityEngine;

namespace Thanking.Managers
{
    public static class OverrideManager
    {
        public static void Load()
        {
            // Main code
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                foreach (Type tClass in asm.GetTypes())
                    if (tClass.IsClass)
                        foreach (MethodInfo method in tClass.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                            LoadDetour(method);

            // Set the variables
        }
        #region Public Functions
        public static void LoadDetour(MethodInfo method)
        {
            // Setup variables
            OverrideAttribute attribute = (OverrideAttribute)Attribute.GetCustomAttribute(method, typeof(OverrideAttribute));

            // Do checks
            if (attribute == null)
                return;
            if (!attribute.MethodFound)
                return;

            try
            {
                OverrideWrapper wrapper = new OverrideWrapper(attribute.Method, method, attribute);

                wrapper.Detour();
            }
            catch (Exception ex)

            {
            }
        }
#endregion
    }
}
