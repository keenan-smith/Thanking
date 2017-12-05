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
        public Vector3 Velocity = Vector3.zero;
        public GameObject Sphere;
        public float Speed = -1;
        public float Radius = -1;

        void Start()
        {
            StartCoroutine(CalcVelocity());
            StartCoroutine(CalcRadius());
        }
  
        IEnumerator CalcVelocity()
        {
            while(true)
            {
                prevPos = transform.position;

                yield return new WaitForSeconds(0.5f);

                Velocity = (prevPos - transform.position) * 2;
                Speed = (float)VectorUtilities.GetMagnitude(Velocity);
                
                Debug.Log(Speed);
            }
        }

        IEnumerator CalcRadius()
        {
            SetRadius();
            Sphere = IcoSphere.Create("HitSphere", Radius, SphereOptions.RecursionLevel);
            SetUpSphere();
            
            Debug.Log(Radius);
            
            while (true)
            {
                yield return new WaitForSeconds(1);
                
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
                Radius = 15.8f - Speed * Provider.ping * 1.1f;
        }

        void SetUpSphere()
        {
            Sphere.transform.parent = transform;
            Sphere.transform.localPosition = Vector3.zero;
            Sphere.layer = LayerMasks.AGENT;
        }
    }
}