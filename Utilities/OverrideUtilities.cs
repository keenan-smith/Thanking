using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Reflection;
using Thanking.Wrappers;
using Thanking.Managers;
using System.Diagnostics;
using Thanking.Attributes;
using Debug = UnityEngine.Debug;

namespace Thanking.Utilities
{
    public static class OverrideUtilities
    {
        #region Public Functions
        /// <summary>
        /// Calls the original method that was Overrideed
        /// </summary>
        /// <param name="method">The original method</param>
        /// <param name="instance">The instance for the method(null if static)</param>
        /// <param name="args">The arguments for the method</param>
        /// <returns>The value that the original function returns</returns>
        public static object CallOriginal(MethodInfo method, object instance = null, params object[] args)
        {
            // Set the variables
            OverrideWrapper wrapper = OverrideManager.Overrides.First(a => a.Value.Original == method).Value;

            // Do the checks
            if (wrapper == null)
                throw new Exception("The Override specified was not found!");

            return wrapper.CallOriginal(args, instance);
        }

        /// <summary>
        /// Calls the original method that was Overrideed
        /// </summary>
        /// <param name="instance">The instance for the method(null if static)</param>
        /// <param name="args">The arguments for the method</param>
        /// <returns>The value tahat the original function returns</returns>
        public static object CallOriginal(object instance = null, params object[] args)
        {
            StackTrace trace = new StackTrace(false);

            if (trace.FrameCount < 1)
                throw new Exception("Invalid trace back to the original method! Please provide the methodinfo instead!");

            MethodBase modded = trace.GetFrame(1).GetMethod();
            MethodInfo original = null;

            if (!Attribute.IsDefined(modded, typeof(OverrideAttribute)))
                modded = trace.GetFrame(2).GetMethod();
            OverrideAttribute att = (OverrideAttribute)Attribute.GetCustomAttribute(modded, typeof(OverrideAttribute));

            if (att == null)
                throw new Exception("This method can only be called from an overwritten method!");
            if (!att.MethodFound)
                throw new Exception("The original method was never found!");
            original = att.Method;

            OverrideWrapper wrapper = OverrideManager.Overrides.First(a => a.Value.Original == original).Value;

            if (wrapper == null)
                throw new Exception("The Override specified was not found!");

            return wrapper.CallOriginal(args, instance);
        }

        /// <summary>
        /// Enables the override of a method(WARNING: The method needs to have been overridden atleast once!)
        /// </summary>
        /// <param name="method">The original method that was overridden</param>
        /// <returns>If the override was enabled successfully</returns>
        public static bool EnableOverride(MethodInfo method)
        {
            // Set the variables
            OverrideWrapper wrapper = OverrideManager.Overrides.First(a => a.Value.Original == method).Value;

            // Do the checks
            if (wrapper == null)
                return false;

            return wrapper.Override();
        }

        /// <summary>
        /// Disables the pverride of a method(WARNING: The method needs to have been overridden atleast once!)
        /// </summary>
        /// <param name="method">The original method that was Overrideed</param>
        /// <returns>If the Override was disabled successfully</returns>
        public static bool DisableOverride(MethodInfo method)
        {
            // Set the variables
            OverrideWrapper wrapper = OverrideManager.Overrides.First(a => a.Value.Original == method).Value;

            // Do the checks
            if (wrapper == null)
                return false;

            return wrapper.Revert();
        }
        #region Public Functions
        public static bool OverrideFunction(IntPtr ptrOriginal, IntPtr ptrModified)
        {
            try
            {
                switch (IntPtr.Size)
                {
                    case sizeof(Int32):
                        unsafe
                        {
                            Debug.LogWarning("Overrideing " + ptrOriginal + " to " + ptrModified + "...");
                            byte* ptrFrom = (byte*)ptrOriginal.ToPointer();

                            *ptrFrom = 0x68; // PUSH
                            *((uint*)(ptrFrom + 1)) = (uint)ptrModified.ToInt32(); // Pointer
                            *(ptrFrom + 5) = 0xC3; // RETN
                        }
                        break;
                    case sizeof(Int64):
                        unsafe
                        {
                            Debug.LogWarning("Overrideing " + ptrOriginal + " to " + ptrModified + "...");

                            byte* ptrFrom = (byte*)ptrOriginal.ToPointer();

                            *ptrFrom = 0x48; // REX.W
                            *(ptrFrom + 1) = 0xB8; // MOV
                            *((ulong*)(ptrFrom + 2)) = (ulong)ptrModified.ToInt64(); // Pointer
                            *(ptrFrom + 10) = 0xFF; // INC 1
                            *(ptrFrom + 11) = 0xE0; // LOOPE
                        }
                        break;
                    default:
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool RevertOverride(OffsetBackup backup)
        {
            try
            {
                unsafe
                {
                    byte* ptrOriginal = (byte*)backup.Method.ToPointer();

                    *ptrOriginal = backup.A;
                    *(ptrOriginal + 1) = backup.B;
                    *(ptrOriginal + 10) = backup.C;
                    *(ptrOriginal + 11) = backup.D;
                    *(ptrOriginal + 12) = backup.E;
                    if (IntPtr.Size == sizeof(Int32))
                    {
                        *((uint*)(ptrOriginal + 1)) = backup.F32;
                        *(ptrOriginal + 5) = backup.G;
                    }
                    else
                    {
                        *((ulong*)(ptrOriginal + 2)) = backup.F64;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region SubClasses
        public class OffsetBackup
        {
            #region Variables
            public IntPtr Method;

            public byte A, B, C, D, E, G;
            public ulong F64;
            public uint F32;
            #endregion

            public OffsetBackup(IntPtr method)
            {
                Method = method;

                unsafe
                {
                    byte* ptrMethod = (byte*)method.ToPointer();

                    A = *ptrMethod;
                    B = *(ptrMethod + 1);
                    C = *(ptrMethod + 10);
                    D = *(ptrMethod + 11);
                    E = *(ptrMethod + 12);
                    if (IntPtr.Size == sizeof(Int32))
                    {
                        F32 = *((uint*)(ptrMethod + 1));
                        G = *(ptrMethod + 5);
                    }
                    else
                    {
                        F64 = *((ulong*)(ptrMethod + 2));
                    }
                }
            }
        }
        #endregion
    }
    #endregion
}
