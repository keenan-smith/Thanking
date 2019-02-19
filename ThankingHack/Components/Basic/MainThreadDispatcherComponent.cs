using System;
using System.Collections.Generic;
using Thinking.Attributes;
using UnityEngine;

namespace Thinking.Components.Basic
{
    /// <summary>
    /// Main thread dispatcher component; invokes a method either once or repeatedly in the main thread.
    /// </summary>
    [Component]
    public class MainThreadDispatcherComponent : MonoBehaviour
    {
        private static readonly Queue<Action> MethodQueue = new Queue<Action>();
        
        /// <summary>
        /// Invoke all methods in the method queue in the main update loop.
        /// </summary>
        public void Update()
        {
            lock(MethodQueue) 
                while (MethodQueue.Count > 0)
                    MethodQueue.Dequeue()();
        }

        public static void InvokeOnMain(Action a)
        {
            lock (MethodQueue)
                MethodQueue.Enqueue(a);
        }
    }
}