using Thanking.Attributes;
using Thanking.Options;
using UnityEngine;

namespace Thanking.Utilities
{
    public static class HotkeyUtilities
    {
        public static bool NeedsKey;
        public static KeyCode ReturnKey = KeyCode.None;

        [Initializer]
        public static void Initialize()
        {
            AddHotkey("_ToggleAimbot", KeyCode.Period);
            AddHotkey("_AimbotOnKey", KeyCode.Comma);
            AddHotkey("_AimbotKey", KeyCode.F);

            AddHotkey("_VFStrafeUp", KeyCode.RightControl);
            AddHotkey("_VFStrafeDown", KeyCode.LeftControl);
            AddHotkey("_VFStrafeLeft", KeyCode.LeftBracket);
            AddHotkey("_VFStrafeRight", KeyCode.RightBracket);
            AddHotkey("_VFMoveForward", KeyCode.W);
            AddHotkey("_VFMoveBackward", KeyCode.S);
            AddHotkey("_VFRotateLeft", KeyCode.A);
            AddHotkey("_VFRotateRight", KeyCode.D);
            AddHotkey("_VFRollLeft", KeyCode.Q);
            AddHotkey("_VFRollRight", KeyCode.E);
            AddHotkey("_VFRotateUp", KeyCode.Space);
            AddHotkey("_VFRotateDown", KeyCode.LeftShift);

            AddHotkey("_PanicButton", KeyCode.RightControl);
            AddHotkey("_ToggleFreecam", KeyCode.Keypad2);
            AddHotkey("_ToggleLogo", KeyCode.Keypad5);
            
            AddHotkey("_PanicButton", KeyCode.RightControl);
            AddHotkey("_ToggleFreecam", KeyCode.Keypad2);
            AddHotkey("_ToggleLogo", KeyCode.Keypad5);
            
            AddHotkey("_SPNextPlayer", KeyCode.Keypad9);
            AddHotkey("_SPLastPlayer", KeyCode.Keypad7);
        }

        public static void GetNextKeyDown() =>
            NeedsKey = true;

        public static void AddHotkey(string Identifier, KeyCode DefaultKey)
        {
            if (!HotkeyOptions.HotkeyDict.ContainsKey(Identifier))
                HotkeyOptions.HotkeyDict.Add(Identifier, DefaultKey);
        }
    }
}