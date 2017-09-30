using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Thanking.Managers
{
    public static class OverrideManager
    {
        public static void OverrideClass(Type basetype, Type overrideType)
        {
            for (int i = 0; i < basetype.GetMethods().Length; i++)
            {
                MethodInfo mi = basetype.GetMethods()[i];
                
                MethodInfo o_mi = overrideType.GetMethod("OV_" + mi.Name);

                if (o_mi == null)
                    continue;

                RuntimeHelpers.PrepareMethod(mi.MethodHandle);
                RuntimeHelpers.PrepareMethod(o_mi.MethodHandle);

                IntPtr ptrOriginal = mi.MethodHandle.GetFunctionPointer();
                IntPtr ptrModified = o_mi.MethodHandle.GetFunctionPointer();

                switch (IntPtr.Size)
                {
                    case sizeof(Int32):
                        unsafe
                        {
                            Debug.LogWarning("Detouring " + ptrOriginal + " to " + ptrModified + "...");
                            byte* ptrFrom = (byte*) ptrOriginal.ToPointer();

                            *ptrFrom = 0x68; // PUSH
                            *((uint*) (ptrFrom + 1)) = (uint) ptrModified.ToInt32(); // Pointer
                            *(ptrFrom + 5) = 0xC3; // RETN
                        }
                        break;
                    case sizeof(Int64):
                        unsafe
                        {
                            byte* ptrFrom = (byte*) ptrOriginal.ToPointer();

                            *ptrFrom = 0x48; // REX.W
                            *(ptrFrom + 1) = 0xB8; // MOV
                            *((ulong*) (ptrFrom + 2)) = (ulong) ptrModified.ToInt64(); // Pointer
                            *(ptrFrom + 10) = 0xFF; // INC 1
                            *(ptrFrom + 11) = 0xE0; // LOOPE
                        }
                        break;
                }
            }
        }
    }
}
