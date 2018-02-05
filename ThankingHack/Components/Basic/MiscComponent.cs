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
        
        private int[] values;
        private int currentKills = 0;

        void Start()
        {
            Instance = this;
            
            Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Players",
                out currentKills);
            
            values = (int[])System.Enum.GetValues(typeof(KeyCode));
            
            HotkeyComponent.ActionDict.Add("_ToggleLogo", () =>
                MiscOptions.LogoEnabled = !MiscOptions.LogoEnabled);
            
            HotkeyComponent.ActionDict.Add("_ToggleAimbot", () =>
                AimbotOptions.Enabled = !AimbotOptions.Enabled);

            HotkeyComponent.ActionDict.Add("_AimbotOnKey", () =>
                AimbotOptions.OnKey = !AimbotOptions.OnKey);
            
            HotkeyComponent.ActionDict.Add("_ToggleFreecam", () => 
                OptimizationVariables.MainPlayer.look.isOrbiting = !OptimizationVariables.MainPlayer.look.isOrbiting);
            
            HotkeyComponent.ActionDict.Add("_PanicButton", () =>
            {
                MiscOptions.PanicMode = !MiscOptions.PanicMode;
                if (MiscOptions.PanicMode)
                {
                    foreach (Type T in Assembly.GetExecutingAssembly().GetTypes())
                        if (T.IsDefined(typeof(SpyComponentAttribute), false))
                            Destroy(Loader.HookObject.GetComponent(T));
                }
                else
                {
                    foreach (Type T in Assembly.GetExecutingAssembly().GetTypes())
                        if (T.IsDefined(typeof(SpyComponentAttribute), false))
                            Loader.HookObject.AddComponent(T);
                }
            });

            Provider.onClientConnected += () =>
            {
                if (MiscOptions.AlwaysCheckMovementVerification)
                    CheckMovementVerification();
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
        }

        public void Update()
        {
            if (MiscOptions.SpectatedPlayer != null)
                OptimizationVariables.MainPlayer.look.orbitPosition = MiscOptions.SpectatedPlayer.transform.position;
            
            if (HotkeyUtilities.NeedsKey)
            {
                for(int i = 0; i < values.Length; i++) 
                {
                    if (!Input.GetKeyDown((KeyCode) values[i]))
                        continue;
                    
                    HotkeyUtilities.ReturnKey = (KeyCode) values[i];
                    HotkeyUtilities.NeedsKey = false;

                    break;
                }
            }
            
            if (!DrawUtilities.ShouldRun() || PlayerCoroutines.IsSpying)
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
            
            VehicleFlight();
            
            if (MiscOptions.NightVision)
            {
                LevelLighting.vision = ELightingVision.MILITARY;
                LevelLighting.updateLighting();
                PlayerLifeUI.updateGrayscale();
                MiscOptions.WasNightVision = true;
            }
            else
            {
                if (!MiscOptions.WasNightVision) return;
                
                LevelLighting.vision = ELightingVision.NONE;
                LevelLighting.updateLighting();
                PlayerLifeUI.updateGrayscale();
                MiscOptions.WasNightVision = false;
            }
        }

		public static void VehicleFlight()
        {
            InteractableVehicle vehicle = OptimizationVariables.MainPlayer.movement.getVehicle();

            if (vehicle == null) return;

            Rigidbody rb = vehicle.GetComponent<Rigidbody>();

            if (rb == null) return;

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
            Vector3 NewPos = LastPos + new Vector3(0, 1337, 0);
            OptimizationVariables.MainPlayer.transform.position = NewPos;
            yield return new WaitForSeconds(0.5f);

            if (VectorUtilities.GetDistance(OptimizationVariables.MainPlayer.transform.position, NewPos) > 100)
                MiscOptions.NoMovementVerification = false;
            else
            {
                MiscOptions.NoMovementVerification = true;
                OptimizationVariables.MainPlayer.transform.position = LastPos;
            }
        }
    }
}
