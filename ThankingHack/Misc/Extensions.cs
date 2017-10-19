using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;

namespace Thanking.Misc
{
	public static class Extensions
	{
		public static SerializableColor ToSerializableColor(this Color32 c) =>
			new SerializableColor(c.r, c.g, c.b, c.a);
	}
}
