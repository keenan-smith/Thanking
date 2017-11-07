using System;
using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Components.UI;
using Thanking.Managers.Main;
using Thanking.Options;
using Thanking.Options.AimOptions;
using Thanking.Options.VisualOptions;
using UnityEngine;

namespace Thanking.Components.Basic
{
	[SpyComponent]
	[Component]
	public class InputComponent : MonoBehaviour
	{
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

			if (Input.GetKeyDown(SphereOptions.Toggle))
				SphereOptions.Enabled = !SphereOptions.Enabled;

			if (Input.GetKeyDown(MiscOptions.LogoToggle))
				MiscOptions.LogoEnabled = !MiscOptions.LogoEnabled;

			if (Input.GetKeyDown(MiscOptions.ReloadConfig))
				ConfigManager.Init();

			if (Input.GetKeyDown(MiscOptions.SaveConfig))
				ConfigManager.SaveConfig(ConfigManager.CollectConfig());

			if (Input.GetKeyDown(MiscOptions.PanicButton))
			{
				MiscOptions.VisualsEnabled = !MiscOptions.VisualsEnabled;
				if (MiscOptions.VisualsEnabled)
				{
					foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
						foreach (Type tClass in asm.GetTypes())
							if (tClass.IsClass)
								if (tClass.IsDefined(typeof(SpyComponentAttribute), false))
									Loader.HookObject.AddComponent(tClass);
				}
				else
				{
					foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
						foreach (Type tClass in asm.GetTypes())
							if (tClass.IsClass)
								if (tClass.IsDefined(typeof(SpyComponentAttribute), false))
									Destroy(Loader.HookObject.GetComponent(tClass));

					if (Player.player.equipment.asset is ItemGunAsset)
					{
						ItemGunAsset PAsset = Player.player.equipment.asset as ItemGunAsset;
						UseableGun PGun = Player.player.equipment.useable as UseableGun;
						if (PGun.isAiming)
							PlayerUI.updateCrosshair(WeaponComponent.AssetBackups[PAsset.id][5]);
						else
							PlayerUI.updateCrosshair(WeaponComponent.AssetBackups[PAsset.id][6]);
					}
				}
			}

			if (Input.GetKeyDown(KeyCode.KeypadPeriod))
				foreach (SteamPlayer player in Provider.clients)
				{
					WalkingPlayerInputPacket packet = new WalkingPlayerInputPacket();
					packet.analog = 0;
					packet.keys = 0;
					packet.pitch = 0;
					packet.yaw = 0;
					packet.clientsideInputs = null;
					packet.serversideInputs = null;
					packet.recov = int.MaxValue;
					packet.sequence = int.MaxValue;
					packet.position = new Vector3(0, 0, 0);

					SteamChannel c = Player.player.channel;
					c.openWrite();
					packet.write(c);
					c.closeWrite("askInput", player.playerID.steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_INSTANT);
				}
		}
	}
}
