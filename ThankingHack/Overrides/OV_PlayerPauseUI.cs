using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;

namespace Thanking.Overrides
{
    public static class OV_PlayerPauseUI
    {
        [Override(typeof(PlayerPauseUI), "onClickedExitButton", BindingFlags.NonPublic | BindingFlags.Static)]
        public static void OV_onClickedExitButton(SleekButton button) =>
            Provider.disconnect();
    }
}
