using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Thanking.Misc;
using Thanking.Options.UIVariables;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.UI.Menu
{
    public static class Prefab
    {
        public static GUIStyle _MenuTabStyle;
        public static GUIStyle _HeaderStyle;
        public static GUIStyle _TextStyle;
        public static GUIStyle _sliderStyle;
        public static GUIStyle _sliderThumbStyle;
        public static GUIStyle _sliderVThumbStyle;
        public static GUIStyle _listStyle;
        public static GUIStyle _ButtonStyle;
        public static Color32 _ToggleBoxBG;

        static int popupListHash = "PopupList".GetHashCode();
        public static Regex digitsOnly = new Regex(@"[^\d]");

        static Prefab()
        {
            _MenuTabStyle = new GUIStyle();
            _MenuTabStyle.font = MenuComponent._TabFont;
            _MenuTabStyle.fontSize = 29;

            _HeaderStyle = new GUIStyle();
            _HeaderStyle.font = MenuComponent._TabFont;
            _HeaderStyle.fontSize = 15;
            _HeaderStyle.alignment = TextAnchor.MiddleCenter;

            _TextStyle = new GUIStyle();
            _TextStyle.font = MenuComponent._TextFont;
            _TextStyle.fontSize = 17;

            _sliderStyle = new GUIStyle();
            _sliderThumbStyle = new GUIStyle(GUI.skin.horizontalSliderThumb);
            _sliderThumbStyle.fixedWidth = 7;

            _sliderVThumbStyle = new GUIStyle(GUI.skin.verticalSliderThumb);
            _sliderVThumbStyle.fixedHeight = 7;

            _listStyle = new GUIStyle();
            _listStyle.padding.left = _listStyle.padding.right = _listStyle.padding.top = _listStyle.padding.bottom = 4;
            _listStyle.alignment = TextAnchor.MiddleLeft;
            _listStyle.font = MenuComponent._TextFont;
            _listStyle.fontSize = 15;

            _ButtonStyle = new GUIStyle();
            _ButtonStyle.alignment = TextAnchor.MiddleLeft;
            _ButtonStyle.font = MenuComponent._TextFont;
            _ButtonStyle.fontSize = 15;
            _ButtonStyle.padding.left = _ButtonStyle.padding.right = _ButtonStyle.padding.top = _ButtonStyle.padding.bottom = 4;

            MenuUtilities.FixGUIStyleColor(_sliderStyle);
            MenuUtilities.FixGUIStyleColor(_MenuTabStyle);
            MenuUtilities.FixGUIStyleColor(_TextStyle);
            ColorUtilities.addColor(new ColorVariable("_MenuTabOff", "Menu Tab - Off", new Color32(160, 160, 160, 255)));
            ColorUtilities.addColor(new ColorVariable("_MenuTabOn", "Menu Tab - On", new Color32(255, 255, 255, 255)));
            ColorUtilities.addColor(new ColorVariable("_MenuTabHover", "Menu Tab - Hover", new Color32(210, 210, 210, 255)));
            ColorUtilities.addColor(new ColorVariable("_TextStyleOff", "Menu Labels - Off", new Color32(160, 160, 160, 255)));
            ColorUtilities.addColor(new ColorVariable("_TextStyleOn", "Menu Labels - On", new Color32(255, 255, 255, 255)));
            ColorUtilities.addColor(new ColorVariable("_TextStyleHover", "Menu Labels - Hover", new Color32(210, 210, 210, 255)));
            ColorUtilities.addColor(new ColorVariable("_HeaderStyle", "Menu Area - Header", new Color32(210, 210, 210, 255)));
            ColorUtilities.addColor(new ColorVariable("_ToggleBoxBG", "Menu Toggle - Background", new Color32(71, 70, 71, 255)));
            ColorUtilities.addColor(new ColorVariable("_ButtonBG", "Menu Button - Background", new Color32(130, 130, 130, 255)));

            UpdateColors();
        }

        public static void UpdateColors()
        {
            _MenuTabStyle.normal.textColor = ColorUtilities.getColor("_MenuTabOff");
            _MenuTabStyle.onNormal.textColor = ColorUtilities.getColor("_MenuTabOn");
            _MenuTabStyle.hover.textColor = ColorUtilities.getColor("_MenuTabHover");
            _MenuTabStyle.onHover.textColor = ColorUtilities.getColor("_MenuTabOn");
            _MenuTabStyle.active.textColor = ColorUtilities.getColor("_MenuTabOn");
            _MenuTabStyle.onActive.textColor = ColorUtilities.getColor("_MenuTabOn");
            _MenuTabStyle.focused.textColor = ColorUtilities.getColor("_MenuTabOff");
            _MenuTabStyle.onFocused.textColor = ColorUtilities.getColor("_MenuTabOff");

            _TextStyle.normal.textColor = ColorUtilities.getColor("_TextStyleOff");
            _TextStyle.onNormal.textColor = ColorUtilities.getColor("_TextStyleOn");
            _TextStyle.hover.textColor = ColorUtilities.getColor("_TextStyleHover");
            _TextStyle.onHover.textColor = ColorUtilities.getColor("_TextStyleOn");
            _TextStyle.active.textColor = ColorUtilities.getColor("_TextStyleOn");
            _TextStyle.onActive.textColor = ColorUtilities.getColor("_TextStyleOn");
            _TextStyle.focused.textColor = ColorUtilities.getColor("_TextStyleOff");
            _TextStyle.onFocused.textColor = ColorUtilities.getColor("_TextStyleOff");

            _HeaderStyle.normal.textColor = ColorUtilities.getColor("_HeaderStyle");
            _listStyle.normal.textColor = ColorUtilities.getColor("_TextStyleOn");
            _listStyle.onNormal.textColor = ColorUtilities.getColor("_TextStyleOn");
            _listStyle.hover.textColor = ColorUtilities.getColor("_OutlineBorderBlack");

            _ButtonStyle.normal.textColor = ColorUtilities.getColor("_TextStyleOn");
            _ButtonStyle.onNormal.textColor = ColorUtilities.getColor("_TextStyleOn");
            _ButtonStyle.hover.textColor = ColorUtilities.getColor("_OutlineBorderBlack");
            _ButtonStyle.onHover.textColor = ColorUtilities.getColor("_OutlineBorderBlack");

            var btex = new Texture2D(1, 1);
            btex.SetPixel(0, 0, ColorUtilities.getColor("_TextStyleHover"));
            btex.Apply();
            _ButtonStyle.hover.background = btex;
            var btex2 = new Texture2D(1, 1);
            btex2.SetPixel(0, 0, ColorUtilities.getColor("_ButtonBG"));
            btex2.Apply();
            _ButtonStyle.normal.background = btex2;
            var btex3 = new Texture2D(1, 1);
            btex3.SetPixel(0, 0, ColorUtilities.getColor("_TextStyleOn"));
            btex3.Apply();
            _ButtonStyle.active.background = btex3;

            var tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, ColorUtilities.getColor("_TextStyleOn"));
            tex.Apply();
            _listStyle.hover.background = tex;
            _listStyle.onHover.background = tex;
            var tex2 = new Texture2D(1, 1);
            tex2.SetPixel(0, 0, ColorUtilities.getColor("_ButtonBG"));
            tex2.Apply();
            _listStyle.normal.background = tex2;
            _listStyle.onNormal.background = tex2;

            _ToggleBoxBG = ColorUtilities.getColor("_ToggleBoxBG");
        }

        public static bool MenuTab(string text, ref bool state, int fontsize = 29)
        {
            bool bPressed = false;
            bool lastValue = state;
            bool nextValue;
            int lastFontSize = _MenuTabStyle.fontSize;
            _MenuTabStyle.fontSize = fontsize;
            nextValue = GUILayout.Toggle(lastValue, text.ToUpper(), _MenuTabStyle);

            if (lastValue != nextValue) { state = !state; bPressed = true; };
            _MenuTabStyle.fontSize = lastFontSize;
            return bPressed;
        }

        public static bool MenuTabAbsolute(Vector2 pos, string text, ref bool state, int fontsize = 29)
        {
            bool bPressed = false;
            bool lastValue = state;
            bool nextValue;
            int lastFontSize = _MenuTabStyle.fontSize;
            _MenuTabStyle.fontSize = fontsize;
            Vector2 size = _MenuTabStyle.CalcSize(new GUIContent(text));
            Rect rect = new Rect(pos, size);
            nextValue = GUI.Toggle(rect, lastValue, text.ToUpper(), _MenuTabStyle);

            if (lastValue != nextValue) { state = !state; bPressed = true; };
            _MenuTabStyle.fontSize = lastFontSize;
            return bPressed;
        }

        public static void MenuArea(Rect area, string header, Action code)
        {
            Rect DarkOutline = new Rect(area.x, area.y + 5, area.width, area.height - 5);
            Rect LightOutline = MenuUtilities.Inline(DarkOutline);
            Rect Filler = MenuUtilities.Inline(LightOutline);
            Drawing.DrawRect(DarkOutline, MenuComponent._OutlineBorderBlack);
            Drawing.DrawRect(LightOutline, MenuComponent._OutlineBorderLightGray);
            Drawing.DrawRect(Filler, MenuComponent._FillLightBlack);
            Vector2 size = _HeaderStyle.CalcSize(new GUIContent(header));
            Drawing.DrawRect(new Rect(area.x + 18, area.y, size.x + 4, size.y), MenuComponent._FillLightBlack);
            GUI.Label(new Rect(area.x + 20, area.y - 3, size.x, size.y), header, _HeaderStyle);
            GUILayout.BeginArea(area);
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            try { code(); }
            catch (Exception e) { Debug.LogException(e); }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        public static void SectionTabButton(string text, Action code, float space = 0, int fontsize = 20)
        {
            bool temp = false;
            GUILayout.Space(space);
            if (MenuTab(text, ref temp, fontsize))
            {
                SectionTab.CurrentSectionTab = new SectionTab(text, code);
            }
            GUILayout.Space(space);
        }

        public static bool Toggle(string text, ref bool state, int fontsize = 17)
        {
            bool pressed = false;
            int bordersize = 1;
            int boxsize = 15;
            int lastfontsize = _TextStyle.fontSize;
            _TextStyle.fontSize = fontsize;
            GUILayout.Space(3);
            Rect area = GUILayoutUtility.GetRect(150, 15);
            Rect border = new Rect(area.x + bordersize, area.y + bordersize, boxsize - bordersize * 2, boxsize - bordersize * 2);
            Rect togglebox = MenuUtilities.Inline(border);
            Drawing.DrawRect(border, MenuComponent._OutlineBorderBlack);
            Drawing.DrawRect(togglebox, _ToggleBoxBG);
            if (GUI.Button(area, GUIContent.none, _TextStyle))
            {
                state = !state;
                pressed = true;
            }
            if (Event.current.type == EventType.Repaint)
            {
                bool hover = area.Contains(Event.current.mousePosition);
                bool active = hover && Input.GetMouseButton(0);
                _TextStyle.Draw(new Rect(area.x + 20, area.y, 130, boxsize), text, hover, active, false, false);
            }
            _TextStyle.fontSize = lastfontsize;
            if (state)
                Drawing.DrawRect(togglebox, MenuComponent._Accent2);
            return pressed;
        }

        public static void Slider(float left, float right, ref float value, int size)
        {
            GUIStyle thumb = _sliderThumbStyle;
            GUIStyle slider = _sliderStyle;
            float thumbSize = thumb.fixedWidth != 0 ? thumb.fixedWidth : thumb.padding.horizontal;
            float start = left;
            float end = right;
            value = GUILayout.HorizontalSlider(value, left, right, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, GUILayout.Width(size));
            Rect position = GUILayoutUtility.GetLastRect();
            float pixelsPerValue = (position.width - slider.padding.horizontal - thumbSize) / (end - start);
            Rect thumbRect = new Rect((value - start) * pixelsPerValue + position.x + slider.padding.left, position.y + slider.padding.top, thumbSize, position.height - slider.padding.vertical);
            Drawing.DrawRect(position, MenuComponent._FillLightBlack);
            Drawing.DrawRect(new Rect(position.x, position.y + position.height / 2 - 2, position.width, 4), _ToggleBoxBG);
            Drawing.DrawRect(thumbRect, MenuComponent._OutlineBorderBlack);
            Drawing.DrawRect(MenuUtilities.Inline(thumbRect), _MenuTabStyle.onNormal.textColor);
            GUILayout.Space(5);
        }

        public static float Slider(float left, float right, float value, int size)
        {
            GUIStyle thumb = _sliderThumbStyle;
            GUIStyle slider = _sliderStyle;
            float thumbSize = thumb.fixedWidth != 0 ? thumb.fixedWidth : thumb.padding.horizontal;
            float start = left;
            float end = right;
            value = GUILayout.HorizontalSlider(value, left, right, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, GUILayout.Width(size));
            Rect position = GUILayoutUtility.GetLastRect();
            float pixelsPerValue = (position.width - slider.padding.horizontal - thumbSize) / (end - start);
            Rect thumbRect = new Rect((value - start) * pixelsPerValue + position.x + slider.padding.left, position.y + slider.padding.top, thumbSize, position.height - slider.padding.vertical);
            Drawing.DrawRect(position, MenuComponent._FillLightBlack);
            Drawing.DrawRect(new Rect(position.x, position.y + position.height / 2 - 2, position.width, 4), _ToggleBoxBG);
            Drawing.DrawRect(thumbRect, MenuComponent._OutlineBorderBlack);
            Drawing.DrawRect(MenuUtilities.Inline(thumbRect), _MenuTabStyle.onNormal.textColor);
            GUILayout.Space(5);
            return value;
        }


        public static void VerticalSlider(Rect pos, float top, float bottom, ref float value)
        {
            GUIStyle thumb = _sliderVThumbStyle;
            GUIStyle slider = _sliderStyle;
            float thumbSize = thumb.fixedHeight != 0 ? thumb.fixedHeight : thumb.padding.vertical;
            float start = top;
            float end = bottom;
            value = GUI.VerticalSlider(pos, value, top, bottom, _sliderStyle, GUI.skin.verticalSliderThumb);
            Rect position = pos;
            float pixelsPerValue = (position.height - slider.padding.vertical - thumbSize) / (end - start);
            Rect thumbRect = new Rect(position.x + slider.padding.left, (value - start) * pixelsPerValue + position.y + slider.padding.top, position.width - slider.padding.horizontal, thumbSize);
            Drawing.DrawRect(position, MenuComponent._FillLightBlack);
            Drawing.DrawRect(new Rect(position.x + position.width / 2 - 2, position.y, 4, position.height), _ToggleBoxBG);
            Drawing.DrawRect(thumbRect, MenuComponent._OutlineBorderBlack);
            Drawing.DrawRect(MenuUtilities.Inline(thumbRect), _MenuTabStyle.onNormal.textColor);
        }

        public static void ScrollView(Rect area, string title, ref Vector2 scrollpos, Action code, int padding = 20, params GUILayoutOption[] options)
        {
            Drawing.DrawRect(area, MenuComponent._OutlineBorderBlack);
            Drawing.DrawRect(MenuUtilities.Inline(area), MenuComponent._OutlineBorderLightGray);
            Rect inlined = MenuUtilities.Inline(area, 2);
            Drawing.DrawRect(inlined, MenuComponent._FillLightBlack);
            Color lastColor = _MenuTabStyle.normal.textColor;
            int lastFontSize = _MenuTabStyle.fontSize;
            _MenuTabStyle.normal.textColor = _MenuTabStyle.onNormal.textColor;
            _MenuTabStyle.fontSize = 15;
            Drawing.DrawRect(new Rect(inlined.x, inlined.y, inlined.width, _MenuTabStyle.CalcSize(new GUIContent(title)).y + 2), MenuComponent._OutlineBorderLightGray);
            GUILayout.BeginArea(inlined);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(title, _MenuTabStyle);
                    _MenuTabStyle.normal.textColor = lastColor;
                    _MenuTabStyle.fontSize = lastFontSize;
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(2);
                Rect rects;
                Rect inner;
                GUILayout.BeginHorizontal();
                {
                    scrollpos = GUILayout.BeginScrollView(scrollpos, false, true);
                    {
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Space(padding);
                            GUILayout.BeginVertical(GUILayout.MinHeight(inlined.height));
                            {

                                try { code(); }
                                catch (Exception e) { Debug.LogException(e); }
                            }
                            GUILayout.EndVertical();
                            inner = GUILayoutUtility.GetLastRect();
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndScrollView();
                    rects = GUILayoutUtility.GetLastRect();
                    GUILayout.Space(1);
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(1);
                Drawing.DrawRect(new Rect(rects.x + rects.width - 16, rects.y, 16, rects.height), MenuComponent._FillLightBlack);
                if (inner.height - rects.height > 0)
                    VerticalSlider(new Rect(rects.x + 4, rects.y + 8, 12, rects.height - 14), 0, inner.height - rects.height, ref scrollpos.y);
            }
            GUILayout.EndArea();

        }

		public static void ScrollView(Rect area, string title, ref SerializableVector2 scrollpos, Action code, int padding = 20, params GUILayoutOption[] options)
		{
			Drawing.DrawRect(area, MenuComponent._OutlineBorderBlack);
			Drawing.DrawRect(MenuUtilities.Inline(area), MenuComponent._OutlineBorderLightGray);
			Rect inlined = MenuUtilities.Inline(area, 2);
			Drawing.DrawRect(inlined, MenuComponent._FillLightBlack);
			Color lastColor = _MenuTabStyle.normal.textColor;
			int lastFontSize = _MenuTabStyle.fontSize;
			_MenuTabStyle.normal.textColor = _MenuTabStyle.onNormal.textColor;
			_MenuTabStyle.fontSize = 15;
			Drawing.DrawRect(new Rect(inlined.x, inlined.y, inlined.width, _MenuTabStyle.CalcSize(new GUIContent(title)).y + 2), MenuComponent._OutlineBorderLightGray);
			GUILayout.BeginArea(inlined);
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label(title, _MenuTabStyle);
					_MenuTabStyle.normal.textColor = lastColor;
					_MenuTabStyle.fontSize = lastFontSize;
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(2);
				Rect rects;
				Rect inner;
				GUILayout.BeginHorizontal();
				{
					scrollpos = GUILayout.BeginScrollView(scrollpos.ToVector2(), false, true);
					{
						GUILayout.BeginHorizontal();
						{
							GUILayout.Space(padding);
							GUILayout.BeginVertical(GUILayout.MinHeight(inlined.height));
							{

								try { code(); }
								catch (Exception e) { Debug.LogException(e); }
							}
							GUILayout.EndVertical();
							inner = GUILayoutUtility.GetLastRect();
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndScrollView();
					rects = GUILayoutUtility.GetLastRect();
					GUILayout.Space(1);
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(1);
				Drawing.DrawRect(new Rect(rects.x + rects.width - 16, rects.y, 16, rects.height), MenuComponent._FillLightBlack);
				if (inner.height - rects.height > 0)
					VerticalSlider(new Rect(rects.x + 4, rects.y + 8, 12, rects.height - 14), 0, inner.height - rects.height, ref scrollpos.y);
			}
			GUILayout.EndArea();

		}

		public static bool List(float width, string identifier, GUIContent buttonContent, GUIContent[] listContent, params GUILayoutOption[] options)
        {
            Vector2 size = _listStyle.CalcSize(buttonContent);
            List<GUILayoutOption> parameters = options.ToList();
            parameters.Add(GUILayout.Height(size.y));
            parameters.Add(GUILayout.Width(width));
            Rect position = GUILayoutUtility.GetRect(width, size.y, parameters.ToArray());
            DropDown dd = DropDown.Get(identifier);
            return List(position, ref dd.IsEnabled, ref dd.ListIndex, buttonContent, listContent, "button", "box", _listStyle);
        }

        public static bool List(string identifier, GUIContent buttonContent, GUIContent[] listContent, params GUILayoutOption[] options)
        {
            Vector2 size = _listStyle.CalcSize(buttonContent);
            List<GUILayoutOption> parameters = options.ToList();
            parameters.Add(GUILayout.Height(size.y));
            parameters.Add(GUILayout.Width(size.x + 5));
            Rect position = GUILayoutUtility.GetRect(size.x + 5, size.y, parameters.ToArray());
            DropDown dd = DropDown.Get(identifier);
            return List(position, ref dd.IsEnabled, ref dd.ListIndex, buttonContent, listContent, "button", "box", _listStyle);
        }

        public static bool List(Rect position, ref bool showList, ref int listEntry, GUIContent buttonContent, GUIContent[] listContent)
        {
            return List(position, ref showList, ref listEntry, buttonContent, listContent, "button", "box", _listStyle);
        }

        public static bool List(Rect position, ref bool showList, ref int listEntry, GUIContent buttonContent, GUIContent[] listContent,
                                 GUIStyle buttonStyle, GUIStyle boxStyle, GUIStyle listStyle)
        {
            int controlID = GUIUtility.GetControlID(popupListHash, FocusType.Passive);
            bool done = false;
            switch (Event.current.GetTypeForControl(controlID))
            {
                case EventType.mouseDown:
                    if (position.Contains(Event.current.mousePosition))
                    {
                        GUIUtility.hotControl = controlID;
                        showList = true;
                    }
                    break;
                case EventType.mouseUp:
                    if (showList)
                    {
                        done = true;
                    }
                    break;
            }

            Drawing.DrawRect(position, MenuComponent._OutlineBorderBlack);
            Drawing.DrawRect(MenuUtilities.Inline(position), MenuComponent._OutlineBorderDarkGray);
            int lastFont = _TextStyle.fontSize;
            Color lastColor = _TextStyle.normal.textColor;
            _TextStyle.fontSize = 15;
            _TextStyle.normal.textColor = _TextStyle.onNormal.textColor;
            _TextStyle.alignment = TextAnchor.MiddleLeft;
            GUI.Label(new Rect(position.x + 4, position.y, position.width, position.height), buttonContent, _TextStyle);
            if (showList)
            {
                float width = 0;
                for (int i = 0; i < listContent.Length; i++)
                {
                    float temp = listStyle.CalcSize(listContent[i]).x;
                    if (temp > width) width = temp;
                }
                if (width < position.width) width = position.width;
                Rect listRect = new Rect(position.x, position.y - listStyle.CalcHeight(listContent[0], 1.0f) * listContent.Length + position.height, width, listStyle.CalcHeight(listContent[0], 1.0f) * listContent.Length);
                Drawing.DrawRect(listRect, MenuComponent._OutlineBorderBlack);
                listEntry = GUI.SelectionGrid(MenuUtilities.Inline(listRect), listEntry, listContent, 1, listStyle);
            }
            if (done)
            {
                showList = false;
            }

            _TextStyle.alignment = TextAnchor.UpperLeft;
            _TextStyle.fontSize = lastFont;
            _TextStyle.normal.textColor = lastColor;
            return done;
        }

        public static bool Button(string text, float width, float height = 25, params GUILayoutOption[] options)
        {
            List<GUILayoutOption> parameters = options.ToList();
            parameters.Add(GUILayout.Height(height));
            parameters.Add(GUILayout.Width(width));
            Rect area = GUILayoutUtility.GetRect(width, height, parameters.ToArray());
            Drawing.DrawRect(area, MenuComponent._OutlineBorderBlack);
            return GUI.Button(MenuUtilities.Inline(area), text, _ButtonStyle);
        }

        public static bool ColorButton(float width, ColorVariable color, float height = 25, params GUILayoutOption[] options)
        {
            List<GUILayoutOption> parameters = options.ToList();
            parameters.Add(GUILayout.Height(height));
            parameters.Add(GUILayout.Width(width));
            Rect area = GUILayoutUtility.GetRect(width, height, parameters.ToArray());
            Drawing.DrawRect(area, MenuComponent._OutlineBorderBlack);
            Rect preview = new Rect(area.x + 4, area.y + 4, area.height - 8, area.height - 8);
            bool pressed = GUI.Button(MenuUtilities.Inline(area), "      " + color.name, _ButtonStyle);
            Drawing.DrawRect(preview, MenuComponent._OutlineBorderBlack);
            Drawing.DrawRect(MenuUtilities.Inline(preview), MenuComponent._OutlineBorderLightGray);
            Drawing.DrawRect(MenuUtilities.Inline(preview, 2), color.color);
            return pressed;
        }

        public static string TextField(string text, string label, int width)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(label, _TextStyle);
                int lastFontSize = _TextStyle.fontSize;
                _TextStyle.fontSize = 13;
                float height = _TextStyle.CalcSize(new GUIContent("asdf")).y;
                Rect rect = GUILayoutUtility.GetRect(width, height);
                Drawing.DrawRect(new Rect(rect.x, rect.y + 2, rect.width, rect.height + 1), MenuComponent._OutlineBorderLightGray);
                Drawing.DrawRect(new Rect(rect.x, rect.y + 2, rect.width, rect.height), MenuComponent._FillLightBlack);
                text = GUI.TextField(new Rect(rect.x + 4, rect.y + 2, rect.width, rect.height), text, _TextStyle);
                GUILayout.FlexibleSpace();
                _TextStyle.fontSize = lastFontSize;
            }
            GUILayout.EndHorizontal();
            return text;
        }

        public static int TextField(int text, string label, int width, int min = 0, int max = 255)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(label, _TextStyle);
                int lastFontSize = _TextStyle.fontSize;
                _TextStyle.fontSize = 13;
                float height = _TextStyle.CalcSize(new GUIContent("asdf")).y;
                Rect rect = GUILayoutUtility.GetRect(width, height);
                Drawing.DrawRect(new Rect(rect.x, rect.y + 2, rect.width, rect.height + 1), MenuComponent._OutlineBorderLightGray);
                Drawing.DrawRect(new Rect(rect.x, rect.y + 2, rect.width, rect.height), MenuComponent._FillLightBlack);
                try
                {
                    int value = int.Parse(digitsOnly.Replace(GUI.TextField(new Rect(rect.x + 4, rect.y + 2, rect.width, rect.height), text.ToString(), _TextStyle), ""));
                    if (value >= min && value <= max)
                        text = value;
                }
                catch { }
                GUILayout.FlexibleSpace();
                _TextStyle.fontSize = lastFontSize;
            }
            GUILayout.EndHorizontal();
            return text;
        }
    }
}
