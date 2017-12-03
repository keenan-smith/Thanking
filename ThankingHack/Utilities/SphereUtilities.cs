using SDG.Unturned;
using System.Collections.Generic;
using System.Diagnostics;
using Thanking.Components.Basic;
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

			int BackupLayer = Target.layer;
			Target.layer = LayerMasks.AGENT;

			RaycastComponent Component = Target.GetComponent<RaycastComponent>();

			if (VectorUtilities.GetDistance(Target.transform.position, StartPos) <= Component.Radius)
			{
				Point = Player.player.transform.position;
				return true;
			}
			
			Vector3[] Vertices = Component.Sphere.GetComponent<MeshCollider>().sharedMesh.vertices;
			for (int i = 0; i < Vertices.Length; i++)
			{
				Vector3 Vertex = Component.Sphere.transform.TransformPoint(Vertices[i]);
				Vector3 Normal = VectorUtilities.Normalize(Vertex - StartPos);

				if (Physics.Raycast(StartPos, Normal, Range + 0.5f, RayMasks.DAMAGE_CLIENT))
					continue;
				
				Target.layer = BackupLayer;
				Point = Vertex;
				
				return true;
			}
			
			Target.layer = BackupLayer;
			return false;
		}
	}
}
