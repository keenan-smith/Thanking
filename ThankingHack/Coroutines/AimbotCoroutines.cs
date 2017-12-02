using System.Collections;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Components.UI;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using UnityEngine;
using System;
using Thanking.Options;

namespace Thanking.Coroutines
{
    public static class AimbotCoroutines
    {
        public static GameObject LockedObject;
        public static bool IsAiming = false;

        public static float Pitch
        {
            get => Player.player.look.pitch;
            set => Player.player.look.SetField<float>("_pitch", ReflectionManager.FieldType.Private, value);
        }

        public static float Yaw
        {
            get => Player.player.look.yaw;
            set => Player.player.look.SetField<float>("_yaw", ReflectionManager.FieldType.Private, value);
        }

        public static IEnumerator SetLockedObject()
        {
            #if DEBUG
            DebugUtilities.Log("Starting Aimbot Lock Coroutine");
            #endif
            
            while (true)
            {
                if (!DrawUtilities.ShouldRun() || !AimbotOptions.Enabled)
                {
                    yield return new WaitForSeconds(.1f);
                    continue;
                }
                else
                {
                    Player p = null;
                    SteamPlayer[] players = Provider.clients.ToArray();
                    for (int i = 0; i < players.Length; i++)
                    {
                        SteamPlayer cPlayer = players[i];
                        if (cPlayer.player == null || cPlayer.player == Player.player || cPlayer.player.life == null ||
                            cPlayer.player.life.isDead || FriendUtilities.IsFriendly(cPlayer.player)) continue;

                        switch (AimbotOptions.TargetMode)
                        {
                            case TargetMode.Distance:
                            {
                                if (p == null)
                                    p = players[i].player;
                                else
                                {
                                    if (p != null)
                                        if (VectorUtilities.GetDistance(p.transform.position) >
                                            VectorUtilities.GetDistance(players[i].player.transform.position))
                                            p = players[i].player;
                                }
                                break;
                            }
                            case TargetMode.FOV:
                            {
                                Vector3 v2dist =
                                    MainCamera.instance.WorldToScreenPoint(GetAimPosition(players[i].player.transform,
                                        "Skull"));
                                if (v2dist.z <= 0) continue;

                                Vector2 pos = new Vector2(v2dist.x, v2dist.y);
                                float vdist = Vector2.Distance(new Vector2(Screen.width / 2, Screen.height / 2), pos);

                                if (vdist < AimbotOptions.FOV && p == null)
                                    p = players[i].player;

                                else if (vdist < AimbotOptions.FOV)
                                {
                                    Vector3 v2dist_ =
                                        MainCamera.instance.WorldToScreenPoint(GetAimPosition(p.transform, "Skull"));
                                    Vector2 pos_ = new Vector2(v2dist_.x, v2dist_.y);
                                    float vdist_ = Vector2.Distance(new Vector2(Screen.width / 2, Screen.height / 2),
                                        pos_);

                                    if (vdist_ > vdist)
                                        p = players[i].player;
                                }
                                break;
                            }
                        }
                    }
                    if (!IsAiming)
                        LockedObject = (p != null ? p.gameObject : null);
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        public static IEnumerator AimToObject()
        {
            #if DEBUG
            DebugUtilities.Log("Starting Aim Coroutine");
            #endif
            
            while (true)
            {
                if (!DrawUtilities.ShouldRun())
                {
                    yield return new WaitForSeconds(.1f);
                    continue;
                }
                
                if (LockedObject != null && LockedObject.transform != null && ESPComponent.MainCamera != null)
                {
                    if (Input.GetKey(HotkeyOptions.HotkeyDict["_AimbotKey"]))
                    {
                        IsAiming = true;
                        if (AimbotOptions.Smooth)
                            SmoothAim(LockedObject);
                        else
                            Aim(LockedObject);
                    }
                    else
                        IsAiming = false;
                }
                else
                    IsAiming = false;
                yield return new WaitForEndOfFrame();
            }
        }

        public static void Aim(GameObject obj)
        {
            Camera mainCam = MainCamera.instance;
            Vector3 skullPosition = GetAimPosition(obj.transform, "Skull");
            Player.player.transform.LookAt(skullPosition);
            Player.player.transform.eulerAngles = new Vector3(0f, Player.player.transform.rotation.eulerAngles.y, 0f);
            mainCam.transform.LookAt(skullPosition);
            float num4 = mainCam.transform.localRotation.eulerAngles.x;

            if (num4 <= 90f && num4 <= 270f)
                num4 = mainCam.transform.localRotation.eulerAngles.x + 90f;
            else if (num4 >= 270f && num4 <= 360f)
                num4 = mainCam.transform.localRotation.eulerAngles.x - 270f;

            Pitch = num4;
            Yaw = Player.player.transform.rotation.eulerAngles.y;
        }

        public static void SmoothAim(GameObject obj)
        {
            Camera mainCam = MainCamera.instance;
            Vector3 skullPosition = GetAimPosition(obj.transform, "Skull");
            Player.player.transform.rotation = Quaternion.Slerp(Player.player.transform.rotation, Quaternion.LookRotation( skullPosition - Player.player.transform.position ), Time.deltaTime * AimbotOptions.AimSpeed);
            Player.player.transform.eulerAngles = new Vector3(0f, Player.player.transform.rotation.eulerAngles.y, 0f);
            mainCam.transform.localRotation = Quaternion.Slerp(mainCam.transform.localRotation, Quaternion.LookRotation(skullPosition - mainCam.transform.position), Time.deltaTime * AimbotOptions.AimSpeed);
            float num4 = mainCam.transform.localRotation.eulerAngles.x;

            if (num4 <= 90f && num4 <= 270f)
                num4 = mainCam.transform.localRotation.eulerAngles.x + 90f;
            else if (num4 >= 270f && num4 <= 360f)
                num4 = mainCam.transform.localRotation.eulerAngles.x - 270f;

            Pitch = num4;
            Yaw = Player.player.transform.rotation.eulerAngles.y;
        }

        private static Vector2 CalcAngle(GameObject obj)
        {
            Vector3 W2SPos = ESPComponent.MainCamera.WorldToScreenPoint(GetAimPosition(obj.transform, "Skull"));
            Vector2 angles = Vector2.zero;
            // lol idk how 2 trig pls help
            return angles;
        }

        public static void AimMouseTo(float x, float y)
        {
            //if (AimbotOptions.Smooth)
            //{
            //    x = Mathf.Lerp(Pitch, x, Time.deltaTime * (AimbotOptions.MaxSpeed - (AimbotOptions.AimSpeed + 1)));
            //    y = Mathf.Lerp(Yaw, y, Time.deltaTime * (AimbotOptions.MaxSpeed - (AimbotOptions.AimSpeed + 1)));
            //}
            
            #if DEBUG
            DebugUtilities.Log($"yaw:{Yaw}|pitch:{Pitch}|x:{x}|y:{y}");
            #endif        
    
            Yaw = x; // left right
            Pitch = y; // up down
        }

        public static Vector3 GetAimPosition(Transform parent, string name)
        {
            Transform[] componentsInChildren = parent.GetComponentsInChildren<Transform>();
            
            if (componentsInChildren == null) return new Vector3(1000, 1000, 1000);
            
            Transform[] array = componentsInChildren;
            
            for (int i = 0; i < array.Length; i++)
            {
                Transform tr = array[i];
                if (tr.name.Trim() == name)
                    return tr.position + new Vector3(0f, 0.4f, 0f);
            }
            
            return new Vector3(1000, 1000, 1000);
        }
    }
}

// scrapped code
/*[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);
private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
private const uint MOUSEEVENTF_LEFTUP = 0x04;
private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
private const uint MOUSEEVENTF_RIGHTUP = 0x10;

public static void AimMouseTo(float x, float y)
{
    float ScreenCenterX = (Screen.width / 2);
    float ScreenCenterY = (Screen.height / 2);
    float TargetX = 0;
    float TargetY = 0;

    float AimSpeed = ((AimbotOptions.MaxSpeed - AimbotOptions.AimSpeed) + 1) * 1000;

    if (x != 0)
    {
        if (x > ScreenCenterX)
        {
            TargetX = -(ScreenCenterX - x);
            TargetX /= AimSpeed;
            if (TargetX + ScreenCenterX > ScreenCenterX * 2) TargetX = 0;
        }

        if (x < ScreenCenterX)
        {
            TargetX = x - ScreenCenterX;
            TargetX /= AimSpeed;
            if (TargetX + ScreenCenterX < 0) TargetX = 0;
        }
    }
    if (y != 0)
    {
        if (y > ScreenCenterY)
        {
            TargetY = -(ScreenCenterY - y);
            TargetY /= AimSpeed;
            if (TargetY + ScreenCenterY > ScreenCenterY * 2) TargetY = 0;
        }

        if (y < ScreenCenterY)
        {
            TargetY = y - ScreenCenterY;
            TargetY /= AimSpeed;
            if (TargetY + ScreenCenterY < 0) TargetY = 0;
        }
    }
    if (!AimbotOptions.Smooth)
    {
        mouse_event(0x0001, (uint)(TargetX), (uint)(TargetY), 0, UIntPtr.Zero);
        return;
    }
    TargetX /= 10;
    TargetY /= 10;
    if (Math.Abs(TargetX) < 1)
    {
        if (TargetX > 0)
            TargetX = 1;
        if (TargetX < 0)
            TargetX = -1;
    }
    if (Math.Abs(TargetY) < 1)
    {
        if (TargetY > 0)
            TargetY = 1;
        if (TargetY < 0)
            TargetY = -1;
    }
    mouse_event(0x0001, (uint)TargetX, (uint)TargetY, 0, UIntPtr.Zero);
}*/
