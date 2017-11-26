using SDG.Unturned;
using System.Collections.Generic;
using Thanking.Options.AimOptions;
using Thanking.Utilities.Mesh_Utilities;
using UnityEngine;

namespace Thanking.Utilities
{
    public static class SphereUtilities
    {
		public static Vector3 Get(Player Player, Vector3 pos, int mask)
		{
			GameObject go = IcoSphere.Create("HitSphere",
					Player.movement.getVehicle() != null ? SphereOptions.VehicleSphereRadius : SphereOptions.SphereRadius,
					SphereOptions.RecursionLevel);

			go.transform.parent = Player.transform;
			go.transform.localPosition = new Vector3(0, 0, 0);

			Vector3 vec = Get(go, pos, mask);
			Object.Destroy(go);

			return vec;
		}
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
