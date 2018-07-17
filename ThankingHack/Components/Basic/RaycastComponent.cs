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
                yield return new WaitForSeconds(0.25f);

                Speed = (float) VectorUtilities.GetDistance(prevPos, transform.position) * 4;
            }
        }

        IEnumerator CalcSphere()
        {
            while (true)
            {
                Destroy(Sphere);
                
                Sphere = IcoSphere.Create("HitSphere", Radius, SphereOptions.RecursionLevel);
                Sphere.layer = LayerMasks.AGENT;
                SetRadius();
                
                yield return new WaitForSeconds(0.25f);
            }
        }

        void SetRadius()
        {
            Speed = SphereOptions.DynamicSphere ? Speed : -1;
            Radius = SphereOptions.SphereRadius;

            float Calculated = Speed * Provider.ping * 1.3f;

            if (Calculated > 15)
                Radius = 1;
            
            else if (Speed > 0)
                Radius = 15.5f - Calculated;
        }
    }
}