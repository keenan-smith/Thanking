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
            AddHotkey("Toggle Aimbot", "_ToggleAimbot", KeyCode.F);
            AddHotkey("Toggle Aimbot on Key", "_AimbotOnKey", KeyCode.F);
            AddHotkey("Aimbot Key", "_AimbotKey", KeyCode.F);

            AddHotkey("Strafe Up", "_VFStrafeUp", KeyCode.RightControl);
            AddHotkey("Strafe Down", "_VFStrafeDown", KeyCode.LeftControl);
            AddHotkey("Strafe Left", "_VFStrafeLeft", KeyCode.LeftBracket);
            AddHotkey("Strafe Right", "_VFStrafeRight", KeyCode.RightBracket);
            AddHotkey("Move Forward", "_VFMoveForward", KeyCode.W);
            AddHotkey("Move Backward", "_VFMoveBackward", KeyCode.S);
            AddHotkey("Rotate Left", "_VFRotateLeft", KeyCode.A);
            AddHotkey("Rotate Right", "_VFRotateRight", KeyCode.D);
            AddHotkey("Roll Left", "_VFRollLeft", KeyCode.Q);
            AddHotkey("Roll Right", "_VFRollRight", KeyCode.E);
            AddHotkey("Rotate Up", "_VFRotateUp", KeyCode.Space);
            AddHotkey("Rotate Down", "_VFRotateDown", KeyCode.LeftShift);

            AddHotkey("Crash Server", "_CrashServer", KeyCode.Keypad0);
            AddHotkey("Toggle All Visuals", "_PanicButton", KeyCode.RightControl);
        }

        public static void GetNextKeyDown() =>
            NeedsKey = true;

        public static void AddHotkey(string Label, string Identifier, KeyCode DefaultKey)
        {
            if (!HotkeyOptions.HotkeyDict.ContainsKey(Identifier))
                HotkeyOptions.HotkeyDict.Add(Identifier, DefaultKey);
        }
    }
}