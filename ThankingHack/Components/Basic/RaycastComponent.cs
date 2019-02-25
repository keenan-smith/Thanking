using System.Collections;
using SDG.Unturned;
using Thanking.Options.AimOptions;
using Thanking.Utilities.Mesh_Utilities;
using UnityEngine;

namespace Thanking.Components.Basic
{
    [DisallowMultipleComponent]
    public class RaycastComponent : MonoBehaviour
    {
        public GameObject Sphere;
       // public Vector3 PointBias;
        
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
                    {
                        float sizeBias = 1 - Provider.ping * rb.velocity.magnitude * 2;
                        Sphere.transform.localScale = new Vector3(sizeBias, sizeBias, sizeBias);
                    }
                }
            }
        }

        IEnumerator RedoSphere()
        {
            while (true)
            {
                GameObject tmp = Sphere;
                
                Sphere = IcoSphere.Create("HitSphere", SphereOptions.SpherePrediction ? 15.5f : SphereOptions.SphereRadius, SphereOptions.RecursionLevel);
                Sphere.layer = LayerMasks.AGENT;
                Sphere.transform.parent = transform;
                Sphere.transform.localPosition = Vector3.zero;
                
                Destroy(tmp);
                
                yield return new WaitForSeconds(1f);
            }
        }
    }
}