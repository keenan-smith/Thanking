﻿﻿using System.Collections;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Components.UI;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using UnityEngine;
using System;
using System.Reflection;
using Thanking.Options;
using Thanking.Variables;
 
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
                if (!DrawUtilities.ShouldRun() || !AimbotOptions.Enabled)
                {
                    yield return new WaitForSeconds(.1f);
                    continue;
                }
                
                Player p = null;
                SteamPlayer[] players = Provider.clients.ToArray();
                for (int i = 0; i < players.Length; i++)
                {
                    SteamPlayer cPlayer = players[i];
                    if (cPlayer == null || cPlayer.player == OptimizationVariables.MainPlayer || cPlayer.player.life == null ||
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
3 file changes in working directory
View changes
commit:8c6225
finish up multithreading