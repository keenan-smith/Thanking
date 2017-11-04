﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using Thanking.Variables;

namespace Thanking.Misc
{
	public static class Extensions
	{
		public static Color Invert(this Color32 color) =>
			new Color(255 - color.r, 255 - color.g, 255 - color.b);

		public static SerializableColor ToSerializableColor(this Color32 c) =>
			new SerializableColor(c.r, c.g, c.b, c.a);
	}
}
