using UnityEngine;

namespace Thanking.Utilities.Mesh_Utilities
{
    public static class NearestPointTest
    {
        public static Vector3 a, b, c;
        public static Vector3 pt;
        
        static Vector3 NearestPointOnMesh(Vector3 pt, Vector3[] verts, int[] tri, VertTriList vt)
        {
            //	First, find the nearest vertex (the nearest point must be on one of the triangles
            //	that uses this vertex if the mesh is convex).
            int nearest = -1;
            float nearestSqDist = 100000000f;

            for (int i = 0; i < verts.Length; i++)
            {
                float sqDist = (verts[i] - pt).sqrMagnitude;

                if (!(sqDist < nearestSqDist)) continue;

                nearest = i;
                nearestSqDist = sqDist;
            }

            //	Get the list of triangles in which the nearest vert "participates".

            if (nearest == -1)
                return Vector3.zero;

            int[] nearTris = vt[nearest];

            Vector3 nearestPt = Vector3.zero;
            nearestSqDist = 100000000f;

            for (int i = 0; i < nearTris.Length; i++)
            {
                int triOff = nearTris[i] * 3;
                Vector3 a = verts[tri[triOff]];
                Vector3 b = verts[tri[triOff + 1]];
                Vector3 c = verts[tri[triOff + 2]];

                Vector3 possNearestPt = NearestPointOnTri(pt, a, b, c);
                float possNearestSqDist = (pt - possNearestPt).sqrMagnitude;

                if (!(possNearestSqDist < nearestSqDist)) continue;

                #if DEBUG
                DebugUtilities.Log("Found possible point! " + possNearestPt);
                #endif
    
                nearestPt = possNearestPt;
                nearestSqDist = possNearestSqDist;
            }

            return nearestPt;
        }


        public static Vector3 NearestPointOnTri(Vector3 pt, Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 edge1 = b - a;
            Vector3 edge2 = c - a;
            Vector3 edge3 = c - b;
            float edge1Len = edge1.magnitude;
            float edge2Len = edge2.magnitude;
            float edge3Len = edge3.magnitude;

            Vector3 ptLineA = pt - a;
            Vector3 ptLineB = pt - b;
            Vector3 ptLineC = pt - c;
            Vector3 xAxis = edge1 / edge1Len;
            Vector3 zAxis = Vector3.Cross(edge1, edge2).normalized;
            Vector3 yAxis = Vector3.Cross(zAxis, xAxis);

            Vector3 edge1Cross = Vector3.Cross(edge1, ptLineA);
            Vector3 edge2Cross = Vector3.Cross(edge2, -ptLineC);
            Vector3 edge3Cross = Vector3.Cross(edge3, ptLineB);
            bool edge1On = Vector3.Dot(edge1Cross, zAxis) > 0f;
            bool edge2On = Vector3.Dot(edge2Cross, zAxis) > 0f;
            bool edge3On = Vector3.Dot(edge3Cross, zAxis) > 0f;

            //	If the point is inside the triangle then return its coordinate.
            if (edge1On && edge2On && edge3On)
            {
                float xExtent = Vector3.Dot(ptLineA, xAxis);
                float yExtent = Vector3.Dot(ptLineA, yAxis);
                return a + xAxis * xExtent + yAxis * yExtent;
            }

            //	Otherwise, the nearest point is somewhere along one of the edges.
            Vector3 edge1Norm = xAxis;
            Vector3 edge2Norm = edge2.normalized;
            Vector3 edge3Norm = edge3.normalized;

            float edge1Ext = Mathf.Clamp(Vector3.Dot(edge1Norm, ptLineA), 0f, edge1Len);
            float edge2Ext = Mathf.Clamp(Vector3.Dot(edge2Norm, ptLineA), 0f, edge2Len);
            float edge3Ext = Mathf.Clamp(Vector3.Dot(edge3Norm, ptLineB), 0f, edge3Len);

            Vector3 edge1Pt = a + edge1Ext * edge1Norm;
            Vector3 edge2Pt = a + edge2Ext * edge2Norm;
            Vector3 edge3Pt = b + edge3Ext * edge3Norm;

            float sqDist1 = (pt - edge1Pt).sqrMagnitude;
            float sqDist2 = (pt - edge2Pt).sqrMagnitude;
            float sqDist3 = (pt - edge3Pt).sqrMagnitude;

            return sqDist1 < sqDist2
                ? (sqDist1 < sqDist3 ? edge1Pt : edge3Pt)
                : (sqDist2 < sqDist3 ? edge2Pt : edge3Pt);
        }
    }
}
