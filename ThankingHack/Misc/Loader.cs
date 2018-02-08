using System.Threading;
using Thanking.Managers.Main;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking
{
    public static class Loader
    {
        public static GameObject HookObject;

        public static bool HasRun = false;
        
        public static void Hook()
        {
            if (HasRun)
                return;

            #if DEBUG
            DebugUtilities.Init();
			DebugUtilities.Log("Initializing Thanking...");
            #endif        
    
			HookObject = new GameObject();
			Object.DontDestroyOnLoad(HookObject);
            
			ConfigManager.Init();
		    AttributeManager.Init();
			AssetManager.Init();
            
            #if DEBUG
			DebugUtilities.Log("Thanking initialized!");
            #endif

            HasRun = true;
		}

        public static void HookThread()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(4000);

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
