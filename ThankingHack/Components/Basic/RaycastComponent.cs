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
        public Vector3 lPos;
        public float LSpeed = 0;
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
                lPos = transform.position;
                yield return new WaitForSeconds(0.25f);
                double d = VectorUtilities.GetDistance(transform.position, lPos);
                
                if (d  > 100)
                    d = 0;

                Speed = (float) (d * 4 + LSpeed) / 2;
                LSpeed = Speed;
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
            float CSpeed = SphereOptions.DynamicSphere ? Speed : -1;
            Radius = SphereOptions.SphereRadius;
            
            if (CSpeed > 0) {
                float Calculated = CSpeed * Provider.ping * 2.5f;
                Radius = 15.5f - Calculated;
            }
        }
    }
}