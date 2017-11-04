using System;
using System.Reflection;
using Thanking.Attributes;
using Thanking.Variables;

namespace Thanking.Managers.Submanagers
{
	public static class InitializationManager
	{
		public static void Load()
		{
			foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
				foreach (Type tClass in asm.GetTypes())
					if (tClass.IsClass)
						foreach (MethodInfo method in tClass.GetMethods(ReflectionVariables.Everything))
							if (method.IsDefined(typeof(InitializerAttribute), false))
								method.Invoke(null, null);
		}
	}
}
