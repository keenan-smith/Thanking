using Thanking.Attributes;
using UnityEngine;

namespace Thanking.Components.Basic
{
	/// <summary>
	/// Handles the storage of coroutines so that they can be stopped/started later on
	/// </summary>
	[Component]
	public class CoroutineComponent : MonoBehaviour
	{
		public static Coroutine ESPCoroutine;
        public static Coroutine ChamsCoroutine;
        public static Coroutine ItemPickupCoroutine;
        public static Coroutine AimbotCoroutine;
        public static Coroutine LockCoroutine;
        public static Coroutine TrajectoryCoroutine;
    }
}
