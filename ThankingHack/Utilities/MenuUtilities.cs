using UnityEngine;

namespace Thanking.Utilities
{
    public static class MenuUtilities
    {
        public static Texture2D TexClear;

        static MenuUtilities()
        {
            TexClear = new Texture2D(1, 1);
            TexClear.SetPixel(0, 0, new Color(0, 0, 0, 0));
            TexClear.Apply();
        }

        public static void FixGUIStyleColor(GUIStyle style)
        {
            style.normal.background = TexClear;
            style.onNormal.background = TexClear;
            style.hover.background = TexClear;
            style.onHover.background = TexClear;
            style.active.background = TexClear;
            style.onActive.background = TexClear;
            style.focused.background = TexClear;
            style.onFocused.background = TexClear;
        }

        public static Rect Inline(Rect rect, int border = 1)
        {
            Rect inlined = new Rect(rect.x + border, rect.y + border, rect.width - border * 2, rect.height - border * 2);
            return inlined;
        }

        public static Rect AbsRect(Vector2 pos1, Vector2 pos2) { return AbsRect(pos1.x, pos1.y, pos2.x, pos2.y); }
        public static Rect AbsRect(float x1, float y1, float x2, float y2)
        {
            float width = y2 - y1;
            float height = x2 - x1;
            return new Rect(x1, y1, width, height);
        }
    }
}
