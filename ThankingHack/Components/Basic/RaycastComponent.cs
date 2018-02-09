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
        public GameObject Sphere;
        public float Speed = -1;
        public float Radius = SphereOptions.SphereRadius;

        void Awake()
        {
            StartCoroutine(CalcVelocity());
            StartCoroutine(CalcSphere());
        }

        private void FixedUpdate() =>
            Sphere.transform.position = transform.position;
        
        IEnumerator CalcVelocity()
        {
            while(true)
            {
                prevPos = transform.position;
                yield return new WaitForSeconds(0.5f);
                
                Velocity = (transform.position - prevPos) * 2;
                Speed = (float) VectorUtilities.GetMagnitude(Velocity);
            }
        }

        IEnumerator CalcSphere()
        {
            SetRadius();
            Sphere = IcoSphere.Create("HitSphere", Radius, SphereOptions.RecursionLevel);
            Sphere.layer = LayerMasks.AGENT;
            
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                
                Destroy(Sphere);
                Sphere = IcoSphere.Create("HitSphere", Radius, SphereOptions.RecursionLevel);
                Sphere.layer = LayerMasks.AGENT;
                SetRadius();
            }
        }

        void SetRadius()
        {
            Speed = SphereOptions.DynamicSphere ? Speed : -1;
            Radius = SphereOptions.SphereRadius;

            if (Speed > 0)
                Radius = 15.5f - Speed * Provider.ping * 1.3f;
        }
    }
}