using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Utilities.Mesh_Utilities;
using UnityEngine;

namespace Thanking.Utilities
{
    public static class SphereUtilities
    {
        public static Vector3 Get(GameObject go, Vector3 pos, int mask)
        {
            Mesh mesh = go.GetComponent<MeshCollider>().sharedMesh;
            VertTriList vt = new VertTriList(mesh);
            Vector3[] verts = mesh.vertices.Where(v => !Physics.Raycast(pos, (go.transform.TransformPoint(v) - pos).normalized, Vector3.Distance(pos, go.transform.TransformPoint(v)) + 0.5f, mask)).ToArray();

            if (verts != null && verts.Length != 0)
                return go.transform.TransformPoint(verts
                    .OrderBy(v => Vector3.Distance(go.transform.TransformPoint(v), pos)).First());
            
            return Vector3.zero;
        }
    }
}
