using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Thanking.Attributes;
using Thanking.Utilities;

namespace Thanking.Wrappers
{
    public class OverrideWrapper
    {
        #region Properties
        
        public MethodInfo Original { get; private set; }
        public MethodInfo Modified { get; private set; }

        public IntPtr PtrOriginal { get; private set; }
        public IntPtr PtrModified { get; private set; }

        public OverrideUtilities.OffsetBackup OffsetBackup { get; private set; }
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
            Local = Modified.DeclaringType.Assembly == Assembly.GetExecutingAssembly();

            RuntimeHelpers.PrepareMethod(original.MethodHandle);
            RuntimeHelpers.PrepareMethod(modified.MethodHandle);
            PtrOriginal = Original.MethodHandle.GetFunctionPointer();
            PtrModified = Modified.MethodHandle.GetFunctionPointer();

            OffsetBackup = new OverrideUtilities.OffsetBackup(PtrOriginal);
            Detoured = false;
        }

        #region Public Functions
        public bool Override()
        {
            if (Detoured)
                return true;
            bool result = OverrideUtilities.OverrideFunction(PtrOriginal, PtrModified);

            if (result)
                Detoured = true;

            return result;
        }

        public bool Revert()
        {
            if (!Detoured)
                return false;
            bool result = OverrideUtilities.RevertOverride(OffsetBackup);

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
            catch (Exception e)
            {
                DebugUtilities.Log("ERROR IN OVERRIDDEN METHOD: " + Original.Name);
                DebugUtilities.LogException(e);
            }

            Override();
            return result;
        }
        #endregion
    }
}
