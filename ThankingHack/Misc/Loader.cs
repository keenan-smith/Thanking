using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Thanking.Managers;
using Thanking.Managers.Main;
using Thanking.Threads;
using Thanking.Variables;
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
    }
}
