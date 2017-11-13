using System;
using System.Linq;
using System.Reflection;
using Thanking.Attributes;
using Thanking.Utilities;

namespace Thanking.Managers.Submanagers
{
    public static class ComponentManager
    {
        #if DEBUG
        DebugUtilities.Log("Initializing ComponentManager");
        #endif
        
        public static void Load()
        {
            Type[] Types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(T => T.IsClass && T.IsDefined(typeof(ComponentAttribute), false)).ToArray();

            for (int i = 0; i < Types.Length; i++)
                Loader.HookObject.AddComponent(Types[i]);
        }
    }
}
