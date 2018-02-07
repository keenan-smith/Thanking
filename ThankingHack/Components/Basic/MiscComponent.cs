    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Components.UI;
using Thanking.Options;
using Thanking.Utilities;
using Thanking.Coroutines;
using Thanking.Options.AimOptions;
    using Thanking.Options.VisualOptions;
    using Thanking.Threads;
    using Thanking.Variables;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Thanking.Components.Basic
{
    [Component]
    public class MiscComponent : MonoBehaviour
    {
        public static MiscComponent Instance;
        public static float LastMovementCheck;
        public static bool NightvisionBeforeSpy;
        
        private int currentKills = 0;
        
        void Start()
        {
            Instance = this;
            
            Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Players",
                out currentKills);
            
            HotkeyComponent.ActionDict.Add("_ToggleLogo", () =>
                MiscOptions.LogoEnabled = !MiscOptions.LogoEnabled);
            
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

            Provider.onClientConnected += () =>
            {
                if (MiscOptions.AlwaysCheckMovementVerification)
                    CheckMovementVerification();
                else
                    MiscOptions.NoMovementVerification = false;
            };
        }
        
        [OnSpy] 
        public static void DisableNightVision()
        {
            if (!MiscOptions.WasNightVision) 
                return;
            
            LevelLighting.vision = ELightingVision.NONE;
            
            LevelLighting.updateLighting();
            PlayerLifeUI.updateGrayscale();
            
            MiscOptions.WasNightVision = false;
            NightvisionBeforeSpy = true;
        }

        [OffSpy]
        public static void EnableNightVision()
        {
            if (!NightvisionBeforeSpy)
                return;

            NightvisionBeforeSpy = false;
            LevelLighting.vision = ELightingVision.MILITARY;
            LevelLighting.updateLighting();
            PlayerLifeUI.updateGrayscale();
            MiscOptions.WasNightVision = true;
        }
        
        public void Update()
        {
            if (Player.player != null && OptimizationVariables.MainPlayer == null)
                OptimizationVariables.MainPlayer = Player.player;

            if (Camera.main != null && OptimizationVariables.MainCam == null)
                OptimizationVariables.MainCam = Camera.main;
            
            if (!DrawUtilities.ShouldRun())
                return;

            Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Players",
                out int New);

            if (WeaponOptions.OofOnDeath)
            {
                if (New != currentKills)
                {
                    currentKills = New;
                    OptimizationVariables.MainPlayer.GetComponentInChildren<AudioSource>().PlayOneShot(AssetVariables.Audio["oof"], 2);
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
            else
            {
                if (!MiscOptions.WasNightVision) 
                    return;
                
                LevelLighting.vision = ELightingVision.NONE;
                LevelLighting.updateLighting();
                PlayerLifeUI.updateGrayscale();
                MiscOptions.WasNightVision = false;
            }
        }

        public void FixedUpdate()
        {
            VehicleFlight();
            PlayerFlight();
        }
        
        public static void PlayerFlight()
        {
            Player plr = OptimizationVariables.MainPlayer;

            if (plr == null)
                return;
            
            if (!MiscOptions.PlayerFlight)
            {
                ItemCloudAsset asset = plr.equipment.asset as ItemCloudAsset;
                plr.movement.itemGravityMultiplier = asset?.gravity ?? 1;
                return;
            }

            plr.movement.itemGravityMultiplier = 0;
            
            if (plr == null)
                return;
                    
            Dictionary<string, KeyCode> keys = HotkeyOptions.HotkeyDict;

            float multiplier = MiscOptions.FlightSpeedMultiplier;
            
            if (Input.GetKey(keys["_FlyUp"]))
                plr.transform.position += plr.transform.up / 5 * multiplier;
            
            if (Input.GetKey(keys["_FlyDown"]))
                plr.transform.position -= plr.transform.up / 5 * multiplier;
            
            if (Input.GetKey(keys["_FlyLeft"]))
                plr.transform.position -= plr.transform.right / 5 * multiplier;
            
            if (Input.GetKey(keys["_FlyRight"]))
                plr.transform.position += plr.transform.right / 5 * multiplier;
            
            if (Input.GetKey(keys["_FlyForward"]))
                plr.transform.position += plr.transform.forward / 5 * multiplier;

            if (Input.GetKey(keys["_FlyBackward"]))
                plr.transform.position -= plr.transform.forward / 5 * multiplier;
        }
        
		public static void VehicleFlight()
		{
		    if (OptimizationVariables.MainPlayer == null)
		        return;
		    
            InteractableVehicle vehicle = OptimizationVariables.MainPlayer.movement.getVehicle();

            if (vehicle == null) 
                return;

            Rigidbody rb = vehicle.GetComponent<Rigidbody>();

            if (rb == null) 
                return;

            if (MiscOptions.VehicleFly)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
                Transform tr = vehicle.transform;

                Dictionary<string, KeyCode> keys = HotkeyOptions.HotkeyDict;
                
                if (Input.GetKey(keys["_VFStrafeUp"]))
                    tr.position = tr.position + new Vector3(0f, 0.03f * MiscOptions.SpeedMultiplier, 0f);

                if (Input.GetKey(keys["_VFStrafeDown"]))
                    tr.position = tr.position - new Vector3(0f, 0.03f * MiscOptions.SpeedMultiplier, 0f);

                if (Input.GetKey(keys["_VFStrafeLeft"]))
                    rb.MovePosition(tr.position + tr.right / 5f * MiscOptions.SpeedMultiplier);

                if (Input.GetKey(keys["_VFStrafeRight"]))
                    rb.MovePosition(tr.position - tr.right / 5f * MiscOptions.SpeedMultiplier);

                if (Input.GetKey(keys["_VFMoveForward"]))
                    rb.MovePosition(tr.position + tr.forward / 5f * MiscOptions.SpeedMultiplier);

                if (Input.GetKey(keys["_VFMoveBackward"]))
                    rb.MovePosition(tr.position - tr.forward / 6f * MiscOptions.SpeedMultiplier);

                if (Input.GetKey(keys["_VFRotateRight"]))
                    tr.Rotate(0f, 0.6f * MiscOptions.SpeedMultiplier, 0f);

                if (Input.GetKey(keys["_VFRotateLeft"]))
                    tr.Rotate(0f, -0.6f * MiscOptions.SpeedMultiplier, 0f);

                if (Input.GetKey(keys["_VFRollLeft"]))
                    tr.Rotate(0f, 0f, 0.8f * MiscOptions.SpeedMultiplier);

                if (Input.GetKey(keys["_VFRollRight"]))
                    tr.Rotate(0f, 0f, -0.8f * MiscOptions.SpeedMultiplier);

                if (Input.GetKey(keys["_VFRotateUp"]))
                    vehicle.transform.Rotate(-0.8f * MiscOptions.SpeedMultiplier, 0f, 0f);

                if (Input.GetKey(keys["_VFRotateDown"]))
                    vehicle.transform.Rotate(0.8f * MiscOptions.SpeedMultiplier, 0f, 0f);
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
