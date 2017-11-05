using System.Threading;
using Thanking.Managers.Main;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking
{
    public static class Loader
    {
        public static GameObject HookObject;

        public static void Hook()
        {
            #if DEBUG
			DebugUtilities.Log("Initializing Thanking...");
            #endif        
    
			HookObject = new GameObject();
			Object.DontDestroyOnLoad(HookObject);

            DebugUtilities.Init();
			ConfigManager.Init();
			AttributeManager.Init();
			AssetManager.Init();
            
            #if DEBUG
			DebugUtilities.Log("Thanking initialized!");
            #endif
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
