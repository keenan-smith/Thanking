using System;
using System.Linq;
using Thanking.Options.VisualOptions;
using Thanking.Utilities;
using Thanking.Variables.UIVariables;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class ColorsTab
    {
        static Vector2 ColorScroll;
        static Color LastColorPreview = ColorOptions.preview.color;

        public static Color LastColorPreview1 { get => LastColorPreview; set => LastColorPreview = value; }

        public static void Tab()
        {
            if (ColorOptions.selectedOption == null)
                ColorOptions.previewselected = ColorOptions.preview;
                

            Prefab.ScrollView(new Rect(0, 0, 250, 436), "Colors", ref ColorScroll, () =>
            {
                GUILayout.Space(10);
                
                var keylist = ColorOptions.ColorDict.ToList();
                keylist.Sort((pair1, pair2) => String.Compare(pair1.Value.name, pair2.Value.name, StringComparison.Ordinal));

                for (int i = 0; i < keylist.Count; i++)
                {
                    ColorVariable option = ColorUtilities.getColor(keylist[i].Value.identity);
                    if (Prefab.ColorButton(205, option))
                    {
                        ColorOptions.selectedOption = option.identity;
                        ColorOptions.previewselected = new ColorVariable(option);
                        LastColorPreview1 = option.color;
                    }
                    GUILayout.Space(3);
                }

                if (Prefab.Button("Reset All Colors", 205))
                {
                    for (int i = 0; i < keylist.Count; i++)
                    {
                        ColorVariable option = ColorUtilities.getColor(keylist[i].Value.identity);
                        option.color = option.origColor;
                        MenuComponent.SetGUIColors();
                        ColorOptions.selectedOption = null;
                        LastColorPreview1 = ColorOptions.preview.color;
                    }
                }
                GUILayout.Space(10);
            });

            Rect previewRect = new Rect(250 + 5, 0, 466 - 250 - 5, 120);
            Prefab.MenuArea(previewRect, "Preview", () =>
            {
                Rect colorRect = new Rect(5, 20, previewRect.width - 10, previewRect.height - 25);
                Drawing.DrawRect(colorRect, MenuComponent._OutlineBorderBlack);
                Drawing.DrawRect(MenuUtilities.Inline(colorRect), MenuComponent._OutlineBorderLightGray);
                Rect innerpreview = MenuUtilities.Inline(colorRect, 2);
                Drawing.DrawRect(new Rect(innerpreview.x, innerpreview.y, innerpreview.width / 2, innerpreview.height), LastColorPreview1);
                Drawing.DrawRect(new Rect(innerpreview.x + innerpreview.width / 2, innerpreview.y, innerpreview.width / 2, innerpreview.height), ColorOptions.previewselected);
            });

            Prefab.MenuArea(new Rect(previewRect.x, previewRect.y + previewRect.height + 5, previewRect.width, 436 - previewRect.height - 5), ColorOptions.previewselected.name, () =>
            {
                GUILayout.BeginArea(new Rect(10, 20, previewRect.width - 10, 205));
                ColorOptions.previewselected.color.r = (byte)Prefab.TextField(ColorOptions.previewselected.color.r, "R: ", 30);
                ColorOptions.previewselected.color.r = (byte)Mathf.Round(Prefab.Slider(0, 255, ColorOptions.previewselected.color.r, 185));
                GUILayout.FlexibleSpace();
                ColorOptions.previewselected.color.g = (byte)Prefab.TextField(ColorOptions.previewselected.color.g, "G: ", 30);
                ColorOptions.previewselected.color.g = (byte)Mathf.Round(Prefab.Slider(0, 255, ColorOptions.previewselected.color.g, 185));
                GUILayout.FlexibleSpace();
                ColorOptions.previewselected.color.b = (byte)Prefab.TextField(ColorOptions.previewselected.color.b, "B: ", 30);
                ColorOptions.previewselected.color.b = (byte)Mathf.Round(Prefab.Slider(0, 255, ColorOptions.previewselected.color.b, 185));
                GUILayout.FlexibleSpace();
                if (!ColorOptions.previewselected.disableAlpha)
                {
                    ColorOptions.previewselected.color.a = (byte)Prefab.TextField(ColorOptions.previewselected.color.a, "A: ", 30);
                    ColorOptions.previewselected.color.a = (byte)Mathf.Round(Prefab.Slider(0, 255, ColorOptions.previewselected.color.a, 185));
                }
                else
                {
                    Prefab.TextField(ColorOptions.previewselected.color.a, "A: ", 30);
                    Prefab.Slider(0, 255, ColorOptions.previewselected.color.a, 185);
                }
                GUILayout.Space(100);
                GUILayout.EndArea();

                GUILayout.Space(160);
                GUILayout.FlexibleSpace();
                if (Prefab.Button("Deselect", 180))
                {
                    ColorOptions.selectedOption = null;
                    LastColorPreview1 = ColorOptions.preview.color;
                }
                GUILayout.Space(3);
                if (Prefab.Button("Reset", 180))
                {
					ColorUtilities.setColor(ColorOptions.previewselected.identity, ColorOptions.previewselected.origColor);
                    ColorOptions.previewselected.color = ColorOptions.previewselected.origColor;
                    MenuComponent.SetGUIColors();
                }
                GUILayout.Space(3);
                if (Prefab.Button("Apply", 180))
                {
					ColorUtilities.setColor(ColorOptions.previewselected.identity, ColorOptions.previewselected.color);
                    MenuComponent.SetGUIColors();
                    LastColorPreview1 = ColorOptions.previewselected.color;
                }
                GUILayout.Space(30);
            });
        }
    }
}
