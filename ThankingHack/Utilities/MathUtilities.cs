using System.Collections.Generic;
using Thanking.Attributes;
using UnityEngine;

namespace Thanking.Utilities
{
	public static class MathUtilities
	{
		public static System.Random Random;

		[Initializer]
		public static void GenerateRandom() => Random = new System.Random();
		
		public static bool Intersect(Vector3 p0, Vector3 p1, Vector3 oVector, Vector3 bCenter, out Vector3 intersection) //dad helped me with this, let's pray it works
		{
			//if intersects return true
			//if not return false

			intersection = Vector3.zero;

			Vector3 lineDir = p1 - p0;
			float dProduct = Vector3.Dot(lineDir, oVector); //dot product between orthogonal vector and the line between p0 and p1

			if (dProduct == 0) //if 0 they're parallel
				return false;

			float oDotProduct = Vector3.Dot(p0 - bCenter, oVector); //dot product between p0 and orthogonal vector
			float lambda = -(oDotProduct / dProduct); //lambda is distance from p0 on vector from p0 to p1
			
			intersection = p0 + lambda * lineDir; //vector to point
			return true;
		}
		public static Vector3 GetOrthogonalVector(Vector3 vCenter, Vector3 vPoint)
		{
			Vector3 ortho = vCenter - vPoint;
			double magnitude = VectorUtilities.GetDistance(vCenter, vPoint);
			
			#if DEBUG
			DebugUtilities.Log("Original ortho vector: " + ortho);
			#endif		
	
			return ortho / (float)magnitude;
		}

		public static Vector3[] GetRectanglePoints(Vector3 playerPos, Vector3[] bCorners, Bounds bound)
		{
			#if DEBUG
			DebugUtilities.Log("Player position:" + playerPos);
			#endif
	
			Vector3 oVector = GetOrthogonalVector(bound.center, playerPos);
			
			#if DEBUG
			DebugUtilities.Log("Ortho vector: " + oVector);
			DebugUtilities.Log("Plane center:" + bound.center);
			#endif

			List<Vector3> fVectors = new List<Vector3>();
			Vector3[] Vertices = new Vector3[24]
				{
					bCorners[0], //front top left to front right
					bCorners[1],
					bCorners[1], //front top right to front bottom right
					bCorners[3],
					bCorners[3], //front bottom right to front bottom left
					bCorners[2],
					bCorners[2], //front bottom left to front top left
					bCorners[0],
					bCorners[4], //back top left to back top right
					bCorners[5],
					bCorners[5],//back top right to back bottom right
					bCorners[7],
					bCorners[7], //back bottom right to back bottom left
					bCorners[6],
					bCorners[6], //front top left to back top left
					bCorners[4],
					bCorners[0], //front top right to back top right
					bCorners[4],
					bCorners[1], //front bottom left to back bottom left
					bCorners[5],
					bCorners[2], //front bottom left to back bottom left
					bCorners[6],
					bCorners[3], //front bottom right to back bottom right
					bCorners[7]
				};

			for (int i = 0; i < 24; i += 2)
			{
				Vector3 p0 = Vertices[i];
				Vector3 p1 = Vertices[i + 1];

				if (Intersect(p0, p1, oVector, bound.center, out Vector3 iPos))
					fVectors.Add(iPos); //returns vector to intercept between plane defined by oVector and bCenter, and line defined by p0 and p1. Returns false if not exist
			}

			Bounds nBound = new Bounds(bound.center, bound.size * 1.2f);

			for (int i = fVectors.Count - 1; i > -1; i--)
				if (!nBound.Contains(fVectors[i]))
					fVectors.RemoveAt(i);

			return fVectors.ToArray();
		}
	}
}
