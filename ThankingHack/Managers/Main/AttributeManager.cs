using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Thanking.Attributes;
using Thanking.Managers.Submanagers;
using Thanking.Utilities;

namespace Thanking.Managers.Main
{
    public static class AttributeManager
    {
        public static void Init()
        {
	        #if DEBUG
			DebugUtilities.Log("Initializing attribute manager...");
            #endif

            List<Type> Components = new List<Type>();
            List<MethodInfo> Pre = new List<MethodInfo>();
            List<MethodInfo> Post = new List<MethodInfo>();
            
            foreach (Type T in Assembly.GetExecutingAssembly().GetTypes())
            {
                // Collect and add components marked with the attribute
                if (T.IsDefined(typeof(ComponentAttribute), false))
                {
                    Loader.HookObject.AddComponent(T);
                    continue;
                }

                // Collect components to be destroyed on spy
                if (T.IsDefined(typeof(SpyComponentAttribute), false))
                {
                    Components.Add(T);
                    continue;
                }

                foreach (MethodInfo M in T.GetMethods())
                {
                    // Collect and invoke methods marked to be initialized
                    if (M.IsDefined(typeof(InitializerAttribute), false))
                    {
                        M.Invoke(null, null);
                        continue;
                    }

                    // Collect and override methods marked with the attribute
                    if (M.IsDefined(typeof(OverrideAttribute), false))
                    {
                        OverrideManager.LoadOverride(M);
                        continue;
                    }

                    // Collect methods to be invoked before spy
                    if (M.IsDefined(typeof(OnSpyAttribute), false))
                    {
                        Pre.Add(M);
                        continue;
                    }

                    // Collect methods to be invoked after spy
                    if (M.IsDefined(typeof(OffSpyAttribute), false))
                    {
                        Post.Add(M);
                        continue;
                    }

                    // Collect and thread methods marked with the attribute
                    if (M.IsDefined(typeof(ThreadAttribute), false))
                        new Thread(new ThreadStart((Action) Delegate.CreateDelegate(typeof(Action), M))).Start();
                }
            }
            
            SpyManager.Components = Components;
            SpyManager.PostSpy = Post;
            SpyManager.PreSpy = Pre;
        }
    }
}
