using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Thanking.Attributes;
using UnityEngine;

namespace Thanking.Threads
{
	[Thread("Start")]
    public static class LoaderThread
    {
        public static void Start()
        {
            while (true)
            {
                if (Loader.HookObject == null)
                    Loader.Hook();

                Thread.Sleep(2000);
            }
        }
    }
}
