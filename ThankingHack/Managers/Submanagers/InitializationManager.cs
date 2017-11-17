using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Thanking.Attributes;
using Thanking.Variables;

namespace Thanking.Managers.Submanagers
{
	public static class InitializationManager
	{
		/// <summary>
		/// Collect and invoked methods marked with InitializationAttribute, as this indicates they are essential to the
		/// initialization of :Thanking:
		/// </summary>
		public static void Load()
		{
			#if DEBUG
			DebugUtilities.Log("Initializing InitializationManager");
			#endif
			
			IEnumerable<Type> Types = Assembly.GetExecutingAssembly().GetTypes().Where(T => T.IsClass);

			foreach (Type T in Types)
			{
				// Only check for static variables because we cannot invoke instantiated methods
				IEnumerable<MethodInfo> Methods = T.GetMethods(ReflectionVariables.PublicStatic)
					.Where(M => M.IsDefined(typeof(InitializerAttribute), false));

				foreach(MethodInfo M in Methods)
					M.Invoke(null, null);
			}
		}
	}
}
