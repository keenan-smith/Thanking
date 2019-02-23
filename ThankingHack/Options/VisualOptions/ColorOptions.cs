using System.Collections.Generic;
using Thinking.Attributes;
using Thinking.Options.UIVariables;
using UnityEngine;

namespace Thinking.Options.VisualOptions
{
	public static class ColorOptions
	{
		public static Dictionary<string, ColorVariable> DefaultColorDict = new Dictionary<string, ColorVariable>();
		
		[Save] public static Dictionary<string, ColorVariable> ColorDict = new Dictionary<string, ColorVariable>();
		public static ColorVariable errorColor = new ColorVariable("errorColor", "#ERROR.NOTFOUND", Color.magenta);
		public static string selectedOption;
		public static ColorVariable preview = new ColorVariable("preview", "No Color Selected", Color.white);
		public static ColorVariable previewselected;
	}
}