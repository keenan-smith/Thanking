using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MeshUtilities
{
    public static class IcoSphere
    {
        private struct TriangleIndices
        {
            public int v1;
            public int v2;
            public int v3;

            public TriangleIndices(int v1, int v2, int v3)
            {
                this.v1 = v1;
                this.v2 = v2;
                this.v3 = v3;
            }
        }

        // return index of point in the middle of p1 and p2
        private static int getMiddlePoint(int p1, int p2, ref List<Vector3> vertices, ref Dictionary<long, int> cache, float radius)
        {
            // first check if we have it already
            bool firstIsSmaller = p1 < p2;
            long smallerIndex = firstIsSmaller ? p1 : p2;
            long greaterIndex = firstIsSmaller ? p2 : p1;
            long key = (smallerIndex << 32) + greaterIndex;

            int ret;
            if (cache.TryGetValue(key, out ret))
            {
                return ret;
            }

            // not in cache, calculate it
            Vector3 point1 = vertices[p1];
            Vector3 point2 = vertices[p2];
            Vector3 middle = new Vector3
            (
                (point1.x + point2.x) / 2f,
                (point1.y + point2.y) / 2f,
                (point1.z + point2.z) / 2f
            );

            // add vertex makes sure point is on unit sphere
            int i = vertices.Count;
            vertices.Add(middle.normalized * radius);

            // store it, return index
            cache.Add(key, i);

            return i;
        }

        public static GameObject Create(string name, float radius, float recursionLevel)
        {
            GameObject sphere = new GameObject(name);

            Mesh mesh = new Mesh();
            mesh.name = name;

            List<Vector3> vertList = new List<Vector3>();
            Dictionary<long, int> middlePointIndexCache = new Dictionary<long, int>();

            // create 12 vertices of a icosahedron
            float t = (1f + Mathf.Sqrt(5f)) / 2f;

            vertList.Add(new Vector3(-1f, t, 0f).normalized * radius);
            vertList.Add(new Vector3(1f, t, 0f).normalized * radius);
            vertList.Add(new Vector3(-1f, -t, 0f).normalized * radius);
            vertList.Add(new Vector3(1f, -t, 0f).normalized * radius);

            vertList.Add(new Vector3(0f, -1f, t).normalized * radius);
            vertList.Add(new Vector3(0f, 1f, t).normalized * radius);
            vertList.Add(new Vector3(0f, -1f, -t).normalized * radius);
            vertList.Add(new Vector3(0f, 1f, -t).normalized * radius);

            vertList.Add(new Vector3(t, 0f, -1f).normalized * radius);
            vertList.Add(new Vector3(t, 0f, 1f).normalized * radius);
            vertList.Add(new Vector3(-t, 0f, -1f).normalized * radius);
            vertList.Add(new Vector3(-t, 0f, 1f).normalized * radius);


            // create 20 triangles of the icosahedron
            List<TriangleIndices> faces = new List<TriangleIndices>();

            // 5 faces around point 0
            faces.Add(new TriangleIndices(0, 11, 5));
            faces.Add(new TriangleIndices(0, 5, 1));
            faces.Add(new TriangleIndices(0, 1, 7));
            faces.Add(new TriangleIndices(0, 7, 10));
            faces.Add(new TriangleIndices(0, 10, 11));

            // 5 adjacent faces
            faces.Add(new TriangleIndices(1, 5, 9));
            faces.Add(new TriangleIndices(5, 11, 4));
            faces.Add(new TriangleIndices(11, 10, 2));
            faces.Add(new TriangleIndices(10, 7, 6));
            faces.Add(new TriangleIndices(7, 1, 8));

            // 5 faces around point 3
            faces.Add(new TriangleIndices(3, 9, 4));
            faces.Add(new TriangleIndices(3, 4, 2));
            faces.Add(new TriangleIndices(3, 2, 6));
            faces.Add(new TriangleIndices(3, 6, 8));
            faces.Add(new TriangleIndices(3, 8, 9));

            // 5 adjacent faces
            faces.Add(new TriangleIndices(4, 9, 5));
            faces.Add(new TriangleIndices(2, 4, 11));
            faces.Add(new TriangleIndices(6, 2, 10));
            faces.Add(new TriangleIndices(8, 6, 7));
            faces.Add(new TriangleIndices(9, 8, 1));


            // refine triangles
            for (int i = 0; i < recursionLevel; i++)
            {
                List<TriangleIndices> faces2 = new List<TriangleIndices>();
                foreach (var tri in faces)
                {
                    // replace triangle by 4 triangles
                    int a = getMiddlePoint(tri.v1, tri.v2, ref vertList, ref middlePointIndexCache, radius);
                    int b = getMiddlePoint(tri.v2, tri.v3, ref vertList, ref middlePointIndexCache, radius);
                    int c = getMiddlePoint(tri.v3, tri.v1, ref vertList, ref middlePointIndexCache, radius);

                    faces2.Add(new TriangleIndices(tri.v1, a, c));
                    faces2.Add(new TriangleIndices(tri.v2, b, a));
                    faces2.Add(new TriangleIndices(tri.v3, c, b));
                    faces2.Add(new TriangleIndices(a, b, c));
                }
                faces = faces2;
            }

            mesh.vertices = vertList.ToArray();

            List<int> triList = new List<int>();
            for (int i = 0; i < faces.Count; i++)
            {
                triList.Add(faces[i].v1);
                triList.Add(faces[i].v2);
                triList.Add(faces[i].v3);
            }
            mesh.triangles = triList.ToArray();

            var nVertices = mesh.vertices;
            Vector2[] UVs = new Vector2[nVertices.Length];

            for (var i = 0; i < nVertices.Length; i++)
            {
                var unitVector = nVertices[i].normalized;
                Vector2 ICOuv = new Vector2(0, 0);
                ICOuv.x = (Mathf.Atan2(unitVector.x, unitVector.z) + Mathf.PI) / Mathf.PI / 2;
                ICOuv.y = (Mathf.Acos(unitVector.y) + Mathf.PI) / Mathf.PI - 1;
                UVs[i] = new Vector2(ICOuv.x, ICOuv.y);
            }

            mesh.uv = UVs;

            Vector3[] normales = new Vector3[vertList.Count];
            for (int i = 0; i < normales.Length; i++)
                normales[i] = vertList[i].normalized;

            mesh.normals = normales;

            mesh.RecalculateBounds();

            sphere.AddComponent<MeshCollider>().sharedMesh = mesh;
            return sphere;
        }
    }

    public class VertTriList
    {
        public int[][] list;


        //	Indexable - use "vertTri[i]" to get the list of triangles for vertex i.
        public int[] this[int index]
        {
            get { return list[index]; }
        }

        public VertTriList(int[] tri, int numVerts)
        {
            Init(tri, numVerts);
        }


        public VertTriList(Mesh mesh)
        {
            Init(mesh.triangles, mesh.vertexCount);
        }


        //	You don't usually need to call this - it's just to assist the implementation
        //	of the constructors.
        public void Init(int[] tri, int numVerts)
        {
            //	First, go through the triangles, keeping a count of how many times
            //	each vert is used.
            int[] counts = new int[numVerts];

            for (int i = 0; i < tri.Length; i++)
            {
                counts[tri[i]]++;
            }

            //	Initialise an empty jagged array with the appropriate number of elements
            //	for each vert.
            list = new int[numVerts][];

            for (int i = 0; i < counts.Length; i++)
            {
                list[i] = new int[counts[i]];
            }

            //	Assign the appropriate triangle number each time a given vert
            //	is encountered in the triangles.
            for (int i = 0; i < tri.Length; i++)
            {
                int vert = tri[i];
                list[vert][--counts[vert]] = i / 3;
            }
        }
    }

    public static class NearestPointTest
    {
        public static GameObject target;
        public static Vector3 a, b, c;
        public static Vector3 pt;

        public static Vector3 Get(GameObject go, Vector3 pos)
        {
            target = go;

            Mesh mesh = go.GetComponent<MeshCollider>().sharedMesh;
            VertTriList vt = new VertTriList(mesh);
            Vector3[] verts = mesh.vertices.Where(v => !Physics.Raycast(Player.player.look.aim.position, (target.transform.TransformPoint(v) - Player.player.look.aim.position).normalized, Vector3.Distance(Player.player.look.aim.position, target.transform.TransformPoint(v)) + 0.5f, RayMasks.ENTITY | RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.SMALL | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.VEHICLE)).ToArray();

            if (verts == null || verts.Length == 0)
            {
                Debug.Log(":(");
                return Vector3.zero;
            }

            return target.transform.TransformPoint(verts.OrderBy(v => Vector3.Distance(target.transform.TransformPoint(v), pos)).First());
        }


        static Vector3 NearestPointOnMesh(Vector3 pt, Vector3[] verts, int[] tri, VertTriList vt)
        {
            //	First, find the nearest vertex (the nearest point must be on one of the triangles
            //	that uses this vertex if the mesh is convex).
            int nearest = -1;
            float nearestSqDist = 100000000f;

            for (int i = 0; i < verts.Length; i++)
            {
                float sqDist = (verts[i] - pt).sqrMagnitude;

                if (sqDist < nearestSqDist)
                {
                    nearest = i;
                    nearestSqDist = sqDist;
                }
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

                if (possNearestSqDist < nearestSqDist)
                {
                    Debug.Log("Found possible point! " + possNearestPt);
                    nearestPt = possNearestPt;
                    nearestSqDist = possNearestSqDist;
                }
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

            if (sqDist1 < sqDist2)
            {
                if (sqDist1 < sqDist3)
                {
                    return edge1Pt;
                }
                else
                {
                    return edge3Pt;
                }
            }
            else if (sqDist2 < sqDist3)
            {
                return edge2Pt;
            }
            else
            {
                return edge3Pt;
            }
        }
    }
}