using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Managers.Main;
using Thanking.Options;
using Thanking.Options.AimOptions;
using Thanking.Options.VisualOptions;
using Thanking.Threads;
using UnityEngine;

namespace Thanking.Components.Basic
{
	[SpyComponent]
	[Component]
	public class InputComponent : MonoBehaviour
	{
		public bool isOrbiting() => Player.player.look.isOrbiting;
		public bool isTracking() => Player.player.look.isTracking;
		public bool isLocking() => Player.player.look.isLocking;
		public bool isFocusing() => Player.player.look.isFocusing;
		public bool isSmoothing() => Player.player.look.isSmoothing;

		public void Update()
		{
			if (Input.GetKeyDown(TriggerbotOptions.Toggle))
			{
				TriggerbotOptions.Enabled = !TriggerbotOptions.Enabled;

				if (!TriggerbotOptions.Enabled)
					TriggerbotOptions.IsFiring = false;
			}

			if (Input.GetKeyDown(ESPOptions.Toggle))
				ESPOptions.Enabled = !ESPOptions.Enabled;

			if (Input.GetKeyDown(MiscOptions.LogoToggle))
				MiscOptions.LogoEnabled = !MiscOptions.LogoEnabled;

			if (Input.GetKeyDown(MiscOptions.ReloadConfig))
				ConfigManager.Init();

			if (Input.GetKeyDown(MiscOptions.SaveConfig))
				ConfigManager.SaveConfig(ConfigManager.CollectConfig());

			if (Input.GetKeyDown(MiscOptions.CrashServer))
				CrashThread.CrashServerEnabled = !CrashThread.CrashServerEnabled;


			#region bypass shit

			if (Provider.isLoading || !Provider.isConnected)
				return;

			if (!Player.player.life.isDead && !PlayerUI.window.showCursor && Input.GetKeyDown(ControlsSettings.perspective))
			{
				if (!(Provider.cameraMode == ECameraMode.BOTH ||
					  Provider.cameraMode == ECameraMode.VEHICLE && Player.player.stance.stance == EPlayerStance.DRIVING))
				{
					Player.player.look.setPerspective(Player.player.look.perspective == EPlayerPerspective.FIRST
						? EPlayerPerspective.THIRD
						: EPlayerPerspective.FIRST);
				}
			}

			if (Input.GetKeyDown(KeyCode.Keypad1))
			{
				Player.player.look.isOrbiting = !isOrbiting();

				if (isOrbiting() && !isTracking() && !isLocking() && !isFocusing())
					Player.player.look.isTracking = true;
			}

			if (Input.GetKeyDown(KeyCode.Keypad2))
			{
				Player.player.look.isTracking = !isTracking();

				if (isTracking())
				{
					Player.player.look.isLocking = false;
					Player.player.look.isFocusing = false;
				}
			}

			if (Input.GetKeyDown(KeyCode.Keypad3))
			{
				Player.player.look.isLocking = !isLocking();
				if (isLocking())
				{
					Player.player.look.isTracking = false;
					Player.player.look.isFocusing = false;
					Player.player.look.lockPosition = Player.player.first.position;
				}
			}

			if (Input.GetKeyDown(KeyCode.Keypad4))
			{
				Player.player.look.isFocusing = !isFocusing();

				if (isFocusing())
				{
					Player.player.look.isTracking = false;
					Player.player.look.isLocking = false;
					Player.player.look.lockPosition = Player.player.first.position;
				}
			}

			if (Input.GetKeyDown(KeyCode.Keypad5))
				Player.player.look.isSmoothing = !isSmoothing();

			if (Input.GetKeyDown(KeyCode.Keypad6))
			{
				if (PlayerWorkzoneUI.active)
				{
					PlayerWorkzoneUI.close();
					PlayerLifeUI.open();
				}
				else
				{
					PlayerWorkzoneUI.open();
					PlayerLifeUI.close();
				}
			}

			#endregion
		}
	}
}
