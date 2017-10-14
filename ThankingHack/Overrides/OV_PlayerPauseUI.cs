using SDG.Unturned;
using Thanking.Attributes;

namespace Thanking.Overrides
{
    public static class OV_PlayerPauseUI
    {
        [Override(typeof(PlayerPauseUI), "onClickedExitButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)]
        public static void OV_onClickedExitButton(SleekButton button) =>
            Provider.disconnect();
    }
}
