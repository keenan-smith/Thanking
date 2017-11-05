using Harmony;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Misc
{
	public static class Profiler
	{
		public static Dictionary<Type, TimeSpan> ProfiledUpdateTime = new Dictionary<Type, TimeSpan>();
		public static Dictionary<Type, TimeSpan> ProfiledFixedUpdateTime = new Dictionary<Type, TimeSpan>();
		public static HarmonyInstance Harmony;

		public static void Prefix(ref DateTime __state) =>
			__state = DateTime.Now;

		public static void UpdatePostfix(object __instance, DateTime __state)
		{
			if (ProfiledUpdateTime.ContainsKey(__instance.GetType()))
				ProfiledUpdateTime[__instance.GetType()] = DateTime.Now - __state;
			else
				ProfiledUpdateTime.Add(__instance.GetType(), DateTime.Now - __state);
		}

		public static void FixedUpdatePostfix(object __instance, DateTime __state)
		{
			if (ProfiledFixedUpdateTime.ContainsKey(__instance.GetType()))
				ProfiledFixedUpdateTime[__instance.GetType()] = DateTime.Now - __state;
			else
				ProfiledFixedUpdateTime.Add(__instance.GetType(), DateTime.Now - __state);
		}

		public static void Init() =>
			Harmony = HarmonyInstance.Create("com.thanking.hack");

		public static void Start()
		{
			MethodInfo Prefix = typeof(Profiler).GetMethod("Prefix");

			MethodInfo UpdatePostfix = typeof(Profiler).GetMethod("UpdatePostfix");
			MethodInfo FixedUpdatePostfix = typeof(Profiler).GetMethod("FixedUpdatePostfix");

			foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
				foreach (Type typ in asm.GetTypes())
					foreach (MethodInfo meth in typ.GetMethods(ReflectionVariables.Everything))
						switch (meth.Name)
						{
							case "Update":
								Harmony.Patch(meth, new HarmonyMethod(Prefix), new HarmonyMethod(UpdatePostfix));
								break;
							case "FixedUpdate":
								Harmony.Patch(meth, new HarmonyMethod(Prefix), new HarmonyMethod(FixedUpdatePostfix));
								break;
						}
		}

		public static void Dump()
		{
			foreach (Type t in ProfiledUpdateTime.Keys.OrderBy(o => ProfiledUpdateTime[o].TotalMilliseconds))
				File.AppendAllText("Update.time", t.Name + " - " + ProfiledUpdateTime[t].TotalMilliseconds.ToString());

			foreach (Type t in ProfiledFixedUpdateTime.Keys.OrderBy(o => ProfiledFixedUpdateTime[o].TotalMilliseconds))
				File.AppendAllText("FixedUpdate.time", t.Name + " - " + ProfiledFixedUpdateTime[t].TotalMilliseconds.ToString());

			Process.GetCurrentProcess().Kill();
		}
	}
}
