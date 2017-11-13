using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Thanking.Utilities
{
    public static class ReflectionManager
    {
        public static T GetPrivateField<T>(this object ptr, string var)
        {
            return (T)ptr.GetType().GetField(var, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ptr);
        }

        public static MethodInfo GetPrivateFunction(this object ptr, string name)
        {
            return ptr.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static object CallPrivateFunction(this object ptr, string name)
        {
            return ptr.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(null, new object[0]);
        }

        public static object CallPrivateFunction(this object ptr, string name, params object[] parameters)
        {
            return ptr.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(null, new object[] { parameters });
        }

        public static void SetPrivateField<T>(this object ptr, string name, T Value)
        {
            ptr.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(ptr, Value);
        }

        public static T GetField<T>(this object Pointer, string Variable, FieldType fieldType)
        {
            BindingFlags bindingFlags = GetBindingFlags(fieldType);
            return (T)(Pointer.GetType().GetField(Variable, bindingFlags).GetValue(Pointer));
        }
        public static void SetField<T>(this object Pointer, string Variable, FieldType fieldType, T Value)
        {
            BindingFlags bindingFlags = GetBindingFlags(fieldType);
            Pointer.GetType().GetField(Variable, bindingFlags).SetValue(Pointer, Value);
        }
        public static BindingFlags GetBindingFlags(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.Private:
                    return BindingFlags.Instance | BindingFlags.NonPublic;
                case FieldType.PrivateStatic:
                    return BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
                case FieldType.Public:
                    return BindingFlags.Instance | BindingFlags.Public;
                case FieldType.PublicStatic:
                    return BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;
                default:
                    return BindingFlags.Instance;
            }
        }

        public enum FieldType
        {
            Private,
            PrivateStatic,
            Public,
            PublicStatic
        }
    }
}