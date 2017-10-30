using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Thanking.Utilities
{
	public static class PlayerUtilities
	{
		public static void UpdateCrosshairInstant(float spread)
		{
			Debug.Log(spread);
			if (PlayerLifeUI.crosshairLeftImage.positionOffset_X == (int)(-spread * 400f) - 4)
				return;

			PlayerLifeUI.crosshairLeftImage.positionOffset_X = (int)(-spread * 400f) - 4;
			PlayerLifeUI.crosshairLeftImage.positionOffset_Y = -4;

			PlayerLifeUI.crosshairRightImage.positionOffset_X = (int)(spread * 400f) - 4;
			PlayerLifeUI.crosshairRightImage.positionOffset_Y = -4;

			PlayerLifeUI.crosshairUpImage.positionOffset_X = -4;
			PlayerLifeUI.crosshairUpImage.positionOffset_Y = (int)(-spread * 400f) - 4;

			PlayerLifeUI.crosshairDownImage.positionOffset_X = -4;
			PlayerLifeUI.crosshairDownImage.positionOffset_Y = -(int)(spread * 400f) - 4;
		}
	}
}
