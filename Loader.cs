using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Thanking.Managers;
using Thanking.Threads;
using UnityEngine;

namespace Thanking
{
    public static class Loader
    {
        public static GameObject HookObject;

        public static void Hook()
        {
            HookObject = new GameObject();
			UnityEngine.Object.DontDestroyOnLoad(HookObject);
            AttributeManager.Init();
			AssetManager.Init();
        }
    }
}
