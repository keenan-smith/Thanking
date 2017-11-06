using System;
using System.Linq;
using System.Reflection;
using Thanking.Attributes;

namespace Thanking.Managers.Submanagers
{
	public static class InitializationManager
	{
		public static void Load()
		{
			Type[] Types = Assembly.GetExecutingAssembly().GetTypes().Where(T => T.IsClass).ToArray();

			for (int i = 0; i < Types.Length; i++)
			{
				MethodInfo[] Methods = Types[i].GetMethods().Where(M => M.IsDefined(typeof(InitializerAttribute), false))
					.ToArray();

				for (int o = 0; o < Methods.Length; o++)
					Methods[o].Invoke(null, null);
			}
		}
	}
}
