using Thanking.Attributes;
using Thanking.Coroutines;
using UnityEngine;

namespace Thanking.Components.Basic
{
    [Component]
    public class ChamsComponent : MonoBehaviour
    {
        public void Start()
        {
            CoroutineComponent.ChamsCoroutine = StartCoroutine(ESPCoroutines.DoChams());
        }
    }
}
