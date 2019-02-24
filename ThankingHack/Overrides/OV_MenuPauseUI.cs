using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using UnityEngine;

namespace Thanking.Overrides
{
	public static class OV_MenuPauseUI
	{
		[Override(typeof(MenuPauseUI), "onClickedExitButton", BindingFlags.NonPublic | BindingFlags.Static)]
		public static void OV_onClickedExitButton(SleekButton button) =>
			Application.Quit();
	}
}
