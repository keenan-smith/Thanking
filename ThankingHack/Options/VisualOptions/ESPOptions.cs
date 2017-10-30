using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
using Thanking.Misc;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Options.VisualOptions
{
	public static class ESPOptions
	{
		#region Global Options
		[Save]
		public static bool Enabled = true;
		[Save]
		public static KeyCode Toggle = KeyCode.LeftBracket;

		[Save]
		public static Dictionary<ESPTarget, ESPVisual> VisualOptions = new Dictionary<ESPTarget, ESPVisual>()
		{
			{ ESPTarget.Players, new ESPVisual()
				{
					Enabled = true,
					Rectangle = true,
					InfiniteDistance = true,
					LineToObject = false,
					TextScaling = true,

					Distance = 500,
					Location = LabelLocation.BottomMiddle,
					Color = ((Color32)Color.red).ToSerializableColor(),

					FixedTextSize = 12,
					MinTextSize = 8,
					MaxTextSize = 17,
					MinTextSizeDistance = 900,
					BorderStrength = 2
				}
			}, //Players
			{ ESPTarget.Items, new ESPVisual()
				{
					Enabled = true,
					Rectangle = false,
					InfiniteDistance = false,
					LineToObject = false,
					TextScaling = true,

					Distance = 250,
					Location = LabelLocation.BottomMiddle,
					Color = ((Color32)Color.cyan).ToSerializableColor(),

					FixedTextSize = 11,
					MinTextSize = 8,
					MaxTextSize = 14,
					MinTextSizeDistance = 800,
					BorderStrength = 2
				}
			}, //Items
			{ ESPTarget.Sentries, new ESPVisual()
				{
					Enabled = false,
					Rectangle = false,
					InfiniteDistance = false,
					LineToObject = false,
					TextScaling = true,

					Distance = 250,
					Location = LabelLocation.BottomMiddle,
					Color = ((Color32)Color.cyan).ToSerializableColor(),

					FixedTextSize = 11,
					MinTextSize = 8,
					MaxTextSize = 11,
					MinTextSizeDistance = 800,
					BorderStrength = 2
				}
			}, //Sentries
			{ ESPTarget.Beds, new ESPVisual()
				{
					Enabled = false,
					Rectangle = false,
					InfiniteDistance = false,
					LineToObject = false,
					TextScaling = true,

					Distance = 250,
					Location = LabelLocation.BottomMiddle,
					Color = ((Color32)Color.cyan).ToSerializableColor(),

					FixedTextSize = 11,
					MinTextSize = 8,
					MaxTextSize = 11,
					MinTextSizeDistance = 800,
					BorderStrength = 2
				}
			}, //Beds
			{ ESPTarget.ClaimFlags, new ESPVisual()
				{
					Enabled = false,
					Rectangle = false,
					InfiniteDistance = false,
					LineToObject = false,
					TextScaling = true,

					Distance = 250,
					Location = LabelLocation.BottomMiddle,
					Color = ((Color32)Color.cyan).ToSerializableColor(),

					FixedTextSize = 11,
					MinTextSize = 8,
					MaxTextSize = 11,
					MinTextSizeDistance = 800,
					BorderStrength = 2
				}
			}, //Claim Flags
			{ ESPTarget.Vehicles, new ESPVisual()
				{
					Enabled = false,
					Rectangle = false,
					InfiniteDistance = false,
					LineToObject = false,
					TextScaling = true,

					Distance = 250,
					Location = LabelLocation.BottomMiddle,
					Color = ((Color32)Color.cyan).ToSerializableColor(),

					FixedTextSize = 11,
					MinTextSize = 8,
					MaxTextSize = 11,
					MinTextSizeDistance = 800,
					BorderStrength = 2
				}
			}, //Vehicles
			{ ESPTarget.Storage, new ESPVisual()
				{
					Enabled = false,
					Rectangle = false,
					InfiniteDistance = false,
					LineToObject = false,
					TextScaling = true,

					Distance = 250,
					Location = LabelLocation.BottomMiddle,
					Color = ((Color32)Color.cyan).ToSerializableColor(),

					FixedTextSize = 11,
					MinTextSize = 8,
					MaxTextSize = 11,
					MinTextSizeDistance = 800,
					BorderStrength = 2
				}
			}, //Storage
			{ ESPTarget.Generators, new ESPVisual()
				{
					Enabled = false,
					Rectangle = false,
					InfiniteDistance = false,
					LineToObject = false,
					TextScaling = true,

					Distance = 250,
					Location = LabelLocation.BottomMiddle,
					Color = ((Color32)Color.cyan).ToSerializableColor(),

					FixedTextSize = 11,
					MinTextSize = 8,
					MaxTextSize = 11,
					MinTextSizeDistance = 800,
					BorderStrength = 2
				}
			} //Generators
		};

		[Save]
		public static Dictionary<ESPTarget, int> PriorityTable = new Dictionary<ESPTarget, int>()
		{
			{ ESPTarget.Players, 0 },
			{ ESPTarget.Items, 1 },
			{ ESPTarget.Sentries, 2 },
			{ ESPTarget.Beds, 3 },
			{ ESPTarget.ClaimFlags, 4 },
			{ ESPTarget.Vehicles, 5 },
			{ ESPTarget.Storage, 6 },
			{ ESPTarget.Generators, 7 }
		};
		#endregion

		#region Player Options
		[Save]
		public static bool ShowPlayerName = true;
		[Save]
		public static bool ShowPlayerDistance = true;
		[Save]
		public static bool ShowPlayerWeapon = true;
		#endregion
	}
}