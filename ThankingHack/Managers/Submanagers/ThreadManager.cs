using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Thanking.Attributes;
using Thanking.Variables;
using Thanking.Utilities;

namespace Thanking.Managers.Submanagers
{
	public static class ThreadManager
	{
		public static void Load()
		{
			#if DEBUG
			DebugUtilities.Log("Initializing ThreadManager");
			#endif
			
			Type[] Types = Assembly.GetExecutingAssembly().GetTypes().Where(T => T.IsClass).ToArray();

			for (int i = 0; i < Types.Length; i++)
			{
				MethodInfo[] Methods = Types[i].GetMethods(ReflectionVariables.Everything).Where(M => M.IsDefined(typeof(ThreadAttribute), false))
					.ToArray();

				for (int o = 0; o < Methods.Length; o++)
				{
					Action ThreadAction = (Action) Delegate.CreateDelegate(typeof(Action), Methods[o]);
					new Thread(new ThreadStart(ThreadAction)).Start();
				}
			}
		}
	}
}
