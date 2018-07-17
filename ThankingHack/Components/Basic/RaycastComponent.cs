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
        public GameObject Sphere;    
        public float Speed = -1;
        public float Radius = SphereOptions.SphereRadius;

        void Awake()
        {
            StartCoroutine(CalcVelocity());
            StartCoroutine(CalcSphere());
        }
        
        IEnumerator CalcVelocity()
        {
            while(true)
            {
                Vector3 p1 = transform.position;
                yield return new WaitForSeconds(0.1f);
                Vector3 p2 = transform.position;
                double d1 = VectorUtilities.GetDistance(transform.position, p1);
                if (d1 > 100)
                    d1 = 0;
                yield return new WaitForSeconds(0.1f);
                Vector3 p3 = transform.position;
                double d2 = VectorUtilities.GetDistance(transform.position, p2);
                if (d2 > 100)
                    d2 = 0;
                yield return new WaitForSeconds(0.1f);
                Vector3 p4 = transform.position;
                double d3 = VectorUtilities.GetDistance(transform.position, p3);
                if (d3 > 100)
                    d3 = 0;
                yield return new WaitForSeconds(0.1f);
                
                double d4 = VectorUtilities.GetDistance(transform.position, p4);
                if (d4 > 100)
                    d4 = 0;

                Speed = (float) ((d1 + d2 + d3 + d4) / 4 * 10);
            }
        }

        IEnumerator CalcSphere()
        {
            while (true)
            {
                Destroy(Sphere);
                
                Sphere = IcoSphere.Create("HitSphere", Radius, SphereOptions.RecursionLevel);
                Sphere.layer = LayerMasks.AGENT;
                Sphere.transform.parent = transform;
                Sphere.transform.localPosition = new Vector3(0, 0, 0);
                
                SetRadius();
                
                yield return new WaitForSeconds(0.25f);
            }
        }

        void SetRadius()
        {
            Speed = SphereOptions.DynamicSphere ? Speed : -1;
            Radius = SphereOptions.SphereRadius;

            float Calculated = Speed * Provider.ping * 1.3f;

            if (Mathf.Abs(Calculated) > 15f)
                Radius = 1;
            
            else if (Speed > 0)
                Radius = 15.5f - Calculated;
        }
    }
}