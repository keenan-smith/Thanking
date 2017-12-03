using System;
using System.Collections;
using System.Collections.Generic;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.Basic
{
    [DisallowMultipleComponent]
    public class VelocityComponent : MonoBehaviour
    {
        private Vector3 prevPos = Vector3.zero;
        public Vector3 Velocity = Vector3.zero;
        public double Speed = -1;       
        
        void Start() =>
            StartCoroutine(CalcVelocity());
  
        IEnumerator CalcVelocity()
        {
            while(true)
            {
                prevPos = transform.position;

                yield return new WaitForFixedUpdate();

                Velocity = (prevPos - transform.position) / Time.fixedDeltaTime;
                Speed = VectorUtilities.GetMagnitude(Velocity);
            }
        }

    }
}