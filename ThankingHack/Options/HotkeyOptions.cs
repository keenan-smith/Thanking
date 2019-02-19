using System.Collections.Generic;
using Thinking.Attributes;
using Thinking.Variables;
using UnityEngine;

namespace Thinking.Options
{
    public static class HotkeyOptions
    {
        [Save] public static Dictionary<string, Dictionary<string, Hotkey>> HotkeyDict =
            new Dictionary<string, Dictionary<string, Hotkey>>();

        [Save] public static Dictionary<string, Hotkey> UnorganizedHotkeys = new Dictionary<string, Hotkey>();
    }
}