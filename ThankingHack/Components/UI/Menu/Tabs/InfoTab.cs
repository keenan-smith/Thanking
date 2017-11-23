using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public class InfoTab
    {
        public static void Tab()
        {
            Prefab.MenuArea(new Rect(0, 0, 466, 436), "INFO", () =>
            {
                GUILayout.Label("Contributors: zoomy500, ic3w0lf, defcon42, Kr4ken", Prefab._TextStyle);
                GUILayout.Space(2);
                
                if (Prefab.Button("Submit a Suggestion", 200))
                {
                    Application.OpenURL("https://goo.gl/TdncYj");
                }
            });
        }
    }
}