using System;
using System.Collections.Generic;
using System.Linq;
using Thanking.Attributes;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Thanking.Wrappers
{
    internal class OverrideWrapper
    {
        #region Properties
        public MethodInfo Original { get; private set; }
        public MethodInfo Modified { get; private set; }

        public IntPtr PtrOriginal { get; private set; }
        public IntPtr PtrModified { get; private set; }

        public OverrideManager.OffsetBackup OffsetBackup { get; private set; }
        public OverrideAttribute Attribute { get; private set; }

        public bool Detoured { get; private set; }
        public object Instance { get; private set; }
        public bool Local { get; private set; }
        #endregion

        public OverrideWrapper(MethodInfo original, MethodInfo modified, OverrideAttribute attribute, object instance = null)
        {
            // Set the variables
            Original = original;
            Modified = modified;
            Instance = instance;
            Attribute = attribute;
            Local = (Modified.DeclaringType.Assembly == Assembly.GetExecutingAssembly());

            RuntimeHelpers.PrepareMethod(original.MethodHandle);
            RuntimeHelpers.PrepareMethod(modified.MethodHandle);
            PtrOriginal = Original.MethodHandle.GetFunctionPointer();
            PtrModified = Modified.MethodHandle.GetFunctionPointer();

            OffsetBackup = new RedirectionHelper.OffsetBackup(PtrOriginal);
            Detoured = false;
        }

        #region Public Functions
        public bool Detour()
        {
            if (Detoured)
                return true;
            bool result = RedirectionHelper.DetourFunction(PtrOriginal, PtrModified);

            if (result)
                Detoured = true;

            return result;
        }

        public bool Revert()
        {
            if (!Detoured)
                return false;
            bool result = RedirectionHelper.RevertDetour(OffsetBackup);

            if (result)
                Detoured = false;

            return result;
        }

        public object CallOriginal(object[] args, object instance = null)
        {
            Revert();
            object result = null;
            try
            {
                result = Original.Invoke(instance ?? Instance, args);
            }
            catch (Exception ex)
            {

            }

            Detour();
            return result;
        }
        #endregion
    }
}
