using SDG.Unturned;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public class InfoTab
    {
        public static void Tab()
        {
            Prefab.MenuArea(new Rect(0, 0, 466, 436), "INFO", () =>
            {
				if (Provider.isConnected)
				{
					GUILayout.Label("Current Server Info (IP/Port): ", Prefab._TextStyle);
					GUILayout.Space(2);

					GUILayout.TextField($"{Parser.getIPFromUInt32(Provider.currentServerInfo.ip)}", Prefab._TextStyle);
					GUILayout.TextField($"{Provider.currentServerInfo.port}", Prefab._TextStyle);
					GUILayout.Space(8);
				}

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