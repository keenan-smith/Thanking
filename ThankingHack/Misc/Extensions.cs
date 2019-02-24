using System;
using System.Collections.Generic;
using System.Linq;
using Thanking.Misc.Serializables;
using UnityEngine;

namespace Thanking.Misc
{
	public static class Extensions
	{
		public static Color Invert(this Color32 color) =>
			new Color(255 - color.r, 255 - color.g, 255 - color.b);

		public static SerializableColor ToSerializableColor(this Color32 c) =>
			new SerializableColor(c.r, c.g, c.b, c.a);
		
		public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int N) =>
			source.Skip(Math.Max(0, source.Count() - N));

		public static void AddRange<T>(this HashSet<T> source, IEnumerable<T> target)
		{
			foreach (T t in target)
				source.Add(t);
		}
	}
}
//Discord test