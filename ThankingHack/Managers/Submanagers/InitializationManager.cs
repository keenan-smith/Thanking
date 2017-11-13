using System;
using System.Linq;
using System.Reflection;
using Thanking.Attributes;
using Thanking.Variables;

namespace Thanking.Managers.Submanagers
{
	public static class InitializationManager
	{
		public static void Load()
		{
			#if DEBUG
			DebugUtilities.Log("Initializing InitializationManager");
			#endif
			
			Type[] Types = Assembly.GetExecutingAssembly().GetTypes().Where(T => T.IsClass).ToArray();

			for (int i = 0; i < Types.Length; i++)
			{
				MethodInfo[] Methods = Types[i].GetMethods(ReflectionVariables.Everything).Where(M => M.IsDefined(typeof(InitializerAttribute), false))
					.ToArray();

				for (int o = 0; o < Methods.Length; o++)
					Methods[o].Invoke(null, null);
			}
		}
	}
}
