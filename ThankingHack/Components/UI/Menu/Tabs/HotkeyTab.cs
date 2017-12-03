using System;
using JetBrains.Annotations;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Framework.UI.Devkit.InspectorUI;
using SDG.Unturned;
using Thanking.Options;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class HotkeyTab
    {
        public static bool IsInitialized = false;
        
        public static Vector2 HotkeyScroll;
        public static string ClickedOption;
        
        public static void Tab()
        {
            Prefab.ScrollView(new Rect(0, 0, 466, 400), "Hotkeys", ref HotkeyScroll, () =>
            {
                Prefab._TextStyle.fontStyle = FontStyle.Bold;
                GUILayout.Label("Aimbot", Prefab._TextStyle);
                Prefab._TextStyle.fontStyle = FontStyle.Normal;
                GUILayout.Space(8);
                
                DrawButton("Toggle Aimbot", "_ToggleAimbot");
                DrawButton("Aimbot on Key", "_AimbotOnKey");
                DrawButton("Aimbot Key", "_AimbotKey");
                
                GUILayout.Space(10);
                Prefab._TextStyle.fontStyle = FontStyle.Bold;
                GUILayout.Label("Vehicle Flight", Prefab._TextStyle);
                Prefab._TextStyle.fontStyle = FontStyle.Normal;
                GUILayout.Space(8);
                DrawButton("Strafe Up", "_VFStrafeUp", KeyCode.RightControl);
                DrawButton("Strafe Down", "_VFStrafeDown", KeyCode.LeftControl);
                DrawButton("Strafe Left", "_VFStrafeLeft", KeyCode.LeftBracket);
                DrawButton("Strafe Right", "_VFStrafeRight", KeyCode.RightBracket);
                DrawButton("Move Forward", "_VFMoveForward", KeyCode.W);
                DrawButton("Move Backward", "_VFMoveBackward", KeyCode.S);
                DrawButton("Rotate Left", "_VFRotateLeft", KeyCode.A);
                DrawButton("Rotate Right", "_VFRotateRight", KeyCode.D);
                DrawButton("Roll Left", "_VFRollLeft", KeyCode.Q);
                DrawButton("Roll Right", "_VFRollRight", KeyCode.E);
                DrawButton("Rotate Up", "_VFRotateUp", KeyCode.Space);
                DrawButton("Rotate Down", "_VFRotateDown", KeyCode.LeftShift);

                IsInitialized = true;
            });
        }

        public static void DrawButton(string Option, string Identifier, KeyCode DefaultKey = KeyCode.None)
        {
            if (!HotkeyOptions.HotkeyDict.ContainsKey(Identifier))
                HotkeyOptions.HotkeyDict.Add(Identifier, DefaultKey);
                
            GUILayout.BeginHorizontal();

            GUILayout.Label(Option, Prefab._TextStyle);
            
            if (ClickedOption == Identifier)
            {
                if (Prefab.Button("Unassign", 100))
                {
                    HotkeyUtilities.ReturnKey = KeyCode.None;
                    HotkeyOptions.HotkeyDict[Identifier] = KeyCode.None;

                    ClickedOption = "";
                    return;
                }
                
                KeyCode key = HotkeyUtilities.ReturnKey;

                switch (key)
                {
                    case KeyCode.None:
                        Prefab.Button("...", 150);
                        break;
                    case KeyCode.Mouse0:
                    case KeyCode.Mouse1:
                    case KeyCode.Mouse2:
                    case KeyCode.Mouse3:
                    case KeyCode.Mouse4:
                    case KeyCode.Mouse5:
                    case KeyCode.Mouse6:
                        Prefab.Button(key.ToString(), 150);
                        HotkeyOptions.HotkeyDict[Identifier] = key;
                        break;
                    default:
                        HotkeyUtilities.ReturnKey = KeyCode.None;

                        Prefab.Button(key.ToString(), 150);
                        HotkeyOptions.HotkeyDict[Identifier] = key;

                        ClickedOption = "";
                        break;
                }
            }
            else
            {
                KeyCode key = HotkeyOptions.HotkeyDict[Identifier];
                if (Prefab.Button(key.ToString(), 150))
                {
                    ClickedOption = Identifier;
                    HotkeyUtilities.GetNextKeyDown();
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(2);
        }
    }
}