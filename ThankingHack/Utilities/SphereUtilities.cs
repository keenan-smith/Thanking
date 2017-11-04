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
			Vector3[] verts = mesh.vertices;

			List<Vector3> nVerts = new List<Vector3>();

			for (int i = 0; i < verts.Length; i++)
				if (!Physics.Raycast(pos, (go.transform.TransformPoint(verts[i]) - pos).normalized, (float)(VectorUtilities.GetDistance(pos, go.transform.TransformPoint(verts[i])) + 0.5), mask))
					return go.transform.TransformPoint(verts[i]);
			
            return Vector3.zero;
        }
    }
}
