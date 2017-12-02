using SDG.Unturned;
using System.Collections.Generic;
using System.Diagnostics;
using Thanking.Components.MultiAttach;
using Thanking.Coroutines;
using Thanking.Options.AimOptions;
using Thanking.Overrides;
using Thanking.Utilities.Mesh_Utilities;
using UnityEngine;

namespace Thanking.Utilities
{
    public static class SphereUtilities
    {
		public static bool GetRaycast(GameObject Target, Vector3 StartPos, float Range, out RaycastHit Hit)
		{
			Hit = new RaycastHit();
			if (AimbotCoroutines.IsAiming)
			{
				GameObject AObject = AimbotCoroutines.LockedObject;
				int AimbotBackupLayer = AObject.layer;
				Vector3 Normal = Vector3.Normalize(AObject.transform.position - StartPos);

				AObject.layer = LayerMasks.AGENT;
				bool Return = Physics.Raycast(StartPos, Normal, out Hit, Range, RayMasks.AGENT);
				AObject.layer = AimbotBackupLayer;

				return Return;
			}

			if (Target == null)
				return false;
			
			float Speed = SphereOptions.DynamicSphere ? Target.GetComponent<VelocityComponent>().Speed : -1;
			float Radius = SphereOptions.SphereRadius;

			if (Speed > -1)
				Radius = 15.8f - Speed * Provider.ping;

			int BackupLayer = Target.layer;
			Target.layer = LayerMasks.AGENT;

			if (VectorUtilities.GetDistance(Target.transform.position, StartPos) <= Radius)
			{
				Physics.Raycast(StartPos + new Vector3(0, 1, 0), Vector3.down, out Hit, Range, RayMasks.ENEMY);
				return true;
			}

			GameObject Sphere = IcoSphere.Create("HitSphere", Radius, SphereOptions.RecursionLevel);
			Vector3[] Vertices = Sphere.GetComponent<MeshCollider>().sharedMesh.vertices;
			
			for (int i = 0; i < Vertices.Length; i++)
			{
				Vector3 Vertex = Sphere.transform.TransformPoint(Vertices[i]);
				Vector3 Normal = Vector3.Normalize(Vertex - StartPos);

				UnityEngine.Debug.Log(Normal);
				
				if (Physics.Raycast(StartPos, Normal, Range, RayMasks.DAMAGE_CLIENT))
					continue;

				UnityEngine.Debug.Log("nigga");
				Physics.Raycast(StartPos, Normal, out Hit, Range, RayMasks.AGENT);

				Object.Destroy(Sphere);
				Target.layer = BackupLayer;

				return true;
			}

			Object.Destroy(Sphere);
			Target.layer = BackupLayer;

			return false;
		}
	}
}
