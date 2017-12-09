using System;
using System.Collections;
using System.Collections.Generic;
using SDG.Unturned;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using Thanking.Utilities.Mesh_Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Thanking.Components.Basic
{
    [DisallowMultipleComponent]
    public class RaycastComponent : MonoBehaviour
    {
        private Vector3 prevPos = Vector3.zero;
        private Vector3 Velocity = Vector3.zero;
        private Vector3 PredictedVelocity = Vector3.zero;
        public GameObject Sphere;
        public float Speed = -1;
        public float Radius = -1;

        void Awake()
        {
            StartCoroutine(CalcSphere());
            StartCoroutine(CalcVelocity());
        }
  
        IEnumerator CalcVelocity()
        {
            while(true)
            {
                prevPos = transform.position;
                yield return new WaitForSeconds(0.25f);
                
                Velocity = (transform.position - prevPos) * 4;
                Speed = (float)VectorUtilities.GetMagnitude(Velocity);
                
                Sphere.transform.localPosition = PredictedVelocity;
            }
        }

        IEnumerator CalcSphere()
        {
            SetRadius();
            Sphere = IcoSphere.Create("HitSphere", Radius, SphereOptions.RecursionLevel);
            SetUpSphere();
            
            while (true)
            {
                yield return new WaitForSeconds(0.25f);
                
                SetRadius();
                Destroy(Sphere);
                Sphere = IcoSphere.Create("HitSphere", Radius, SphereOptions.RecursionLevel);
                SetUpSphere();
            }
        }

        void SetRadius()
        {
            Speed = SphereOptions.DynamicSphere ? Speed : -1;
            Radius = SphereOptions.SphereRadius;

            if (Speed > 0)
            {
                PredictedVelocity = VectorUtilities.Normalize(Velocity) * Speed * Provider.ping;
                float MarginOfError = Provider.ping * 1.1f;
                
                Radius = 15.8f - MarginOfError;
            }
        }

        void SetUpSphere()
        {
            Sphere.transform.parent = transform;
            Sphere.layer = LayerMasks.AGENT;
        }
    }
}