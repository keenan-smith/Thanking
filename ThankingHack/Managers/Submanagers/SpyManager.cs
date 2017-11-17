using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Thanking.Attributes;
using Thanking.Variables;
using Thanking.Utilities;
using Object = UnityEngine.Object;

namespace Thanking.Managers.Submanagers
{
    public class SpyManager
    {
        public static IEnumerable<MethodInfo> PreSpy;
        public static IEnumerable<Type> Components;
        public static IEnumerable<MethodInfo> PostSpy;

        /// <summary>
        ///  Collects types marked with SpyComponentAttribute and Methods marked with OnSpyAttribute and OffSpyAttribute
        /// </summary>
        public static void Load()
        {
            #if DEBUG
            DebugUtilities.Log("Initializing SpyManager");
            #endif
            
            List<MethodInfo> Pre = new List<MethodInfo>();
            List<Type> Comps = new List<Type>();
            List<MethodInfo> Post = new List<MethodInfo>();

            IEnumerable<Type> Types = Assembly.GetExecutingAssembly().GetTypes().Where(T => T.IsClass);

            foreach (Type T in Types)
            {
                // Collect components that should be destroyed before a spy screenshot is taken
                if (T.IsDefined(typeof(SpyComponentAttribute), false))
                    Comps.Add(T);

                // Collect only static methods because we cannot invoke methods that must be instantiated
                MethodInfo[] Methods = T.GetMethods(ReflectionVariables.PublicStatic);

                for (int o = 0; o < Methods.Length; o++)
                {
                    MethodInfo Method = Methods[o];

                    // Collect methods marked with OnSpyAttribute
                    if (Method.IsDefined(typeof(OnSpyAttribute), false))
                        Pre.Add(Method);

                    // Collect methods marked with OffSpyAttribute
                    if (Method.IsDefined(typeof(OffSpyAttribute), false))
                        Post.Add(Method);
                }
            }

            PreSpy = Pre;
            Components = Comps;
            PostSpy = Post;
        }

        /// <summary>
        /// Invoke methods marked with OnSpyAttribute
        /// </summary>
        public static void InvokePre()
        {
            foreach (MethodInfo M in PreSpy)
                M.Invoke(null, null);
        }

        /// <summary>
        /// Invoke methods marked with OffSpyAttribute
        /// </summary>
        public static void InvokePost()
        {
            foreach (MethodInfo M in PostSpy)
                M.Invoke(null, null);
        }

        /// <summary>
        /// Destroy components marked with SpyComponentAttribute
        /// </summary>
        public static void DestroyComponents()
        {
            foreach (Type C in Components)
                Object.Destroy(Loader.HookObject.GetComponent(C));
        }

        /// <summary>
        /// Add components marked with SpyComponentAttribute that were previously destroyed
        /// </summary>
        public static void AddComponents()
        {
            foreach (Type C in Components)
                Loader.HookObject.AddComponent(C);
        }
    }
}