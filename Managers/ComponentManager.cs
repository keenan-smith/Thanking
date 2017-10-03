using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Thanking.Attributes;

namespace Thanking.Managers
{
    public static class ComponentManager
    {
        public static void Load()
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                foreach (Type tClass in asm.GetTypes())
                    if (tClass.IsClass)
                        foreach (MethodInfo method in tClass.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                            if (Attribute.GetCustomAttribute(method, typeof(OverrideAttribute)) != null)
                                LOADING.Loader.HookObject.AddComponent(tClass);]
        }
    }
}
