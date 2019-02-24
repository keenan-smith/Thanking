using System.Collections.Generic;
using Thanking.Attributes;
using Thanking.Variables.UIVariables;
using UnityEngine;

namespace Thanking.Options.VisualOptions
{
	public static class ColorOptions
	{
		[Save] public static Dictionary<string, ColorVariable> ColorDict = new Dictionary<string, ColorVariable>();
		
		public static Dictionary<string, ColorVariable> DefaultColorDict = new Dictionary<string, ColorVariable>();
		
		public static ColorVariable errorColor = new ColorVariable("errorColor", "#ERROR.NOTFOUND", Color.magenta);
		public static ColorVariable preview = new ColorVariable("preview", "No Color Selected", Color.white);
		public static ColorVariable previewselected;
		
		public static string selectedOption;
	}
}