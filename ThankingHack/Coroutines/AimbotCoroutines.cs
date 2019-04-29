using System;
using System.Collections;
using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Components.UI;
using Thanking.Misc.Enums;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Coroutines
{
    public static class AimbotCoroutines
    {
        public static Vector3 PiVector = new Vector3(0, Mathf.PI, 0);
        
        public static GameObject LockedObject;
        public static bool IsAiming = false;

        public static FieldInfo PitchInfo;
        public static FieldInfo YawInfo;

        public static float Pitch
        {
            get => OptimizationVariables.MainPlayer.look.pitch;
            set => PitchInfo.SetValue(OptimizationVariables.MainPlayer.look, value);
        }

        public static float Yaw
        {
            get => OptimizationVariables.MainPlayer.look.yaw;
            set => YawInfo.SetValue(OptimizationVariables.MainPlayer.look, value);
        }

        [Initializer]
        public static void Init()
        {
            PitchInfo = typeof(PlayerLook).GetField("_pitch", BindingFlags.NonPublic | BindingFlags.Instance);
            YawInfo = typeof(PlayerLook).GetField("_yaw", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static IEnumerator SetLockedObject()
        {
            #if DEBUG
            DebugUtilities.Log("Starting Aimbot Lock Coroutine");
            #endif
            
            while (true)
            {
                if (!DrawUtilities.ShouldRun() || !AimbotOptions.Enabled || OptimizationVariables.MainPlayer?.look?.aim == null)
                {
                    yield return new WaitForSeconds(.1f);
                    continue;
                }
                
                Player newTarget = null;
                
                Vector3 aimPos = OptimizationVariables.MainPlayer.look.aim.position;
                Vector3 aimForward = OptimizationVariables.MainPlayer.look.aim.forward;

                var players = Provider.clients;

                for (int i = 0; i < Provider.clients.Count; i++)
                {
                    SteamPlayer cPlayer = players[i];

                    if (cPlayer?.player?.transform == null)
                        continue;

                    var enemyPosition = GetAimPosition(cPlayer.player.transform, "Skull");

                    if (cPlayer.player == OptimizationVariables.MainPlayer || cPlayer.player.life == null ||
                        cPlayer.player.life.isDead || FriendUtilities.IsFriendly(cPlayer.player) || VectorUtilities.GetDistance(enemyPosition) > AimbotOptions.Distance)
                        continue;

                    if (AimbotOptions.UseGunDistance && OptimizationVariables.MainPlayer?.equipment?.asset is ItemGunAsset gun && VectorUtilities.GetDistance(aimPos) > gun.range)
                        continue;

                    if (!AimbotOptions.AimThroughWalls && Physics.Linecast(aimPos, enemyPosition, RayMasks.DAMAGE_SERVER))
                        continue;

                    switch (AimbotOptions.TargetMode)
                    {
                        case TargetMode.Distance:
                        {
                            if (newTarget == null)
                                newTarget = players[i].player;
                            else if (VectorUtilities.GetDistance(newTarget.transform.position) > VectorUtilities.GetDistance(players[i].player.transform.position))
                                newTarget = players[i].player;
                            break;
                        }
                        case TargetMode.FOV:
                        {
                            if (VectorUtilities.GetAngleDelta(aimPos, aimForward, players[i].player.transform.position) < AimbotOptions.FOV)
                            {
                                if (newTarget == null)
                                {
                                    newTarget = players[i].player;
                                }
                                else if (!AimbotOptions.ClosestInFOV &&
                                         VectorUtilities.GetAngleDelta(aimPos, aimForward, players[i].player.transform.position) < VectorUtilities.GetAngleDelta(aimPos, aimForward, newTarget.transform.position))
                                {
                                    newTarget = players[i].player;
                                }
                                else if (AimbotOptions.ClosestInFOV &&
                                         VectorUtilities.GetDistance(newTarget.transform.position) > VectorUtilities.GetDistance(players[i].player.transform.position))
                                {
                                    newTarget = players[i].player;
                                }
                            }
                            break;
                        }
                    }
                }

                if (!IsAiming && !(AimbotOptions.UseReleaseAimKey && HotkeyUtilities.IsHotkeyHeld("_AimbotReleaseKey")))
                    LockedObject = newTarget?.gameObject;
                
                yield return new WaitForEndOfFrame();
            }
        }

        public static IEnumerator AimToObject()
        {
            #if DEBUG
            DebugUtilities.Log("Starting Aim Coroutine");
            #endif
            
            while (true)
            {
                if (!DrawUtilities.ShouldRun() || !AimbotOptions.Enabled)
                {
                    yield return new WaitForSeconds(.1f);
                    continue;
                }

                if (LockedObject != null &&
                    !AimbotOptions.AimThroughWalls &&
                    Physics.Linecast(OptimizationVariables.MainPlayer.look.aim.position, GetAimPosition(LockedObject?.transform, "Skull"), RayMasks.DAMAGE_SERVER))
                    LockedObject = null;
                
                if (LockedObject?.transform != null && ESPComponent.MainCamera != null)
                {
                    if (HotkeyUtilities.IsHotkeyHeld("_AimbotKey") || !AimbotOptions.OnKey)
                    {
                        if (!AimbotOptions.OnKey && AimbotOptions.UseReleaseAimKey && HotkeyUtilities.IsHotkeyHeld("_AimbotReleaseKey"))
                        {
                            IsAiming = false;
                            LockedObject = null;
                        }
                        else
                        {
                            IsAiming = true;
                            if (AimbotOptions.Smooth)
                                SmoothAim(LockedObject);
                            else
                                Aim(LockedObject);
                        }
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
            Camera mainCam = OptimizationVariables.MainCam;
            Vector3 skullPosition = GetAimPosition(obj.transform, "Skull");
            
            if (skullPosition == PiVector)
                return;
            
            OptimizationVariables.MainPlayer.transform.LookAt(skullPosition);
            OptimizationVariables.MainPlayer.transform.eulerAngles = new Vector3(0f, OptimizationVariables.MainPlayer.transform.rotation.eulerAngles.y, 0f);
            mainCam.transform.LookAt(skullPosition);
            float num4 = mainCam.transform.localRotation.eulerAngles.x;

            if (num4 <= 90f && num4 <= 270f)
                num4 = mainCam.transform.localRotation.eulerAngles.x + 90f;
            else if (num4 >= 270f && num4 <= 360f)
                num4 = mainCam.transform.localRotation.eulerAngles.x - 270f;

            Pitch = num4;
            Yaw = OptimizationVariables.MainPlayer.transform.rotation.eulerAngles.y;
        }

        public static void SmoothAim(GameObject obj)
        {
            Camera mainCam = OptimizationVariables.MainCam;
            Vector3 skullPosition = GetAimPosition(obj.transform, "Skull");
            
            if (skullPosition == PiVector)
                return;
            
            OptimizationVariables.MainPlayer.transform.rotation = Quaternion.Slerp(OptimizationVariables.MainPlayer.transform.rotation, Quaternion.LookRotation( skullPosition - OptimizationVariables.MainPlayer.transform.position ), Time.deltaTime * AimbotOptions.AimSpeed);
            OptimizationVariables.MainPlayer.transform.eulerAngles = new Vector3(0f, OptimizationVariables.MainPlayer.transform.rotation.eulerAngles.y, 0f);
            mainCam.transform.localRotation = Quaternion.Slerp(mainCam.transform.localRotation, Quaternion.LookRotation(skullPosition - mainCam.transform.position), Time.deltaTime * AimbotOptions.AimSpeed);
            float num4 = mainCam.transform.localRotation.eulerAngles.x;

            if (num4 <= 90f && num4 <= 270f)
                num4 = mainCam.transform.localRotation.eulerAngles.x + 90f;
            else if (num4 >= 270f && num4 <= 360f)
                num4 = mainCam.transform.localRotation.eulerAngles.x - 270f;

            Pitch = num4;
            Yaw = OptimizationVariables.MainPlayer.transform.rotation.eulerAngles.y;
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
            if (parent == null)
                return PiVector;

            Transform[] componentsInChildren = parent.GetComponentsInChildren<Transform>();
            
            if (componentsInChildren == null) 
                return PiVector;
            
            Transform[] array = componentsInChildren;
            
            for (int i = 0; i < array.Length; i++)
            {
                Transform tr = array[i];
                if (tr.name.Trim() == name)
                    return tr.position + new Vector3(0f, 0.4f, 0f);
            }

            return PiVector;
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
