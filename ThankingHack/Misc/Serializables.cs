using UnityEngine;

namespace Thanking.Misc
{
	public class SerializableVector
	{
		public float x, y, z;

		public SerializableVector(float nx, float ny, float nz)
		{
			x = nx;
			y = ny;
			z = nz;
		}

		public Vector3 ToVector() =>
			new Vector3(x, y, z);

		public static implicit operator Vector3(SerializableVector vector) => vector.ToVector();
	}

	public class SerializableVector2
	{
		public float x, y;

		public SerializableVector2(float nx, float ny)
		{
			x = nx;
			y = ny;
		}

		public Vector2 ToVector2() =>
			new Vector2(x, y);

		public static implicit operator Vector2(SerializableVector2 vector) => vector.ToVector2();
		public static implicit operator SerializableVector2(Vector2 vector) => new SerializableVector2(vector.x, vector.y);
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

        public static implicit operator Color32(SerializableColor color) => color.ToColor();

		public static implicit operator Color(SerializableColor color) => color.ToColor();

		public static implicit operator SerializableColor(Color32 color) => color.ToSerializableColor();

		public Color32 ToColor() =>
			new Color32((byte)r, (byte)g, (byte)b, (byte)a);
	}
}
