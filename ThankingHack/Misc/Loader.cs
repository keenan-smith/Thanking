using System;
using System.Threading;
using Thanking.Managers.Main;
using Thanking.Misc;
using UnityEngine;

namespace Thanking
{
    public static class Loader
    {
        public static GameObject HookObject;

        public static void Hook()
        {
			Debug.Log("Initializing Thanking...");
            
			HookObject = new GameObject();
			UnityEngine.Object.DontDestroyOnLoad(HookObject);

			ConfigManager.Init();
			AttributeManager.Init();
			AssetManager.Init();
			Profiler.Init();
			Debug.Log("Thanking initialized!");
		}

        public static void HookThread()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(2000);

                if (HookObject == null)
                    Hook();

                System.Threading.Thread.Sleep(5000);
            }
        }

        public static void Thread()
        {
            Thread thread = new Thread(HookThread);
            thread.Start();
        }
    }
}
