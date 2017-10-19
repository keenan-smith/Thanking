using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Thanking.Misc
{
	public class SerializableVector
	{
		public int x, y, z;

		public SerializableVector(int nx, int ny, int nz)
		{
			x = nx;
			y = ny;
			z = nz;
		}

		public Vector3 ToVector() =>
			new Vector3(x, y, z);
	}

	public class SerializableColor
	{
		public int r, g, b, a;

		public SerializableColor() { }

		public SerializableColor(int nr, int ng, int nb, int na)
		{
			r = nr;
			g = ng;
			b = nb;
			a = na;
		}

		public SerializableColor(int nr, int ng, int nb)
		{
			r = nr;
			g = ng;
			b = nb;
			a = 255;
		}

		public Color32 ToColor() =>
			new Color32((byte)r, (byte)g, (byte)b, (byte)a);
	}
}
