﻿using System;
using SDG.Unturned;
using System.Reflection;
using Thanking.Attributes;
using Thanking.Options.AimOptions;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Overrides
{
	public class OV_PlayerUI
	{
		[Override(typeof(PlayerUI), "updateCrosshair", BindingFlags.Public | BindingFlags.Static)]
		public static void updateCrosshair(float spread)
		{
			if (Provider.modeConfigData.Gameplay.Crosshair)
			{
				PlayerLifeUI.crosshairLeftImage.positionOffset_X = ((int)(-spread * 400f) - 4);
				PlayerLifeUI.crosshairLeftImage.positionOffset_Y = -4;

				PlayerLifeUI.crosshairRightImage.positionOffset_X = ((int)(spread * 400f) - 4);
				PlayerLifeUI.crosshairRightImage.positionOffset_Y = -4;

				PlayerLifeUI.crosshairUpImage.positionOffset_X = -4;
				PlayerLifeUI.crosshairUpImage.positionOffset_Y = ((int)(-spread * 400f) - 4);

				PlayerLifeUI.crosshairDownImage.positionOffset_X = -4;
				PlayerLifeUI.crosshairDownImage.positionOffset_Y = (-(int)(spread * 400f) - 4);
			}
		}
	}
}
