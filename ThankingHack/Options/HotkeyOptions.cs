using System.Collections.Generic;
using Thanking.Attributes;
using UnityEngine;

namespace Thanking.Options
{
    public static class HotkeyOptions
    {
        [Save] public static Dictionary<string, KeyCode> HotkeyDict = new Dictionary<string, KeyCode>();
    }
}