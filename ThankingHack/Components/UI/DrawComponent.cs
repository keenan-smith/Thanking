using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Thanking.Components.UI
{
	public class DrawComponent : MonoBehaviour
	{
		public Vector3[] CornerOffsets;
		public Vector3[] CenterOffset;
		public Camera mcamera;
		public Renderer[] renderers;
		public MeshFilter[] meshes;
		public Material[][] Materials;
		Quaternion quat;
		Bounds bound;

		void Awake()
		{
			renderers = GetComponentsInChildren<Renderer>();
			meshes = GetComponentsInChildren<MeshFilter>();
			Materials = new Material[renderers.Length][];
			for (int i = 0; i < renderers.Length; i++)
				Materials[i] = renderers[i].materials;
		}

		void Start()
		{
			mcamera = Camera.main;
			init();
		}

		public Vector3[] corners;

		public void init()
		{
			calculateBounds();
			setPoints();
			setLines();
		}

		void calculateBounds()
		{
			Quaternion quat = transform.rotation;//object axis AABB
			if (renderers[0].isPartOfStaticBatch) quat = Quaternion.Euler(0f, 0f, 0f);//world axis
			
			bound = new Bounds();
			if (renderers[0].isPartOfStaticBatch)
			{
				bound = renderers[0].bounds;
				for (int i = 1; i < renderers.Length; i++)
					bound.Encapsulate(renderers[i].bounds);
				return;
			}
			transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			for (int i = 0; i < meshes.Length; i++)
			{
				Mesh ms = meshes[i].mesh;
				Vector3 tr = meshes[i].gameObject.transform.position;
				Vector3 ls = meshes[i].gameObject.transform.lossyScale;
				Quaternion lr = meshes[i].gameObject.transform.rotation;
				int vc = ms.vertexCount;
				for (int j = 0; j < vc; j++)
				{
					if (i == 0 && j == 0)
						bound = new Bounds(tr + lr * Vector3.Scale(ls, ms.vertices[j]), Vector3.zero);
					else
						bound.Encapsulate(tr + lr * Vector3.Scale(ls, ms.vertices[j]));
				}
			}
			transform.rotation = quat;
		}

		void setPoints()
		{

			Vector3 bc = transform.position + quat * (bound.center - transform.position);

			topFrontRight = bc + quat * Vector3.Scale(bound.extents, new Vector3(1, 1, 1));
			topFrontLeft = bc + quat * Vector3.Scale(bound.extents, new Vector3(-1, 1, 1));
			bottomFrontRight = bc + quat * Vector3.Scale(bound.extents, new Vector3(1, -1, 1));
			bottomFrontLeft = bc + quat * Vector3.Scale(bound.extents, new Vector3(-1, -1, 1));
			topBackLeft = bc + quat * Vector3.Scale(bound.extents, new Vector3(-1, 1, -1));
			topBackRight = bc + quat * Vector3.Scale(bound.extents, new Vector3(1, 1, -1));
			bottomBackLeft = bc + quat * Vector3.Scale(bound.extents, new Vector3(-1, -1, -1));
			bottomBackRight = bc + quat * Vector3.Scale(bound.extents, new Vector3(1, -1, -1));
			corners = new Vector3[] { topFrontRight, topFrontLeft, topBackLeft, topBackRight, bottomFrontRight, bottomFrontLeft, bottomBackLeft, bottomBackRight };

		}

		void setLines()
		{

			int i1;
			int linesCount = 12;

			lines = new Vector3[linesCount, 2];
			for (int i = 0; i < 4; i++)
			{
				i1 = (i + 1) % 4;//top rectangle
				lines[i, 0] = corners[i];
				lines[i, 1] = corners[i1];
				//break;
				i1 = i + 4;//vertical lines
				lines[i + 4, 0] = corners[i];
				lines[i + 4, 1] = corners[i1];
				//bottom rectangle
				lines[i + 8, 0] = corners[i1];
				i1 = 4 + (i + 1) % 4;
				lines[i + 8, 1] = corners[i1];
			}
		}
	}
}
