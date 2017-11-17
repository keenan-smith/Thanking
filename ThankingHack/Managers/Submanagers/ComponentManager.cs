using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Thanking.Attributes;
using Thanking.Utilities;

namespace Thanking.Managers.Submanagers
{
    public static class ComponentManager
    {
        /// <summary>
        /// Collect and add components marked with ComponentAttribute
        /// </summary>
        public static void Load()
        {
            #if DEBUG
            DebugUtilities.Log("Initializing ComponentManager");
            #endif
            
            IEnumerable<Type> Types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(T => T.IsClass && T.IsDefined(typeof(ComponentAttribute), false)).ToArray();

            foreach (Type T in Types)
                Loader.HookObject.AddComponent(T);
        }
    }
}
