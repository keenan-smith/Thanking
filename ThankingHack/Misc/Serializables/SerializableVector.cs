using UnityEngine;

namespace Thanking.Misc.Serializables
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
}
