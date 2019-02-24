using System.Collections.Generic;
using Thanking.Attributes;
using Thanking.Variables;

namespace Thanking.Options
{
    public static class HotkeyOptions
    {
        public static Dictionary<string, Dictionary<string, Hotkey>> DefaultHotkeyDict = new Dictionary<string, Dictionary<string, Hotkey>>();

        [Save] public static Dictionary<string, Hotkey> UnorganizedHotkeys = new Dictionary<string, Hotkey>();
    }
}