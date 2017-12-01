using SDG.Unturned;
using System.Collections.Generic;
using Thanking.Components.MultiAttach;
using Thanking.Options.AimOptions;
using Thanking.Overrides;
using Thanking.Utilities.Mesh_Utilities;
using UnityEngine;

namespace Thanking.Utilities
{
    public static class SphereUtilities
    {
		public static RaycastHit Get(Player Player, Vector3 pos)
		{
			double speed = SphereOptions.DynamicSphere ? Player.GetComponent<VelocityComponent>().Speed : -1;
			double radius = Player.movement.getVehicle() == null ? SphereOptions.SphereRadius : SphereOptions.VehicleSphereRadius;

			if (speed > 0)
				radius = 15.8f - speed * Provider.ping;

			Debug.Log(speed);
			Debug.Log(radius);

			GameObject go = IcoSphere.Create("HitSphere", (float)radius, SphereOptions.RecursionLevel);

			go.transform.parent = Player.transform;
			go.transform.localPosition = new Vector3(0, 0, 0);
			go.layer = LayerMasks.ENEMY; 

			RaycastHit vec = Get(go, pos, RayMasks.ENEMY);
			Object.Destroy(go);

			return vec;
		}

		public static RaycastHit Get(GameObject obj, Vector3 pos, double radius)
		{
			double speed = SphereOptions.DynamicSphere ? obj.GetComponent<VelocityComponent>().Speed : -1;
			if (speed > 0)
				radius = 15.8f - speed * Provider.ping;

			int OrigLayer = obj.layer;
			GameObject go = IcoSphere.Create("HitSphere", (float)radius, SphereOptions.RecursionLevel);

			go.transform.parent = obj.transform;
			go.transform.localPosition = new Vector3(0, 0, 0);
			go.layer = LayerMasks.ENEMY;
			obj.layer = LayerMasks.ENEMY;

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
			int mask = RayMasks.DAMAGE_CLIENT & ~layer;

			for (int i = 0; i < verts.Length; i++)
				if (!Physics.Raycast(pos, (go.transform.TransformPoint(verts[i]) - pos).normalized, (float)VectorUtilities.GetDistance(pos, go.transform.TransformPoint(verts[i])) + 0.5f, mask))
				{
					Physics.Raycast(pos, (go.transform.TransformPoint(verts[i]) - pos).normalized, out RaycastHit hit, (float)VectorUtilities.GetDistance(pos, go.transform.TransformPoint(verts[i])) + 0.5f, layer);
					return hit;
				}
			
			return new RaycastHit();
        }
    }
}
