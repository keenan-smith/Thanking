using LOADING;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Thanking.Threads
{
    public class LoaderThread
    {
        public static void Start()
        {
            while (true)
            {
                if (LOADING.Loader.HookObject == null)
                    LOADING.Loader.Hook();

                Thread.Sleep(2000);
            }
        }
    }
}
