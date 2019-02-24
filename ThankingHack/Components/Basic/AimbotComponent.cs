using Thanking.Attributes;
using Thanking.Coroutines;
using UnityEngine;

namespace Thanking.Components.Basic
{
	/// <summary>
	/// Component used to start aimbot coroutines and check hotkeys if necessary
	/// </summary>
	[Component]
	public class AimbotComponent : MonoBehaviour 
	{
		public void Start()
		{
			CoroutineComponent.LockCoroutine = StartCoroutine(AimbotCoroutines.SetLockedObject());
			CoroutineComponent.AimbotCoroutine = StartCoroutine(AimbotCoroutines.AimToObject());

			StartCoroutine(RaycastCoroutines.UpdateObjects());
		}
	}
}