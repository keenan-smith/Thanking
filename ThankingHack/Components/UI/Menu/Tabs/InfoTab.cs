using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public class InfoTab
    {
        public static void Tab()
        {
            Prefab.MenuArea(new Rect(0, 0, 466, 436), "INFO", () =>
            {
                GUILayout.Label("Contributors: Ceenan Smitt, ic3w0lf, Carl Roebuck, Kr4ken");
                GUILayout.Space(2);
                
                GUILayout.Label("Submit suggestions via https://goo.gl/TdncYj");
            });
        }
    }
}