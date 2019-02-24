using System.Collections.Generic;
using System.Linq;
using Thanking.Components.Basic;
using Thanking.Options;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class HotkeyTab
    {
        public static Vector2 HotkeyScroll;
        public static string ClickedOption;
        public static bool IsFirst = true;
        
        public static void Tab()
        {
            Prefab.ScrollView(new Rect(0, 0, 466, 400), "Hotkeys", ref HotkeyScroll, () =>
            {
                foreach (KeyValuePair<string, Dictionary<string, Hotkey>> HotkeyGroup in HotkeyOptions.DefaultHotkeyDict)
                {
                    if (IsFirst)
                    {
                        IsFirst = false;
                        DrawSpacer(HotkeyGroup.Key, true);
                    }
                    
                    else
                        DrawSpacer(HotkeyGroup.Key, false);
                    
                    foreach (KeyValuePair<string, Hotkey> Hotkey in HotkeyGroup.Value)
                        DrawButton(Hotkey.Value.Name, Hotkey.Key);
                }
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
                if (Prefab.Button("Unassign", 75))
                {
                    HotkeyComponent.Clear();
                    HotkeyOptions.UnorganizedHotkeys[Identifier].Keys = new KeyCode[0];

                    ClickedOption = "";
                }
                
                if (!HotkeyComponent.StopKeys)
                {
                    string kCode;

                    if (HotkeyOptions.UnorganizedHotkeys[Identifier].Keys.Length > 0)
                        kCode = string.Join(" + ",
                            HotkeyOptions.UnorganizedHotkeys[Identifier].Keys.Select(k => k.ToString()).ToArray());
                    else
                        kCode = "Unassigned";
                    
                    Prefab.Button(kCode, 200);
                }
                else
                {
                    HotkeyOptions.UnorganizedHotkeys[Identifier].Keys = HotkeyComponent.CurrentKeys.ToArray();
                    HotkeyComponent.Clear();

                    Prefab.Button(string.Join(" + ", HotkeyOptions.UnorganizedHotkeys[Identifier].Keys.Select(k => k.ToString()).ToArray()), 200);
                    ClickedOption = "";
                }
            }
            else
            {
                string kCode;

                if (HotkeyOptions.UnorganizedHotkeys[Identifier].Keys.Length > 0)
                    kCode = string.Join(" + ",
                        HotkeyOptions.UnorganizedHotkeys[Identifier].Keys.Select(k => k.ToString()).ToArray());
                else
                    kCode = "Unassigned";

                if (Prefab.Button(kCode, 200))
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