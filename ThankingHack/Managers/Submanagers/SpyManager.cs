using System;
using System.Collections.Generic;
using System.Reflection;
using Thanking.Misc;
using Thnkng;
using Object = UnityEngine.Object;

namespace Thanking.Managers.Submanagers
{
    public class SpyManager
    {
        #region Fields
        
        // Methods to be invoked before spy
        public static IEnumerable<MethodInfo> PreSpy;
        
        // Components to be destroyed before spy, and created again after spy
        public static IEnumerable<Type> Components;
        
        // Methods to be invoked after spy
        public static IEnumerable<MethodInfo> PostSpy;

        #endregion
        
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
                Object.Destroy(Ldr.HookObject.GetComponent(C));
        }

        /// <summary>
        /// Add components marked with SpyComponentAttribute that were previously destroyed
        /// </summary>
        public static void AddComponents()
        {
            foreach (Type C in Components)
                Ldr.HookObject.AddComponent(C);
        }
    }
}