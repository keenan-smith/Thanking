using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Thanking.Attributes;
using Thanking.Variables;

namespace Thanking.Managers
{
	public static class ThreadManager
	{
		public static void Load()
		{
			foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
				foreach (Type tClass in asm.GetTypes())
					if (tClass.IsClass)
						if (tClass.IsDefined(typeof(ThreadAttribute), false))
						{
							ThreadAttribute classAttribute = Attribute.GetCustomAttribute(tClass, typeof(ThreadAttribute)) as ThreadAttribute;
							MethodInfo ThreadStart = tClass.GetMethod(classAttribute.StartMethod, BFlags.Everything);
							Action ThreadAction = (Action)Delegate.CreateDelegate(typeof(Action), ThreadStart);
							new Thread(new ThreadStart(ThreadAction)).Start();
						}

		}
	}
}
