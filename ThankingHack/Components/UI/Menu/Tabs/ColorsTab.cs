using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Options.UIVariables;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class ColorsTab
    {
        static Vector2 ColorScroll;
        static Color LastColorPreview = ColorOption.preview.color;

        public static Color LastColorPreview1 { get => LastColorPreview; set => LastColorPreview = value; }

        public static void Tab()
        {
            ColorOption.errorColor = new ColorOption("errorColor", "#ERROR.NOTFOUND", Color.magenta);
            ColorOption.preview = new ColorOption("preview", "No Color Selected", Color.white);

            if (ColorOption.selectedOption != null)
            {

            }
            else
                ColorOption.previewselected = ColorOption.preview;

            Prefab.ScrollView(new Rect(0, 0, 250, 436), "Colors", ref ColorScroll, () =>
            {
                GUILayout.Space(10);
                var keylist = ColorOption.ColorDict.ToList();

                keylist.Sort((pair1, pair2) => pair1.Value.name.CompareTo(pair2.Value.name));

                for (int i = 0; i < keylist.Count; i++)
                {
                    ColorOption option = ColorOption.getColor(keylist[i].Value.identity);
                    if (Prefab.ColorButton(205, option))
                    {
                        ColorOption.selectedOption = option.identity;
                        ColorOption.previewselected = new ColorOption(option);
                        LastColorPreview1 = option.color;
                    }
                    GUILayout.Space(3);
                }

                if (Prefab.Button("Reset All Colors", 205))
                {
                    for (int i = 0; i < keylist.Count; i++)
                    {
                        ColorOption option = ColorOption.getColor(keylist[i].Value.identity);
                        option.color = option.origColor;
                        MenuComponent.SetGUIColors();
                        ColorOption.selectedOption = null;
                        LastColorPreview1 = ColorOption.preview.color;
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
                Drawing.DrawRect(new Rect(innerpreview.x + innerpreview.width / 2, innerpreview.y, innerpreview.width / 2, innerpreview.height), ColorOption.previewselected);
            });

            Prefab.MenuArea(new Rect(previewRect.x, previewRect.y + previewRect.height + 5, previewRect.width, 436 - previewRect.height - 5), ColorOption.previewselected.name, () =>
            {
                GUILayout.BeginArea(new Rect(10, 20, previewRect.width - 10, 205));
                ColorOption.previewselected.color.r = (byte)Prefab.TextField(ColorOption.previewselected.color.r, "R: ", 30);
                ColorOption.previewselected.color.r = (byte)Mathf.Round(Prefab.Slider(0, 255, ColorOption.previewselected.color.r, 185));
                GUILayout.FlexibleSpace();
                ColorOption.previewselected.color.g = (byte)Prefab.TextField(ColorOption.previewselected.color.g, "G: ", 30);
                ColorOption.previewselected.color.g = (byte)Mathf.Round(Prefab.Slider(0, 255, ColorOption.previewselected.color.g, 185));
                GUILayout.FlexibleSpace();
                ColorOption.previewselected.color.b = (byte)Prefab.TextField(ColorOption.previewselected.color.b, "B: ", 30);
                ColorOption.previewselected.color.b = (byte)Mathf.Round(Prefab.Slider(0, 255, ColorOption.previewselected.color.b, 185));
                GUILayout.FlexibleSpace();
                if (!ColorOption.previewselected.disableAlpha)
                {
                    ColorOption.previewselected.color.a = (byte)Prefab.TextField(ColorOption.previewselected.color.a, "A: ", 30);
                    ColorOption.previewselected.color.a = (byte)Mathf.Round(Prefab.Slider(0, 255, ColorOption.previewselected.color.a, 185));
                }
                else
                {
                    Prefab.TextField(ColorOption.previewselected.color.a, "A: ", 30);
                    Prefab.Slider(0, 255, ColorOption.previewselected.color.a, 185);
                }
                GUILayout.Space(100);
                GUILayout.EndArea();

                GUILayout.Space(160);
                GUILayout.FlexibleSpace();
                if (Prefab.Button("Deselect", 180))
                {
                    ColorOption.selectedOption = null;
                    LastColorPreview1 = ColorOption.preview.color;
                }
                GUILayout.Space(3);
                if (Prefab.Button("Reset", 180))
                {
                    ColorOption.setColor(ColorOption.previewselected.identity, ColorOption.previewselected.origColor);
                    ColorOption.previewselected.color = ColorOption.previewselected.origColor;
                    MenuComponent.SetGUIColors();
                }
                GUILayout.Space(3);
                if (Prefab.Button("Apply", 180))
                {
                    ColorOption.setColor(ColorOption.previewselected.identity, ColorOption.previewselected.color);
                    MenuComponent.SetGUIColors();
                    LastColorPreview1 = ColorOption.previewselected.color;
                }
                GUILayout.Space(30);
            });
        }
    }
}
