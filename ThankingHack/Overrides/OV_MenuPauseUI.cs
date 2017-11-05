using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Thanking.Attributes;

namespace Thanking.Overrides
{
	public static class OV_MenuPauseUI
	{
		[Override(typeof(MenuPauseUI), "onClickedExitButton", BindingFlags.NonPublic | BindingFlags.Static)]
		public static void OV_onClickedExitButton(SleekButton button) =>
			Process.GetCurrentProcess().Kill();
	}
}
