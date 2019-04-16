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
using Thanking.Options;
using Thanking.Options.VisualOptions;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.Basic
{
    [Component]
    public class VanishPlayerComponent : MonoBehaviour
    {
        static bool WasEnabled;
        public static Rect veww;

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
            if (ESPOptions.ShowVanishPlayers && Provider.isConnected && !Provider.isLoading)
            {
                GUI.color = new Color(1f, 1f, 1f, 0f);
                veww = GUILayout.Window(350, MiscOptions.vew, PlayersMenu, "Vanish Players");
                MiscOptions.vew.x = veww.x;
                MiscOptions.vew.y = veww.y;
                GUI.color = Color.white;
            }
        }

        void PlayersMenu(int windowID)
        {
            Drawing.DrawRect(new Rect(0, 0, MiscOptions.vew.width, 20), new Color32(44, 44, 44, 255));
            Drawing.DrawRect(new Rect(0, 20, MiscOptions.vew.width, 5), new Color32(34, 34, 34, 255));
            Drawing.DrawRect(new Rect(0, 25, MiscOptions.vew.width, MiscOptions.vew.height + 25), new Color32(64, 64, 64, 255)); //bg
            GUILayout.Space(-19);
            GUILayout.Label("Vanish Players");

            int height = 55;
            foreach (SteamPlayer player in Provider.clients)
            {
                if (Vector3.Distance(player.player.transform.position, Vector3.zero) < 10)
                {
                    GUILayout.Label(player.playerID.characterName);
                    height += 12;
                }
            }
            MiscOptions.vew.height = height;
            GUI.DragWindow();
        }
    }
}
