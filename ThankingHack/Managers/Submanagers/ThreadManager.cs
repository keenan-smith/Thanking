﻿using System;
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
					if (tClass.IsClass)
						if (tClass.IsDefined(typeof(ThreadAttribute), false))
						{
							ThreadAttribute classAttribute = Attribute.GetCustomAttribute(tClass, typeof(ThreadAttribute)) as ThreadAttribute;
							MethodInfo ThreadStart = tClass.GetMethod(classAttribute.StartMethod, ReflectionVariables.Everything);
							Action ThreadAction = (Action)Delegate.CreateDelegate(typeof(Action), ThreadStart);
							new Thread(new ThreadStart(ThreadAction)).Start();
						}

		}
	}
}
