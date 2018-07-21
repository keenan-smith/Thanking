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
        
        void Awake()
        {
            StartCoroutine(RedoSphere());
            StartCoroutine(CalcSphere());
        }

        IEnumerator CalcSphere()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                if (Sphere)
                {
                    Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                    
                    if (rb)
                        Sphere.transform.localPosition = rb.velocity * Provider.ping * 2;
                    else
                        Sphere.transform.localPosition = Vector3.zero;
                }
            }
        }

        IEnumerator RedoSphere()
        {
            while (true)
            {
                GameObject tmp = Sphere;
                
                Sphere = IcoSphere.Create("HitSphere", SphereOptions.SpherePrediction ? 15f : SphereOptions.SphereRadius, SphereOptions.RecursionLevel);
                Sphere.layer = LayerMasks.AGENT;
                Sphere.transform.parent = transform;
                Sphere.transform.localPosition = Vector3.zero;
                
                Destroy(tmp);
                
                yield return new WaitForSeconds(1);
            }
        }
    }
}