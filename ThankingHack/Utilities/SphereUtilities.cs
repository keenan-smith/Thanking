using SDG.Unturned;
using System.Collections.Generic;
using Thanking.Options.AimOptions;
using Thanking.Utilities.Mesh_Utilities;
using UnityEngine;

namespace Thanking.Utilities
{
    public static class SphereUtilities
    {
		public static RaycastHit Get(Player Player, Vector3 pos)
		{
			GameObject go = IcoSphere.Create("HitSphere",
					Player.movement.getVehicle() != null ? SphereOptions.VehicleSphereRadius : SphereOptions.SphereRadius,
					SphereOptions.RecursionLevel);

			go.transform.parent = Player.transform;
			go.transform.localPosition = new Vector3(0, 0, 0);
			go.layer = RayMasks.ENEMY;

			RaycastHit vec = Get(go, pos, RayMasks.ENEMY);
			Object.Destroy(go);

			return vec;
		}

		public static RaycastHit Get(GameObject obj, Vector3 pos, float radius)
		{
			int OrigLayer = obj.layer;
			GameObject go = IcoSphere.Create("HitSphere", radius, SphereOptions.RecursionLevel);

			go.transform.parent = obj.transform;
			go.transform.localPosition = new Vector3(0, 0, 0);
			go.layer = RayMasks.ENEMY;
			obj.layer = RayMasks.ENEMY;

			RaycastHit vec = Get(go, pos, RayMasks.ENEMY);
			Object.Destroy(go);
			obj.layer = OrigLayer;

			return vec;
		}

        public static RaycastHit Get(GameObject go, Vector3 pos, int layer)
        {
            Mesh mesh = go.GetComponent<MeshCollider>().sharedMesh;
            VertTriList vt = new VertTriList(mesh);
			Vector3[] verts = mesh.vertices;

			List<Vector3> nVerts = new List<Vector3>();

			for (int i = 0; i < verts.Length; i++)
				if (!Physics.Raycast(pos, (go.transform.TransformPoint(verts[i]) - pos).normalized, out RaycastHit hit, (float)(VectorUtilities.GetDistance(pos, go.transform.TransformPoint(verts[i])) + 0.5), layer))
					return hit;

			return new RaycastHit();
        }
    }
}
