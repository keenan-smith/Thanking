using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Thanking.Utilities
{
    public static class ReflectionManager
    {
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

        public static BindingFlags GetBindingFlags(FieldType fieldType) =>
            (BindingFlags) fieldType;

        public enum FieldType
        {
            Private =  BindingFlags.Instance | BindingFlags.NonPublic,
            PrivateStatic = BindingFlags.NonPublic | BindingFlags.Static,
            Public = BindingFlags.Instance | BindingFlags.Public,
            PublicStatic = BindingFlags.Public | BindingFlags.Static
        }
    }
}