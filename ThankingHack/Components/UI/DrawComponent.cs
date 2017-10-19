using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Components.UI
{
	public class DrawComponent : MonoBehaviour
	{
		public Vector3 LocalOffset;
		public Vector3 LocalSize;
		public Color DrawColor = new Color(0, 0, 0, 0);

		public void Awake()
		{
			Bounds b = DrawUtilities.GetBounds(gameObject);
			LocalOffset = b.center - transform.position;
			LocalSize = b.size * 0.8f;
		}

		public void OnGUI()
		{
			if (Event.current.type != EventType.Repaint || DrawColor == new Color(0, 0, 0, 0))
				return;

			Vector3 position = transform.position;
			Vector3 vec = Camera.main.WorldToScreenPoint(position);

			if (vec.z <= 0)
				return;

			Bounds b = new Bounds(transform.position + LocalOffset, LocalSize);
			DrawUtilities.DrawOutline(b, ESPComponent.mat, DrawColor);
		}
	}
}
