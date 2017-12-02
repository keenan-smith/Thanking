using Thanking.Attributes;
using Thanking.Coroutines;
using UnityEngine;

namespace Thanking.Components.Basic
{
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