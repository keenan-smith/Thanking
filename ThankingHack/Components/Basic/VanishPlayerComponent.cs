using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thanking.Attributes;
using Thanking.Components.UI;
using Thanking.Components.UI.Menu;
using Thanking.Misc.Enums;
using Thanking.Options.VisualOptions;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.Basic
{
    [Component]
    public class VanishPlayerComponent : MonoBehaviour
    {
        static bool WasEnabled;
        public static Rect vew = new Rect(1075, 10, 200, 300);

        [OnSpy]
        public static void Disable()
        {
            WasEnabled = ESPOptions.ShowVanishPlayers;
            ESPOptions.ShowVanishPlayers = false;
        }

        [OffSpy]
        public static void Enable() =>
            ESPOptions.ShowVanishPlayers = WasEnabled;

        void OnGUI()
        {
            if (ESPOptions.ShowVanishPlayers)
            {
                GUI.color = new Color(1f, 1f, 1f, 0f);
                vew = GUILayout.Window(350, vew, PlayersMenu, "Vanish Players");
                GUI.color = Color.white;
            }
        }

        void PlayersMenu(int windowID)
        {
            Drawing.DrawRect(new Rect(0, 0, vew.width, 20), new Color32(44, 44, 44, 255));
            Drawing.DrawRect(new Rect(0, 20, vew.width, 5), new Color32(34, 34, 34, 255));
            Drawing.DrawRect(new Rect(0, 25, vew.width, vew.height + 25), new Color32(64, 64, 64, 255)); //bg
            GUILayout.Space(-19);
            GUILayout.Label("Vanish Players");

            foreach (SteamPlayer player in Provider.clients)
            {
                if (Vector3.Distance(player.player.transform.position, Vector3.zero) < 10)
                    GUILayout.Label(player.playerID.characterName);
            }
            GUI.DragWindow();
        }
    }
}
