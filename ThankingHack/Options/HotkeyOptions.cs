using System.Collections.Generic;
using Thanking.Attributes;
using UnityEngine;

namespace Thanking.Options
{
    public static class HotkeyOptions
    {
        [Save] public static Dictionary<string, List<KeyCode>> HotkeyDict = new Dictionary<string, List<KeyCode>>();
    }
}