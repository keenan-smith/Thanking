using SDG.Unturned;
using Thanking.Attributes;
using UnityEngine;

namespace Thanking.Components.UI.Menu
{
    [SpyComponent]
    [Component]
    public class CursorComponent : MonoBehaviour
    {
        private Rect _cursor = new Rect(0, 0, 20f, 20f);
        private Texture _cursorTexture;
        
        public void OnGUI()
        {
            if (MenuComponent.IsInMenu)
            {
                GUI.depth = -2;
            
                if (_cursorTexture == null)
                    _cursorTexture = Resources.Load("UI/Cursor") as Texture;

                PlayerUI.window.showCursor = true;
                _cursor.x = Input.mousePosition.x;
                _cursor.y = Screen.height - Input.mousePosition.y;
                GUI.DrawTexture(_cursor, _cursorTexture);
            }
        }
    }
}