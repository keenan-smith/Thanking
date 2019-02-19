using System.Diagnostics;
using System.Reflection;
using SDG.Unturned;
using Thinking.Attributes;
using UnityEngine;

namespace Thinking.Overrides
{
	public static class OV_MenuPauseUI
	{
		[Override(typeof(MenuPauseUI), "onClickedExitButton", BindingFlags.NonPublic | BindingFlags.Static)]
		public static void OV_onClickedExitButton(SleekButton button) =>
			Application.Quit();
	}
}
