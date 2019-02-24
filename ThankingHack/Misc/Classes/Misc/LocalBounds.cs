using UnityEngine;

namespace Thanking.Misc.Classes.Misc
{
	public class LocalBounds
	{
		public Vector3 PosOffset;
		public Vector3 Extents;

		public LocalBounds(Vector3 po, Vector3 e)
		{
			PosOffset = po;
			Extents = e;
		}
	}
}