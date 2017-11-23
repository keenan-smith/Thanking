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