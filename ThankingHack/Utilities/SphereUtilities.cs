using System;
using SDG.Unturned;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Thinking.Components.Basic;
using Thinking.Coroutines;
using Thinking.Options.AimOptions;
using Thinking.Overrides;
using Thinking.Utilities.Mesh_Utilities;
using Thinking.Variables;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Thinking.Utilities
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

			Vector3[] verts = Component.Sphere.GetComponent<MeshCollider>().sharedMesh.vertices;

			Vector3[] nVerts =
				verts
					.Select(v => Component.Sphere.transform.TransformPoint(v))
					.ToArray();
			
			for (int i = 0; i < nVerts.Length; i++)
			{
				Vector3 Vertex = nVerts[i];
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
