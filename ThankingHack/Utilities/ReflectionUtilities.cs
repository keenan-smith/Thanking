using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Thanking.Utilities
{
	public static class ReflectionUtilities
	{
		public static MethodInfo GetPrivateFunction(this object ptr, string name) =>
			ptr.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);

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
					return BindingFlags.NonPublic | BindingFlags.Static;
				case FieldType.Public:
					return BindingFlags.Instance | BindingFlags.Public;
				case FieldType.PublicStatic:
					return BindingFlags.Instance | BindingFlags.Public;
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
