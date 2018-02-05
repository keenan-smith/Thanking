using System;
using JetBrains.Annotations;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Framework.UI.Devkit.InspectorUI;
using SDG.Unturned;
using Thanking.Options;
using Thanking.Utilities;
using UnityEngine;
using Action = System.Action;

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
                DrawSpacer("Aimbot", true);
                
                DrawButton("Toggle Aimbot", "_ToggleAimbot");
                DrawButton("Toggle Aimbot on Key", "_AimbotOnKey");
                DrawButton("Aimbot Key", "_AimbotKey");
                
                DrawSpacer("Vehicle Flight", false);
                
                DrawButton("Strafe Up", "_VFStrafeUp");
                DrawButton("Strafe Down", "_VFStrafeDown");
                DrawButton("Strafe Left", "_VFStrafeLeft");
                DrawButton("Strafe Right", "_VFStrafeRight");
                DrawButton("Move Forward", "_VFMoveForward");
                DrawButton("Move Backward", "_VFMoveBackward");
                DrawButton("Rotate Left", "_VFRotateLeft");
                DrawButton("Rotate Right", "_VFRotateRight");
                DrawButton("Roll Left", "_VFRollLeft");
                DrawButton("Roll Right", "_VFRollRight");
                DrawButton("Rotate Up", "_VFRotateUp");
                DrawButton("Rotate Down", "_VFRotateDown");
                
                DrawSpacer("Misc", false);
                
                DrawButton("Toggle All Visuals", "_PanicButton");
                DrawButton("Toggle Freecam", "_ToggleFreecam");
                DrawButton("Toggle Logo", "_ToggleLogo");
                
                DrawButton("Spectate Next Player", "_SPNextPlayer");
                DrawButton("Spectate Last Player", "_SPLastPlayer");
                
                IsInitialized = true;
            });
        }

        public static void DrawSpacer(string Text, bool First)
        {
            if(!First)
                GUILayout.Space(10);
            
            Prefab._TextStyle.fontStyle = FontStyle.Bold;
            GUILayout.Label(Text, Prefab._TextStyle);
            Prefab._TextStyle.fontStyle = FontStyle.Normal;
            GUILayout.Space(8);
        }
        
        public static void DrawButton(string Option, string Identifier)
        {
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