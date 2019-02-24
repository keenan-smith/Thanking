﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SDG.Unturned;
using Thinking.Attributes;
using Thinking.Components.UI;
using Thinking.Options;
using Thinking.Utilities;
using Thinking.Coroutines;
using Thinking.Options.AimOptions;
using Thinking.Options.VisualOptions;
using Thinking.Overrides;
using Thinking.Threads;
using Thinking.Variables;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Thinking.Components.Basic
{
    [Component]
    public class MiscComponent : MonoBehaviour
    {
        public static MiscComponent Instance;
        public static float LastMovementCheck;
        public static bool FreecamBeforeSpy;
        public static bool NightvisionBeforeSpy;
        public static List<PlayerInputPacket> ClientsidePackets;

        public static FieldInfo Primary =
            typeof(PlayerEquipment).GetField("_primary", BindingFlags.NonPublic | BindingFlags.Instance);
        
        public static FieldInfo Sequence =
            typeof(PlayerInput).GetField("sequence", BindingFlags.NonPublic | BindingFlags.Instance);
        
        public static FieldInfo CPField =
            typeof(PlayerInput).GetField("clientsidePackets", BindingFlags.NonPublic | BindingFlags.Instance);

        private int currentKills = -1;

        [Initializer]
        public static void Initialize()
        {
            HotkeyComponent.ActionDict.Add("_VFToggle", () => 
                MiscOptions.VehicleFly = !MiscOptions.VehicleFly);
            
            HotkeyComponent.ActionDict.Add("_ToggleAimbot", () =>
                AimbotOptions.Enabled = !AimbotOptions.Enabled);

            HotkeyComponent.ActionDict.Add("_AimbotOnKey", () =>
                AimbotOptions.OnKey = !AimbotOptions.OnKey);

            HotkeyComponent.ActionDict.Add("_ToggleFreecam", () =>
                MiscOptions.Freecam = !MiscOptions.Freecam);

            HotkeyComponent.ActionDict.Add("_PanicButton", () =>
            {
                MiscOptions.PanicMode = !MiscOptions.PanicMode;
                if (MiscOptions.PanicMode)
                    PlayerCoroutines.DisableAllVisuals();
                else
                    PlayerCoroutines.EnableAllVisuals();
            });

            HotkeyComponent.ActionDict.Add("_SelectPlayer", () =>
            {
                Vector3 aimPos = OptimizationVariables.MainPlayer.look.aim.position;
                Vector3 aimForward = OptimizationVariables.MainPlayer.look.aim.forward;
                
                if (RaycastOptions.EnablePlayerSelection)
                {
                    foreach (GameObject o in RaycastUtilities.Objects)
                    {
                        Player player = o.GetComponent<Player>();
                        if (player != null)
                        {
                            if (VectorUtilities.GetAngleDelta(aimPos, aimForward, o.transform.position) < RaycastOptions.SelectedFOV)
                            {
                                RaycastUtilities.TargetedPlayer = player;
                                break;
                            }
                        }
                    }
                }
            });
        }
        
        [OnSpy]
        public static void Disable()
        {
            if (MiscOptions.WasNightVision)
            {
                NightvisionBeforeSpy = true;
                MiscOptions.NightVision = false;
            }

            if (MiscOptions.Freecam)
            {
                FreecamBeforeSpy = true;
                MiscOptions.Freecam = false;
            }
        }

        [OffSpy]
        public static void Enable()
        {
            if (NightvisionBeforeSpy)
            {
                NightvisionBeforeSpy = false;
                MiscOptions.NightVision = true;
            }

            if (FreecamBeforeSpy)
            {
                FreecamBeforeSpy = false;
                MiscOptions.Freecam = true;
            }
        }

        void OnGUI()
        {
            if (Event.current.type != EventType.Repaint)
                return;
            
            GUI.Label(new Rect(40, 20, 40, 20), OV_PlayerInput.SequenceDiff.ToString());
            GUI.Label(new Rect(40, 40, 40, 20), OV_PlayerInput.ClientSequence.ToString());
        }
        
        void Start()
        {
            Instance = this;
           
            Provider.onClientConnected += () =>
            {
                if (MiscOptions.AlwaysCheckMovementVerification)
                    CheckMovementVerification();
                else
                    MiscOptions.NoMovementVerification = false;
            };

            SkinsUtilities.RefreshEconInfo();
        }

        public void Update()
        {
            if (Camera.main != null && OptimizationVariables.MainCam == null)
                OptimizationVariables.MainCam = Camera.main;

            if (!OptimizationVariables.MainPlayer)
                return;
            
            if (!DrawUtilities.ShouldRun())
                return;
            
            if (Input.GetKeyDown(KeyCode.RightControl))
                OV_PlayerInput.Step = 2;
            
            else if (OV_PlayerInput.Step == 2 && Input.GetKeyUp(KeyCode.RightControl))
                OV_PlayerInput.Step = -1;
            
            if (Input.GetKeyDown(KeyCode.Keypad7))
                OV_PlayerInput.Step = 0;
            
            if (Input.GetKeyDown(KeyCode.Keypad8))
                OV_PlayerInput.Step = 1;
            
            Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Players",
                out int New);

            if (WeaponOptions.OofOnDeath)
            {
                if (New != currentKills)
                {
                    if (currentKills != -1)
                        OptimizationVariables.MainPlayer.GetComponentInChildren<AudioSource>().PlayOneShot(AssetVariables.Audio["oof"], 3);
                    
                    currentKills = New;
                }
            }
            else
                currentKills = New;

            if (MiscOptions.NightVision)
            {
                LevelLighting.vision = ELightingVision.MILITARY;
                LevelLighting.updateLighting();
                PlayerLifeUI.updateGrayscale();
                MiscOptions.WasNightVision = true;
            }
            else if (MiscOptions.WasNightVision)
            {
                LevelLighting.vision = ELightingVision.NONE;
                LevelLighting.updateLighting();
                PlayerLifeUI.updateGrayscale();
                MiscOptions.WasNightVision = false;
            }

            if (MiscOptions.EnableDistanceCrash)
                foreach (SteamPlayer plr in Provider.clients.Where(p => VectorUtilities.GetDistance(p.player.transform.position, OptimizationVariables.MainPlayer.transform.position) < MiscOptions.CrashDistance))
                    PlayerCrashThread.CrashTargets.Add(plr.playerID.steamID);
        }

        public void FixedUpdate()
        {
            if (!OptimizationVariables.MainPlayer) 
                return;
            
            VehicleFlight();
            PlayerFlight();
        }

        public static void PlayerFlight()
        {
            Player plr = OptimizationVariables.MainPlayer;
            
            if (!MiscOptions.PlayerFlight)
            {
                ItemCloudAsset asset = plr.equipment.asset as ItemCloudAsset;
                plr.movement.itemGravityMultiplier = asset?.gravity ?? 1;
                return;
            }

            plr.movement.itemGravityMultiplier = 0;

            float multiplier = MiscOptions.FlightSpeedMultiplier;

            if (HotkeyUtilities.IsHotkeyHeld("_FlyUp"))
                plr.transform.position += plr.transform.up / 5 * multiplier;

            if (HotkeyUtilities.IsHotkeyHeld("_FlyDown"))
                plr.transform.position -= plr.transform.up / 5 * multiplier;

            if (HotkeyUtilities.IsHotkeyHeld("_FlyLeft"))
                plr.transform.position -= plr.transform.right / 5 * multiplier;

            if (HotkeyUtilities.IsHotkeyHeld("_FlyRight"))
                plr.transform.position += plr.transform.right / 5 * multiplier;

            if (HotkeyUtilities.IsHotkeyHeld("_FlyForward"))
                plr.transform.position += plr.transform.forward / 5 * multiplier;

            if (HotkeyUtilities.IsHotkeyHeld("_FlyBackward"))
                plr.transform.position -= plr.transform.forward / 5 * multiplier;
        }

        public static void VehicleFlight()
        {
            InteractableVehicle vehicle = OptimizationVariables.MainPlayer.movement.getVehicle();

            if (vehicle == null)
                return;

            Rigidbody rb = vehicle.GetComponent<Rigidbody>();
            
            if (rb == null)
                return;

            if (MiscOptions.VehicleFly)
            {
                float speedMul = MiscOptions.VehicleUseMaxSpeed ? vehicle.asset.speedMax * Time.fixedDeltaTime : MiscOptions.SpeedMultiplier / 3;
                
                rb.useGravity = false;
                rb.isKinematic = true;
                
                Transform tr = vehicle.transform;

                if (HotkeyUtilities.IsHotkeyHeld("_VFStrafeUp"))
                    tr.position = tr.position + new Vector3(0f, speedMul * 0.65f, 0f);

                if (HotkeyUtilities.IsHotkeyHeld("_VFStrafeDown"))
                    tr.position = tr.position - new Vector3(0f, speedMul * 0.65f, 0f);

                if (HotkeyUtilities.IsHotkeyHeld("_VFStrafeLeft"))
                    rb.MovePosition(tr.position - tr.right * speedMul);

                if (HotkeyUtilities.IsHotkeyHeld("_VFStrafeRight"))
                    rb.MovePosition(tr.position + tr.right * speedMul);

                if (HotkeyUtilities.IsHotkeyHeld("_VFMoveForward"))
                    rb.MovePosition(tr.position + tr.forward * speedMul);

                if (HotkeyUtilities.IsHotkeyHeld("_VFMoveBackward"))
                    rb.MovePosition(tr.position - tr.forward * speedMul);

                if (HotkeyUtilities.IsHotkeyHeld("_VFRotateRight"))
                    tr.Rotate(0f, 1f, 0f);

                if (HotkeyUtilities.IsHotkeyHeld("_VFRotateLeft"))
                    tr.Rotate(0f, -1f, 0f);

                if (HotkeyUtilities.IsHotkeyHeld("_VFRollLeft"))
                    tr.Rotate(0f, 0f, 2f);

                if (HotkeyUtilities.IsHotkeyHeld("_VFRollRight"))
                    tr.Rotate(0f, 0f, -2f);

                if (HotkeyUtilities.IsHotkeyHeld("_VFRotateUp"))
                    vehicle.transform.Rotate(-2f, 0f, 0f);

                if (HotkeyUtilities.IsHotkeyHeld("_VFRotateDown"))
                    vehicle.transform.Rotate(2f, 0f, 0f);
            }
            else
            {
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }

        public static void CheckMovementVerification() =>
            Instance.StartCoroutine(CheckVerification(OptimizationVariables.MainPlayer.transform.position));

        public static IEnumerator CheckVerification(Vector3 LastPos)
        {
            if (Time.realtimeSinceStartup - LastMovementCheck < 0.8f)
                yield break;

            LastMovementCheck = Time.realtimeSinceStartup;

            Vector3 NewPos = LastPos + new Vector3(0, -1337, 0);
            OptimizationVariables.MainPlayer.transform.position = NewPos;
            yield return new WaitForSeconds(1);

            if (VectorUtilities.GetDistance(OptimizationVariables.MainPlayer.transform.position, NewPos) > 100)
                MiscOptions.NoMovementVerification = false;
            else
            {
                MiscOptions.NoMovementVerification = true;
                OptimizationVariables.MainPlayer.transform.position = LastPos + new Vector3(0, 5, 0);
            }
        }
    }
}
