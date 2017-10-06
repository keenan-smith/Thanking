using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Thanking.Attributes;
using Thanking.Variables;

namespace Thanking.Managers
{
    public static class ComponentManager
    {
        public static void Load()
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                foreach (Type tClass in asm.GetTypes())
                    if (tClass.IsClass)
                        foreach (MethodInfo method in tClass.GetMethods(BFlags.Everything))
                            if (Attribute.GetCustomAttribute(method, typeof(ComponentAttribute)) != null)
                                Loader.HookObject.AddComponent(tClass);
        }
    }
}
