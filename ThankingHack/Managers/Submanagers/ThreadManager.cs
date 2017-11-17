using System;
using System.Collections.Generic;
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
		/// <summary>
		/// Collect and start methods marked with ThreadAttribute in their own threads
		/// </summary>
		public static void Load()
		{
			#if DEBUG
			DebugUtilities.Log("Initializing ThreadManager");
			#endif
			
			IEnumerable<Type> Types = Assembly.GetExecutingAssembly().GetTypes().Where(T => T.IsClass);

			foreach(Type T in Types)
			{
				IEnumerable<MethodInfo> Methods = T.GetMethods(ReflectionVariables.Everything)
					.Where(M => M.IsDefined(typeof(ThreadAttribute), false));

				foreach(MethodInfo M in Methods)
				{
					Action ThreadAction = (Action) Delegate.CreateDelegate(typeof(Action), M);
					new Thread(new ThreadStart(ThreadAction)).Start();
				}
			}
		}
	}
}
