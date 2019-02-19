using System;
using System.Threading;
using Thinking.Managers.Main;
using Thinking.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Thnkng
{
    public static class Ldr
    {
        public static GameObject HookObject;
        
        public static void Hook()
        {
           // #if DEBUG
            DebugUtilities.Init();
			DebugUtilities.Log("Initializing Thinking...");
           // #endif        
    
			HookObject = new GameObject();
			Object.DontDestroyOnLoad(HookObject);
            try
            {
                AttributeManager.Init();
                AssetManager.Init();
                ConfigManager.Init();
            }
            catch (Exception e)
            {
                DebugUtilities.LogException(e);
            }
            //#if DEBUG
			DebugUtilities.Log("Thanking initialized!");
           // #endif
		}

        public static void HookThread()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(10000);

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
