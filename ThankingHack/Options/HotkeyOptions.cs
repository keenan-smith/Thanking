using System.Collections.Generic;
using Thanking.Attributes;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Options
{
    public static class HotkeyOptions
    {
        [Save] public static Dictionary<string, Dictionary<string, Hotkey>> HotkeyDict =
            new Dictionary<string, Dictionary<string, Hotkey>>();

        [Save] public static Dictionary<string, Hotkey> UnorganizedHotkeys = new Dictionary<string, Hotkey>();
    }
}