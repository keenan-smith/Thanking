using UnityEngine;

namespace Thanking.Misc.Serializables
{
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