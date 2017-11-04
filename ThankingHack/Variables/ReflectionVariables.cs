using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Thanking.Variables
{
	public static class ReflectionVariables
	{
		public static BindingFlags PublicInstance = BindingFlags.Public | BindingFlags.Instance;
		public static BindingFlags PrivateInstance = BindingFlags.NonPublic | BindingFlags.Instance;
		public static BindingFlags PublicStatic = BindingFlags.Public | BindingFlags.Static;
		public static BindingFlags PrivateStatic = BindingFlags.NonPublic | BindingFlags.Static;
		public static BindingFlags Everything = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
	}
}
