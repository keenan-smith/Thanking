using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
