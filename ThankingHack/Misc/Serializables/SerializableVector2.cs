using UnityEngine;

namespace Thanking.Misc.Serializables
{
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
}