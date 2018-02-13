using System.Collections.Generic;
using System.Linq;
using Thanking.Attributes;
using Thanking.Options;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Utilities
{
    public static class HotkeyUtilities
    {
        [Initializer]
        public static void Initialize()
        {
            AddHotkey("Aimbot", "Aimbot On/Off", "_ToggleAimbot", KeyCode.Period);
            AddHotkey("Aimbot", "Aimbot On Key On/Off", "_AimbotOnKey", KeyCode.Comma);
            AddHotkey("Aimbot", "Aimbot Key", "_AimbotKey", KeyCode.F);

            AddHotkey("Vehicle Flight", "Toggle Vehicle Flight", "_VFToggle", KeyCode.Slash);
            AddHotkey("Vehicle Flight", "Strafe Up", "_VFStrafeUp", KeyCode.RightControl);
            AddHotkey("Vehicle Flight", "Strafe Down","_VFStrafeDown", KeyCode.LeftControl);
            AddHotkey("Vehicle Flight", "Strafe Left","_VFStrafeLeft", KeyCode.LeftBracket);
            AddHotkey("Vehicle Flight", "Strafe Right","_VFStrafeRight", KeyCode.RightBracket);
            AddHotkey("Vehicle Flight", "Move Forward","_VFMoveForward", KeyCode.W);
            AddHotkey("Vehicle Flight", "Move Backward","_VFMoveBackward", KeyCode.S);
            AddHotkey("Vehicle Flight", "Rotate Left","_VFRotateLeft", KeyCode.A);
            AddHotkey("Vehicle Flight", "Rotate Right","_VFRotateRight", KeyCode.D);
            AddHotkey("Vehicle Flight", "Rotate Up","_VFRotateUp", KeyCode.Space);
            AddHotkey("Vehicle Flight", "Rotate Down","_VFRotateDown", KeyCode.LeftShift);
            AddHotkey("Vehicle Flight", "Roll Left","_VFRollLeft", KeyCode.Q);
            AddHotkey("Vehicle Flight", "Roll Right","_VFRollRight", KeyCode.E);
            
            AddHotkey("Player Flight", "Fly Up", "_FlyUp", KeyCode.Space);
            AddHotkey("Player Flight", "Fly Down", "_FlyDown", KeyCode.LeftControl);
            AddHotkey("Player Flight", "Fly Left", "_FlyLeft", KeyCode.A);
            AddHotkey("Player Flight", "Fly Right", "_FlyRight", KeyCode.D);
            AddHotkey("Player Flight", "Fly Forward", "_FlyForward", KeyCode.W);
            AddHotkey("Player Flight", "Fly Backward", "_FlyBackward", KeyCode.S);
            
            AddHotkey("Misc", "Toggle All Visuals", "_PanicButton", KeyCode.Keypad0);
            AddHotkey("Misc", "Toggle Freecam", "_ToggleFreecam", KeyCode.Keypad2);
            AddHotkey("Misc", "Select Player", "_SelectPlayer", KeyCode.LeftAlt);
        }
        
        public static void AddHotkey(string Group, string Name, string Identifier, params KeyCode[] DefaultKeys)
        {
            if (!HotkeyOptions.HotkeyDict.ContainsKey(Group))
                HotkeyOptions.HotkeyDict.Add(Group, new Dictionary<string, Hotkey>());

            Dictionary<string, Hotkey> GroupHotkeys = HotkeyOptions.HotkeyDict[Group];
            
            if (GroupHotkeys.ContainsKey(Identifier)) 
                return;

            Hotkey HKey = new Hotkey
            {
                Name = Name,
                Keys = DefaultKeys
            };
            
            GroupHotkeys.Add(Identifier, HKey);
            HotkeyOptions.UnorganizedHotkeys.Add(Identifier, HKey);
        }

        public static bool IsHotkeyDown(string Identifier) => 
            HotkeyOptions.UnorganizedHotkeys[Identifier].Keys.Any(Input.GetKeyDown) && 
            HotkeyOptions.UnorganizedHotkeys[Identifier].Keys.All(Input.GetKey);

        public static bool IsHotkeyHeld(string Identifier) => 
            HotkeyOptions.UnorganizedHotkeys[Identifier].Keys.All(Input.GetKey);
    }
}