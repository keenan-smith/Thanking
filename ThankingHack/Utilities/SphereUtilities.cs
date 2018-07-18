using SDG.Unturned;
using System.Collections.Generic;
using System.Diagnostics;
using Thanking.Components.Basic;
using Thanking.Coroutines;
using Thanking.Options.AimOptions;
using Thanking.Overrides;
using Thanking.Utilities.Mesh_Utilities;
using Thanking.Variables;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Thanking.Utilities
{
    public static class SphereUtilities
    {
		public static bool GetRaycast(GameObject Target, Vector3 StartPos, out Vector3 Point)
		{
			Point = Vector3.zero;
			
			if (Target == null)
				return false;

			int BackupLayer = Target.layer;
			Target.layer = LayerMasks.AGENT;

			RaycastComponent Component = Target.GetComponent<RaycastComponent>();

			if (VectorUtilities.GetDistance(Target.transform.position, StartPos) <= 15.5f)
			{
				Point = OptimizationVariables.MainPlayer.transform.position;
				return true;
			}
			
			Vector3[] Vertices = Component.Sphere.GetComponent<MeshCollider>().sharedMesh.vertices;
			for (int i = 0; i < Vertices.Length; i++)
			{
				Vector3 Vertex = Component.Sphere.transform.TransformPoint(Vertices[i]);
				Vector3 Normal = VectorUtilities.Normalize(Vertex - StartPos);

				double Distance = VectorUtilities.GetDistance(StartPos, Vertex);
				
				if (Physics.Raycast(StartPos, Normal, (float)Distance + 0.5f, RayMasks.DAMAGE_CLIENT))
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
