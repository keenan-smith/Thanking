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
                Sphere.transform.localPosition = gameObject.GetComponent<Rigidbody>().velocity * Provider.ping * 2;
            }
        }

        IEnumerator RedoSphere()
        {
            while (true)
            {
                Destroy(Sphere);
                
                Sphere = IcoSphere.Create("HitSphere", SphereOptions.SpherePrediction ? 15.5f : SphereOptions.SphereRadius, SphereOptions.RecursionLevel);
                Sphere.layer = LayerMasks.AGENT;
                Sphere.transform.parent = transform;
                Sphere.transform.localPosition = new Vector3(0, 0, 0);
                
                yield return new WaitForSeconds(1);
            }
        }
    }
}