using System.Collections.Generic;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Components.UI;
using Thanking.Options;
using Thanking.Utilities;
using Thanking.Coroutines;
using Thanking.Options.AimOptions;
using Thanking.Variables;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Thanking.Components.Basic
{
    [Component]
    public class MiscComponent : MonoBehaviour
    {
        private int[] values;
        private int currentKills = 0;

        void Start()
        {
            Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Players",
                out currentKills);
            
            values = (int[])System.Enum.GetValues(typeof(KeyCode));
        }
        
        [OnSpy] 
        public static void TurnOffMyFuckingNightVision()
        {
            if (!MiscOptions.WasNightVision) return;
            
            LevelLighting.vision = ELightingVision.NONE;
            
            LevelLighting.updateLighting();
            PlayerLifeUI.updateGrayscale();
            
            MiscOptions.WasNightVision = false;
        }

        public void Update()
        {
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
                    Player.player.GetComponentInChildren<AudioSource>().PlayOneShot(AssetVariables.Audio["oof"], 2);
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
            InteractableVehicle vehicle = Player.player.movement.getVehicle();

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
    }
}
