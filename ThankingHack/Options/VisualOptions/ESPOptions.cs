using System.Collections.Generic;
using Thanking.Attributes;
using Thanking.Misc;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Options.VisualOptions
{
	public static class ESPOptions
	{
		#region Global Options

		[Save] public static bool Enabled = true;
		[Save] public static KeyCode Toggle = KeyCode.LeftBracket;

		[Save]
		public static ESPVisual[] VisualOptions =
		{
			new ESPVisual
			{
				Enabled = true,
				Labels = true,
				Boxes = true,
				TwoDimensional = true,
				InfiniteDistance = true,
				LineToObject = false,
				TextScaling = true,
				UseObjectCap = false,

				Distance = 500,
				Location = LabelLocation.BottomMiddle,
				Color = ((Color32) Color.red).ToSerializableColor(),

				FixedTextSize = 12,
				MinTextSize = 8,
				MaxTextSize = 17,
				MinTextSizeDistance = 900,
				BorderStrength = 2,
				ObjectCap = 24,
			}, //Players

			new ESPVisual
			{
				Enabled = true,
				Labels = true,
				Boxes = true,
				TwoDimensional = true,
				InfiniteDistance = true,
				LineToObject = false,
				TextScaling = true,
				UseObjectCap = true,

				Distance = 500,
				Location = LabelLocation.BottomMiddle,
				Color = ((Color32) Color.red).ToSerializableColor(),

				FixedTextSize = 12,
				MinTextSize = 8,
				MaxTextSize = 17,
				MinTextSizeDistance = 900,
				BorderStrength = 2,
				ObjectCap = 24,
			}, //Zombies

			new ESPVisual
			{
				Enabled = true,
				Labels = true,
				Boxes = true,
				TwoDimensional = false,
				InfiniteDistance = false,
				LineToObject = false,
				TextScaling = true,
				UseObjectCap = true,

				Distance = 250,
				Location = LabelLocation.BottomMiddle,
				Color = ((Color32) Color.cyan).ToSerializableColor(),

				FixedTextSize = 11,
				MinTextSize = 8,
				MaxTextSize = 14,
				MinTextSizeDistance = 800,
				BorderStrength = 2,
				ObjectCap = 24,
			}, //Items

			new ESPVisual
			{
				Enabled = false,
				Labels = true,
				Boxes = true,
				TwoDimensional = false,
				InfiniteDistance = false,
				LineToObject = false,
				TextScaling = true,
				UseObjectCap = true,

				Distance = 250,
				Location = LabelLocation.BottomMiddle,
				Color = ((Color32) Color.cyan).ToSerializableColor(),

				FixedTextSize = 11,
				MinTextSize = 8,
				MaxTextSize = 11,
				MinTextSizeDistance = 800,
				BorderStrength = 2,
				ObjectCap = 24,
			}, //Sentries

			new ESPVisual
			{
				Enabled = false,
				Labels = true,
				Boxes = true,
				TwoDimensional = false,
				InfiniteDistance = false,
				LineToObject = false,
				TextScaling = true,
				UseObjectCap = true,

				Distance = 250,
				Location = LabelLocation.BottomMiddle,
				Color = ((Color32) Color.cyan).ToSerializableColor(),

				FixedTextSize = 11,
				MinTextSize = 8,
				MaxTextSize = 11,
				MinTextSizeDistance = 800,
				BorderStrength = 2,
				ObjectCap = 24,
			}, //Beds

			new ESPVisual
			{
				Enabled = false,
				Labels = true,
				Boxes = true,
				TwoDimensional = false,
				InfiniteDistance = false,
				LineToObject = false,
				TextScaling = true,
				UseObjectCap = true,

				Distance = 250,
				Location = LabelLocation.BottomMiddle,
				Color = ((Color32) Color.cyan).ToSerializableColor(),

				FixedTextSize = 11,
				MinTextSize = 8,
				MaxTextSize = 11,
				MinTextSizeDistance = 800,
				BorderStrength = 2,
				ObjectCap = 24,
			}, //Claim Flags

			new ESPVisual
			{
				Enabled = false,
				Labels = true,
				Boxes = true,
				TwoDimensional = false,
				InfiniteDistance = false,
				LineToObject = false,
				TextScaling = true,
				UseObjectCap = true,

				Distance = 250,
				Location = LabelLocation.BottomMiddle,
				Color = ((Color32) Color.cyan).ToSerializableColor(),

				FixedTextSize = 11,
				MinTextSize = 8,
				MaxTextSize = 11,
				MinTextSizeDistance = 800,
				BorderStrength = 2,
				ObjectCap = 24,
			}, //Vehicles

			new ESPVisual
			{
				Enabled = false,
				Labels = true,
				Boxes = true,
				TwoDimensional = false,
				InfiniteDistance = false,
				LineToObject = false,
				TextScaling = true,
				UseObjectCap = true,

				Distance = 250,
				Location = LabelLocation.BottomMiddle,
				Color = ((Color32) Color.cyan).ToSerializableColor(),

				FixedTextSize = 11,
				MinTextSize = 8,
				MaxTextSize = 11,
				MinTextSizeDistance = 800,
				BorderStrength = 2,
				ObjectCap = 24,
			}, //Storage

			new ESPVisual
			{
				Enabled = false,
				Labels = true,
				Boxes = true,
				TwoDimensional = false,
				InfiniteDistance = false,
				LineToObject = false,
				TextScaling = true,
				UseObjectCap = true,

				Distance = 250,
				Location = LabelLocation.BottomMiddle,
				Color = ((Color32) Color.cyan).ToSerializableColor(),

				FixedTextSize = 11,
				MinTextSize = 8,
				MaxTextSize = 11,
				MinTextSizeDistance = 800,
				BorderStrength = 2,
				ObjectCap = 24,
			} //Generators
		};

		[Save] public static Dictionary<ESPTarget, int> PriorityTable = new Dictionary<ESPTarget, int>
		{
			{ESPTarget.Players, 0},
			{ESPTarget.Items, 1},
			{ESPTarget.Sentries, 2},
			{ESPTarget.Beds, 3},
			{ESPTarget.ClaimFlags, 4},
			{ESPTarget.Vehicles, 5},
			{ESPTarget.Storage, 6},
			{ESPTarget.Generators, 7}
		};

		#endregion

		#region Player Options

		[Save] public static bool ShowPlayerName = true;
		[Save] public static bool ShowPlayerDistance = true;
		[Save] public static bool ShowPlayerWeapon = true;
		[Save] public static bool UsePlayerGroup = true;

		[Save] public static SerializableColor SameGroupColor = ((Color32) Color.green).ToSerializableColor();

		#endregion
	}
}