using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Framework.UI.Devkit.InspectorUI;
using SDG.Unturned;
using Thanking.Components.Basic;
using Thanking.Options;
using Thanking.Utilities;
using UnityEngine;
using Action = System.Action;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class HotkeyTab
    {
        public static Vector2 HotkeyScroll;
        public static string ClickedOption;
        
        public static void Tab()
        {
            Prefab.ScrollView(new Rect(0, 0, 466, 400), "Hotkeys", ref HotkeyScroll, () =>
            {
                DrawSpacer("Aimbot", true);
                
                DrawButton("Aimbot On/Off", "_ToggleAimbot");
                DrawButton("Aimbot Key On/Off", "_AimbotOnKey");
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
                
                DrawSpacer("Player Flight", false);
                
                DrawButton("Fly Up", "_FlyUp");
                DrawButton("Fly Down", "_FlyDown");
                DrawButton("Fly Left", "_FlyLeft");
                DrawButton("Fly Right", "_FlyRight");
                DrawButton("Fly Forward", "_FlyForward");
                DrawButton("Fly Backward", "_FlyBackward");
                
                DrawSpacer("Misc", false);
                
                DrawButton("Toggle All Visuals", "_PanicButton");
                DrawButton("Toggle Freecam", "_ToggleFreecam");
                DrawButton("Toggle Logo", "_ToggleLogo");
                
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
                    HotkeyComponent.Clear();
                    HotkeyOptions.HotkeyDict[Identifier] = new List<KeyCode>();

                    ClickedOption = "";
                }
                
                if (!HotkeyComponent.StopKeys)
                {
                    string kCode;

                    if (HotkeyOptions.HotkeyDict[Identifier].Count > 0)
                        kCode = string.Join(" + ",
                            HotkeyOptions.HotkeyDict[Identifier].Select(k => k.ToString()).ToArray());
                    else
                        kCode = "Unassigned";
                    
                    Prefab.Button(kCode, 150);
                }
                else
                {
                    HotkeyOptions.HotkeyDict[Identifier] = HotkeyComponent.CurrentKeys;
                    HotkeyComponent.Clear();

                    Prefab.Button(string.Join(" + ", HotkeyOptions.HotkeyDict[Identifier].Select(k => k.ToString()).ToArray()), 150);
                    ClickedOption = "";
                }
            }
            else
            {
                string kCode;

                if (HotkeyOptions.HotkeyDict[Identifier].Count > 0)
                    kCode = string.Join(" + ",
                        HotkeyOptions.HotkeyDict[Identifier].Select(k => k.ToString()).ToArray());
                else
                    kCode = "Unassigned";

                if (Prefab.Button(kCode, 150))
                {
                    HotkeyComponent.Clear();
                    
                    ClickedOption = Identifier;
                    HotkeyComponent.NeedsKeys = true;
                }
            }
            
            GUILayout.EndHorizontal();
            GUILayout.Space(2);
        }
    }
}