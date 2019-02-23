using System.Globalization;
using System.Linq;
using Thinking.Misc;
using Thinking.Options.UIVariables;
using Thinking.Options.VisualOptions;
using UnityEngine;

namespace Thinking.Utilities
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
			ColorVariable toret;
			if (ColorOptions.ColorDict.TryGetValue(identifier, out toret))
				return toret;

			return ColorOptions.errorColor;
		}

		public static string getHex(string identifier)
		{
			ColorVariable toret;
			if (ColorOptions.ColorDict.TryGetValue(identifier, out toret))
				return ColorToHex(toret);
			return ColorToHex(ColorOptions.errorColor);
		}

		public static void setColor(string identifier, Color32 color)
		{
			ColorVariable co;
			if (ColorOptions.ColorDict.TryGetValue(identifier, out co))
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
