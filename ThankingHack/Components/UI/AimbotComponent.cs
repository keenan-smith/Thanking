using Thanking.Attributes;
using Thanking.Components.Basic;
using Thanking.Coroutines;
using Thanking.Options.AimOptions;
using UnityEngine;

namespace Thanking.Components
{
    [Component]
    public class AimbotComponent : MonoBehaviour
    {
        public void Start()
        {
            CoroutineComponent.LockCoroutine = StartCoroutine(AimbotCoroutines.SetLockedObject());
            CoroutineComponent.AimbotCoroutine = StartCoroutine(AimbotCoroutines.AimToObject());
        }
    }
}
