using UnityEngine;

namespace Thanking.Utilities
{
    public static class HotkeyUtilities
    {
        public static bool NeedsKey;
        public static KeyCode ReturnKey = KeyCode.None;

        public static void GetNextKeyDown() =>
            NeedsKey = true;
    }
}