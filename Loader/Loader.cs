using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Thanking.Threads;
using UnityEngine;

namespace LOADING
{
    public class Loader : MonoBehaviour
    {
        public static GameObject HookObject;
        public static void thread()
        {
            try
            {
                Thread th = new Thread(new ThreadStart(LoaderThread.Start));
                th.Start();
            }
            catch (Exception x) { Debug.Log("ERROR START\n" + x + "\nERROR END"); }
        }

        public static void Hook()
        {

        }
    }
}
