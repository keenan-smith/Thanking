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
					GUILayout.Label("Server Info");
					GUILayout.Space(2);

					GUILayout.Label($"IP: {Parser.getIPFromUInt32(Provider.ip)}");
					GUILayout.Label($"Port: {Provider.port}");

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