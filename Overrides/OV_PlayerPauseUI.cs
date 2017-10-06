using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;

namespace Thanking.Overrides
{
    public static class OV_PlayerPauseUI
    {
        [Override(typeof(SDG.Unturned.PlayerPauseUI), "onClickedExitButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)]
        public static void OV_onClickedExitButton(SleekButton button) =>
            Provider.disconnect();
    }
}
