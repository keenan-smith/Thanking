using System;
using System.Reflection;
using Thanking.Attributes;

namespace Thanking.Managers.Submanagers
{
    public static class ComponentManager
    {
        public static void Load()
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                foreach (Type tClass in asm.GetTypes())
                    if (tClass.IsClass)
						if (tClass.IsDefined(typeof(ComponentAttribute), false))
							Loader.HookObject.AddComponent(tClass);
        }
    }
}
