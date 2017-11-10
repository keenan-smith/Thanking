using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
