using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;

namespace Thanking.Overrides
{
	public class OV_PlayerUI
	{
		[Override(typeof(PlayerUI), "updateCrosshair", BindingFlags.Public | BindingFlags.Static)]
		public static void OV_updateCrosshair(float spread)
		{
			if (!Provider.modeConfigData.Gameplay.Crosshair) return;
			
			PlayerLifeUI.crosshairLeftImage.positionOffset_X = (int)(-spread * 400f) - 4;
			PlayerLifeUI.crosshairLeftImage.positionOffset_Y = -4;

			PlayerLifeUI.crosshairRightImage.positionOffset_X = (int)(spread * 400f) - 4;
			PlayerLifeUI.crosshairRightImage.positionOffset_Y = -4;

			PlayerLifeUI.crosshairUpImage.positionOffset_X = -4;
			PlayerLifeUI.crosshairUpImage.positionOffset_Y = (int)(-spread * 400f) - 4;

			PlayerLifeUI.crosshairDownImage.positionOffset_X = -4;
			PlayerLifeUI.crosshairDownImage.positionOffset_Y = (int)(spread * 400f) - 4;
		}
	}
}
