using System;
using System.Threading;
using Thanking.Managers.Main;
using UnityEngine;

namespace Thanking
{
    public static class Loader
    {
        public static GameObject HookObject;

        public static void Hook()
        {
			Debug.Log("Initializing Thanking...");
			#region Unity
			HookObject = new GameObject();
			UnityEngine.Object.DontDestroyOnLoad(HookObject);
			#endregion

			#region Manager Initialization
			ConfigManager.Init();
			AttributeManager.Init();
			AssetManager.Init();
			#endregion
			Debug.Log("Thanking initialized!");
		}

        public static void HookThread()
        {
            try
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(2000);
                    if (HookObject == null)
                    {
                        Debug.Log("Initializing Thanking...");
                        #region Unity
                        HookObject = new GameObject();
                        UnityEngine.Object.DontDestroyOnLoad(HookObject);
                        #endregion

                        #region Manager Initialization
                        ConfigManager.Init();
                        AttributeManager.Init();
                        AssetManager.Init();
                        #endregion
                        Debug.Log("Thanking initialized!");
                    }
                    System.Threading.Thread.Sleep(5000);
                }
            }
            catch (Exception ex) { UnityEngine.Debug.LogException(ex); }
        }

        public static void Thread()
        {
            Thread thread = new System.Threading.Thread(new ThreadStart(Hook));
            thread.Start();
        }
    }
}
