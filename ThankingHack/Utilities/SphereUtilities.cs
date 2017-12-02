using SDG.Unturned;
using System.Collections.Generic;
using System.Diagnostics;
using Thanking.Components.MultiAttach;
using Thanking.Coroutines;
using Thanking.Options.AimOptions;
using Thanking.Overrides;
using Thanking.Utilities.Mesh_Utilities;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Thanking.Utilities
{
    public static class SphereUtilities
    {
		public static bool GetRaycast(GameObject Target, Vector3 StartPos, float Range, out Vector3 Point)
		{
			Point = Vector3.zero;

			if (AimbotCoroutines.IsAiming)
			{
				GameObject AObject = AimbotCoroutines.LockedObject;
				int AimbotBackupLayer = AObject.layer;
				Vector3 Normal = (AObject.transform.position - StartPos).normalized;

				AObject.layer = LayerMasks.AGENT;
				bool Return = !Physics.Raycast(StartPos, Normal, Range, RayMasks.DAMAGE_CLIENT);
				AObject.layer = AimbotBackupLayer;

				Point = Target.transform.position;
				return Return;
			}
			if (Target == null)
				return false;
			
			float Speed = SphereOptions.DynamicSphere ? Target.GetComponent<VelocityComponent>().Speed : -1;
			float Radius = SphereOptions.SphereRadius;
			if (Speed > -1)
				Radius = 15.8f - Speed * Provider.ping;

			Debug.Log(Radius);
			
			int BackupLayer = Target.layer;
			Target.layer = LayerMasks.AGENT;

			if (VectorUtilities.GetDistance(Target.transform.position, StartPos) <= Radius)
			{
				Point = Player.player.transform.position;
				return true;
			}
			
			GameObject Sphere = IcoSphere.Create("HitSphere", Radius, SphereOptions.RecursionLevel);
			
			Sphere.transform.parent = Target.transform;
			Sphere.transform.localPosition = new Vector3(0, 0, 0);
			Sphere.layer = LayerMasks.AGENT;
			
			Vector3[] Vertices = Sphere.GetComponent<MeshCollider>().sharedMesh.vertices;
			for (int i = 0; i < Vertices.Length; i++)
			{
				Vector3 Vertex = Sphere.transform.TransformPoint(Vertices[i]);
				Vector3 Normal = VectorUtilities.Normalize(Vertex - StartPos);

				if (Physics.Raycast(StartPos, Normal, Range + 0.5f, RayMasks.DAMAGE_CLIENT))
					continue;
				
				Object.Destroy(Sphere);;
				Target.layer = BackupLayer;
				Point = Vertex;
				
				return true;
			}
			Object.Destroy(Sphere);
			Target.layer = BackupLayer;

			return false;
		}
	}
}
