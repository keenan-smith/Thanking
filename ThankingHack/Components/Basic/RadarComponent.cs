using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Components.UI.Menu;
using Thanking.Options.VisualOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Components.Basic
{
    [Component]
    public class RadarComponent : MonoBehaviour
    {
        public static Rect veww;
        public static Rect vew = new Rect(Screen.width - RadarOptions.RadarSize - 20, 10, RadarOptions.RadarSize + 10, RadarOptions.RadarSize + 10); //Viewport of the mirror camera
        public static Vector2 radarcenter;

        public static bool WasEnabled;

        [OnSpy]
        public static void Disable()
        {
            WasEnabled = RadarOptions.Enabled;
            RadarOptions.Enabled = false;
        }

        [OffSpy]
        public static void Enable() =>
            RadarOptions.Enabled = WasEnabled;

        void OnGUI()
        {
            if (RadarOptions.Enabled && Provider.isConnected && !Provider.isLoading)
            {
                vew.width = vew.height = RadarOptions.RadarSize + 10;

                GUI.color = new Color(1f, 1f, 1f, 0f);
                veww = GUILayout.Window(345, vew, RadarMenu, "Radar");
                vew.x = veww.x;
                vew.y = veww.y;
                GUI.color = Color.white;
            }
        }

        void RadarMenu(int windowID)
        {
            Drawing.DrawRect(new Rect(0, 0, vew.width, 20), new Color32(44, 44, 44, 255));
            Drawing.DrawRect(new Rect(0, 20, vew.width, 5), new Color32(34, 34, 34, 255));
            Drawing.DrawRect(new Rect(0, 25, vew.width, vew.height + 25), new Color32(64, 64, 64, 255)); //bg
            GUILayout.Space(-19);
            GUILayout.Label("Radar");
            Vector2 realradarcenter = new Vector2((vew.width) / 2, (vew.height + 25) / 2);
            radarcenter = new Vector2((vew.width) / 2, (vew.height + 25) / 2);
            Vector2 localpos = GameToRadarPosition(Player.player.transform.position);
            if (RadarOptions.TrackPlayer)
            {
                radarcenter.x -= localpos.x;
                radarcenter.y += localpos.y;
            }

            Drawing.DrawRect(new Rect(realradarcenter.x, 25, 1, vew.height), Color.gray);
            Drawing.DrawRect(new Rect(0, realradarcenter.y, vew.width, 1), Color.gray);
            #region localplayer
            DrawRadarDot(new Vector2(radarcenter.x + localpos.x, radarcenter.y - localpos.y), Color.black, 4);
            DrawRadarDot(new Vector2(radarcenter.x + localpos.x, radarcenter.y - localpos.y), Color.white, 3);

            #endregion
            #region Vehicles
            if (RadarOptions.ShowVehicles)
            {
                foreach (InteractableVehicle vehicle in VehicleManager.vehicles)
                {
                    if (RadarOptions.ShowVehiclesUnlocked)
                    {
                        if (!vehicle.isLocked)
                        {
                            Vector2 radarpos = GameToRadarPosition(vehicle.transform.position);
                            DrawRadarDot(new Vector2(radarcenter.x + radarpos.x, radarcenter.y - radarpos.y), Color.black, 3);
                            DrawRadarDot(new Vector2(radarcenter.x + radarpos.x, radarcenter.y - radarpos.y), ColorUtilities.getColor($"_Vehicles"), 2);
                        }
                    }
                    else
                    {
                        Vector2 radarpos = GameToRadarPosition(vehicle.transform.position);
                        DrawRadarDot(new Vector2(radarcenter.x + radarpos.x, radarcenter.y - radarpos.y), Color.black, 3);
                        DrawRadarDot(new Vector2(radarcenter.x + radarpos.x, radarcenter.y - radarpos.y), ColorUtilities.getColor($"_Vehicles"), 2);
                    }
                }
            }
            #endregion
            #region players
            if (RadarOptions.ShowPlayers)
            {
                foreach (SteamPlayer player in Provider.clients)
                {
                    if (player.player != OptimizationVariables.MainPlayer)
                    {
                        Vector2 radarpos1 = GameToRadarPosition(player.player.transform.position);
                        DrawRadarDot(new Vector2(radarcenter.x + radarpos1.x, radarcenter.y - radarpos1.y), Color.black, 3);
                        DrawRadarDot(new Vector2(radarcenter.x + radarpos1.x, radarcenter.y - radarpos1.y), ColorUtilities.getColor($"_Players"), 2);
                    }
                }
            }
            #endregion
            #region lastdeath
            if (MiscComponent.LastDeath != new Vector3(0, 0, 0))
            {
                Vector2 radarpos = GameToRadarPosition(MiscComponent.LastDeath);
                DrawRadarDot(new Vector2(radarcenter.x + radarpos.x, radarcenter.y - radarpos.y), Color.black, 4);
                DrawRadarDot(new Vector2(radarcenter.x + radarpos.x, radarcenter.y - radarpos.y), Color.grey, 3);
            }
            #endregion
            GUI.DragWindow();
        }

        void DrawRadarDot(Vector2 pos, Color color, float size = 2)
        {
            Drawing.DrawRect(new Rect(pos.x - size, pos.y - size, size * 2, size * 2), color);
        }

        public static Vector2 GameToRadarPosition(Vector3 pos)
        {
            Vector2 endpos;
            endpos.x = pos.x / (Level.size / (RadarOptions.RadarZoom * RadarOptions.RadarSize));
            endpos.y = pos.z / (Level.size / (RadarOptions.RadarZoom * RadarOptions.RadarSize));
            return endpos;
        }
    }
}
