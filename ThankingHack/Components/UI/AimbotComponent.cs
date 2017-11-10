using Thanking.Attributes;
using Thanking.Components.Basic;
using Thanking.Coroutines;
using UnityEngine;

namespace Thanking.Components
{
    [Component]
    public class AimbotComponent : MonoBehaviour
    {
        public void Start()
        {
            CoroutineComponent.AimbotCoroutine = StartCoroutine(AimbotCoroutines.SetLockedObject());
        }
    }
}
