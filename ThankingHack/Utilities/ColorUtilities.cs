using System.Globalization;
using System.Linq;
using Thanking.Misc;
using Thanking.Options.VisualOptions;
using Thanking.Variables.UIVariables;
using UnityEngine;

namespace Thanking.Utilities
{
	public static class ColorUtilities
	{
		public static void addColor(ColorVariable ColorVariable)
		{
            if (!ColorOptions.DefaultColorDict.ContainsKey(ColorVariable.identity))
	            ColorOptions.DefaultColorDict.Add(ColorVariable.identity, ColorVariable);
		}

		public static ColorVariable getColor(string identifier)
		{
			if (ColorOptions.ColorDict.TryGetValue(identifier, out var toret))
				return toret;

			return ColorOptions.errorColor;
		}

		public static string getHex(string identifier)
		{
			if (ColorOptions.ColorDict.TryGetValue(identifier, out var toret))
				return ColorToHex(toret);
			return ColorToHex(ColorOptions.errorColor);
		}

		public static void setColor(string identifier, Color32 color)
		{
			if (ColorOptions.ColorDict.TryGetValue(identifier, out var co))
				co.color = color.ToSerializableColor();
		}

		public static string ColorToHex(Color32 color)
		{
			string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + "FF";
			return hex;
		}

		public static ColorVariable[] getArray()
		{
			return ColorOptions.ColorDict.Values.ToArray();
		}

		public static Color32 HexToColor(string hex)
		{
			byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			return new Color32(r, g, b, 255);
		}
	}
}
