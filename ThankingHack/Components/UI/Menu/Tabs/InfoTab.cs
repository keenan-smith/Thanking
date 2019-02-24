using SDG.Unturned;
using UnityEngine;

namespace Thinking.Components.UI.Menu.Tabs
{
    public class InfoTab
    {
        public static void Tab()
        {
            Prefab.MenuArea(new Rect(0, 0, 466, 436), "INFO", () =>
            {
				if (Provider.isConnected)
				{
					GUILayout.Label("Current Server Info: ", Prefab._TextStyle);
					GUILayout.Space(2);

					GUILayout.TextField($"{Parser.getIPFromUInt32(Provider.currentServerInfo.ip)}:{Provider.currentServerInfo.port}", Prefab._TextStyle);
					GUILayout.Space(4);
					
					GUILayout.Label("Current Server SteamID: ", Prefab._TextStyle);
					GUILayout.Space(2);
					GUILayout.TextField($"{Provider.server}", Prefab._TextStyle);
					GUILayout.Space(8);
				}

				GUILayout.Label("Contributors: zoomy500, ic3w0lf, DefCon42, Kr4ken, Coopyy :]", Prefab._TextStyle);
                GUILayout.Space(2);
                
                if (Prefab.Button("Submit a Suggestion", 200))
	                Application.OpenURL("https://goo.gl/forms/3JawxatKpTfPGXS73");
	            
	            GUILayout.Space(2);
	            
	            GUILayout.Label("Submit bug reports by E-Mailing them to this address:", Prefab._TextStyle);
	            GUILayout.TextField("incoming+DualExploits/Thanking@incoming.gitlab.com", Prefab._TextStyle);
	            
	            GUILayout.Space(2);
                
	            if (Prefab.Button("Website", 200))
		            Application.OpenURL("http://ironic.services");
            });
        }
    }
}