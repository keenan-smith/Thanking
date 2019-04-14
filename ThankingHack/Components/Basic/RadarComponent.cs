using SDG.Unturned;
using System;
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
            if (RadarOptions.type == 2 || RadarOptions.type == 3)
            {
                radarcenter.x -= localpos.x;
                radarcenter.y += localpos.y;
            }

            Drawing.DrawRect(new Rect(realradarcenter.x, 25, 1, vew.height), Color.gray);
            Drawing.DrawRect(new Rect(0, realradarcenter.y, vew.width, 1), Color.gray);

            #region localplayer
            Vector2 top = new Vector2(realradarcenter.x, realradarcenter.y - 10);
            Vector2 left = new Vector2(realradarcenter.x + 5, realradarcenter.y + 5);
            Vector2 right = new Vector2(realradarcenter.x - 5, realradarcenter.y + 5);

            if (RadarOptions.type == 2)
            {
                top = RotatePoint(top, realradarcenter, Math.Round(MainCamera.instance.transform.eulerAngles.y, 2));
                left = RotatePoint(left, realradarcenter, Math.Round(MainCamera.instance.transform.eulerAngles.y, 2));
                right = RotatePoint(right, realradarcenter, Math.Round(MainCamera.instance.transform.eulerAngles.y, 2));
                DrawLine(top, left, Color.white, 1);
                DrawLine(left, right, Color.white, 1);
                DrawLine(right, top, Color.white, 1);
            }

            if (RadarOptions.type == 3)
            {
                DrawLine(top, left, Color.white, 1);
                DrawLine(left, right, Color.white, 1);
                DrawLine(right, top, Color.white, 1);
            }

            if (RadarOptions.type == 1)
            {

                Vector2 pos = new Vector2(radarcenter.x + localpos.x, radarcenter.y - localpos.y);
                Vector2 t = new Vector2(pos.x, pos.y - 10);
                Vector2 l = new Vector2(pos.x + 5, pos.y + 5);
                Vector2 r = new Vector2(pos.x - 5, pos.y + 5);
                t = RotatePoint(t, pos, Math.Round(MainCamera.instance.transform.eulerAngles.y, 2));
                l = RotatePoint(l, pos, Math.Round(MainCamera.instance.transform.eulerAngles.y, 2));
                r = RotatePoint(r, pos, Math.Round(MainCamera.instance.transform.eulerAngles.y, 2));
                DrawLine(t, l, Color.white, 1);
                DrawLine(l, r, Color.white, 1);
                DrawLine(r, t, Color.white, 1);
            }



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
                        Vector2 rpos = new Vector2(radarcenter.x + radarpos1.x, radarcenter.y - radarpos1.y);
                        if (RadarOptions.DetialedPlayers)
                        {
                            if (rpos.y > 30)
                            {
                                Vector2 t = new Vector2(rpos.x, rpos.y - 10);
                                Vector2 l = new Vector2(rpos.x + 5, rpos.y + 5);
                                Vector2 r = new Vector2(rpos.x - 5, rpos.y + 5);
                                t = RotatePoint(t, rpos, Math.Round(player.player.look.aim.eulerAngles.y, 2));
                                l = RotatePoint(l, rpos, Math.Round(player.player.look.aim.eulerAngles.y, 2));
                                r = RotatePoint(r, rpos, Math.Round(player.player.look.aim.eulerAngles.y, 2));
                                DrawLine(t, l, ColorUtilities.getColor($"_Players"), 1);
                                DrawLine(l, r, ColorUtilities.getColor($"_Players"), 1);
                                DrawLine(r, t, ColorUtilities.getColor($"_Players"), 1);
                            }
                        }
                        else
                        {

                            DrawRadarDot(rpos, Color.black, 3);
                            DrawRadarDot(rpos, ColorUtilities.getColor($"_Players"), 2);
                        }
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
            if (pos.y > 28)
                Drawing.DrawRect(new Rect(pos.x - size, pos.y - size, size * 2, size * 2), color);
        }

        public Vector2 GameToRadarPosition(Vector3 pos)
        {
            Vector2 endpos;
            endpos.x = pos.x / (Level.size / (RadarOptions.RadarZoom * RadarOptions.RadarSize));
            endpos.y = pos.z / (Level.size / (RadarOptions.RadarZoom * RadarOptions.RadarSize));

            if (RadarOptions.type == 3)
            {
                Vector2 newpoints = RotatePoint(endpos, new Vector2((vew.width) / 2, (vew.height + 25) / 2), Math.Round(MainCamera.instance.transform.eulerAngles.y, 2));
                return newpoints;
            }

            return endpos;
        }

        static Texture2D lineTex;
        public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
        {
            var matrix = GUI.matrix;
            if (!lineTex)
            {
                lineTex = new Texture2D(1, 1);
                lineTex.SetPixel(0, 0, Color.white);
                lineTex.Apply();
            }
            var savedColor = GUI.color;
            GUI.color = color;
            var angle = Vector2.Angle(pointB - pointA, Vector2.right);
            if (pointA.y > pointB.y) { angle = -angle; }
            GUIUtility.RotateAroundPivot(angle, pointA);
            GUI.DrawTexture(new Rect(pointA.x, pointA.y, (pointB - pointA).magnitude, width), lineTex);
            GUI.matrix = matrix;
            GUI.color = savedColor;
        }
        public static Vector2 RotatePoint(Vector2 pointToRotate, Vector2 centerPoint, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new Vector2((int)(cosTheta * (pointToRotate.x - centerPoint.x) - sinTheta * (pointToRotate.y - centerPoint.y) + centerPoint.x), (int)(sinTheta * (pointToRotate.x - centerPoint.x) + cosTheta * (pointToRotate.y - centerPoint.y) + centerPoint.y));
        }
    }
}
