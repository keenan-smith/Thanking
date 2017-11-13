using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options;
using Thanking.Utilities;
using Thanking.Coroutines;
using UnityEngine;

namespace Thanking.Components.Basic
{
    [Component]
    public class MiscComponent : MonoBehaviour
    {
        [OnSpy] // idk how this wurks im sry kr4ken pls dont roast me :[
        private void TurnOffMyFuckingNightVision()
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
            if (!DrawUtilities.ShouldRun() || PlayerCoroutines.IsSpying)
                return;
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
                if (!MiscOptions.WasNightVision)
                    return;
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

                if (Input.GetKey(MiscOptions.StrafeUp))
                    tr.position = tr.position + new Vector3(0f, 0.03f * MiscOptions.SpeedMultiplier, 0f);

                if (Input.GetKey(MiscOptions.StrafeDown))
                    tr.position = tr.position - new Vector3(0f, 0.03f * MiscOptions.SpeedMultiplier, 0f);

                if (Input.GetKey(MiscOptions.StrafeLeft))
                    rb.MovePosition(tr.position + tr.right / 5f * MiscOptions.SpeedMultiplier);

                if (Input.GetKey(MiscOptions.StrafeRight))
                    rb.MovePosition(tr.position - tr.right / 5f * MiscOptions.SpeedMultiplier);

                if (Input.GetKey(MiscOptions.MoveForward))
                    rb.MovePosition(tr.position + tr.forward / 5f * MiscOptions.SpeedMultiplier);

                if (Input.GetKey(MiscOptions.MoveBackward))
                    rb.MovePosition(tr.position - tr.forward / 6f * MiscOptions.SpeedMultiplier);

                if (Input.GetKey(MiscOptions.RotateRight))
                    tr.Rotate(0f, 0.6f * MiscOptions.SpeedMultiplier, 0f);

                if (Input.GetKey(MiscOptions.RotateLeft))
                    tr.Rotate(0f, -0.6f * MiscOptions.SpeedMultiplier, 0f);

                if (Input.GetKey(MiscOptions.RollLeft))
                    tr.Rotate(0f, 0f, 0.8f * MiscOptions.SpeedMultiplier);

                if (Input.GetKey(MiscOptions.RollRight))
                    tr.Rotate(0f, 0f, -0.8f * MiscOptions.SpeedMultiplier);

                if (Input.GetKey(MiscOptions.RotateUp))
                    vehicle.transform.Rotate(-0.8f * MiscOptions.SpeedMultiplier, 0f, 0f);

                if (Input.GetKey(MiscOptions.RotateDown))
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
