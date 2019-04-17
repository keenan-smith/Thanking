using System;
using System.Collections.Generic;
using System.Linq;
using Thanking.Attributes;
using Thanking.Misc;
using Thanking.Misc.Classes.ESP;
using Thanking.Misc.Enums;
using Thanking.Misc.Serializables;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Options.VisualOptions
{
	public static class ESPOptions
	{
		#region Global Options

		[Save] public static bool Enabled = true;
		//[Save] public static KeyCode Toggle = KeyCode.LeftBracket;
        [Save] public static bool ChamsEnabled = true;
        [Save] public static bool ChamsFlat = false;
        [Save] public static bool ShowVanishPlayers = true;
        [Save] public static bool IgnoreZ = false;

        [Save]
        public static ESPVisual[] VisualOptions = Enumerable.Repeat(new ESPVisual
        {
            Enabled = false,
            Labels = true,
            Boxes = true,
            ShowName = true,
            ShowDistance = true,
	        ShowAngle = false,
            TwoDimensional = true,
            Glow = false,
            InfiniteDistance = false,
            LineToObject = false,
            TextScaling = true,
            UseObjectCap = true,
	        CustomTextColor = false,

            Distance = 250,
            Location = LabelLocation.BottomMiddle,

            FixedTextSize = 11,
            MinTextSize = 8,
            MaxTextSize = 11,
            MinTextSizeDistance = 800,
            BorderStrength = 2,
            ObjectCap = 24
        }, Enum.GetValues(typeof(ESPTarget)).Length).ToArray();

        [Save]
        public static Dictionary<ESPTarget, int> PriorityTable = Enum.GetValues(typeof(ESPTarget)).Cast<ESPTarget>().ToDictionary(x => x, x => (int)x);

		#endregion

		#region Player Options

		[Save] public static bool ShowPlayerWeapon = true;
		[Save] public static bool ShowPlayerVehicle = true;
		[Save] public static bool UsePlayerGroup = true;

		[Save] public static SerializableColor SameGroupColor = ((Color32) Color.green).ToSerializableColor();

        #endregion

        #region Item Options

        [Save] public static bool FilterItems = false;

        #endregion
		
		#region Vehicle Options

		[Save] public static bool ShowVehicleFuel;
		[Save] public static bool ShowVehicleHealth;
		[Save] public static bool ShowVehicleLocked;
		[Save] public static bool FilterVehicleLocked;

		#endregion
		
		#region Sentry Options

		[Save] public static bool ShowSentryItem;

		#endregion
		
		#region Bed Options

		[Save] public static bool ShowClaimed;

		#endregion
		
		#region Generator Options

		[Save] public static bool ShowGeneratorFuel;
		[Save] public static bool ShowGeneratorPowered;

		#endregion
	}
}