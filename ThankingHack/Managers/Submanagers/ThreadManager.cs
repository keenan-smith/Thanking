using System;
using System.Reflection;
using System.Threading;
using Thanking.Attributes;
using Thanking.Utilities;
using Thanking.Variables;

namespace Thanking.Managers.Submanagers
{
	public static class ThreadManager
	{
		public static void Load()
		{
			foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
				foreach (Type tClass in asm.GetTypes())
					foreach (MethodInfo tMethod in tClass.GetMethods(ReflectionVariables.Everything))
						if (tMethod.IsDefined(typeof(ThreadAttribute), false))
						{
							Action ThreadAction = (Action)Delegate.CreateDelegate(typeof(Action), tMethod);
							new Thread(new ThreadStart(ThreadAction)).Start();
						}

		}
	}
}
