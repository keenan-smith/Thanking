using System;
using System.Collections.Generic;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Components.UI
{
	[Component]
	[SpyComponent]
	public class WeaponComponent : MonoBehaviour
	{
		public static Dictionary<ushort, float[]> AssetBackups = new Dictionary<ushort, float[]>();
		public static Vector3 SwayBackup;
		public static ItemWeaponAsset CurrentWeapon;

		private Byte Ammo() => (Byte) typeof(UseableGun).GetField("ammo", ReflectionVariables.PublicInstance)
			.GetValue(Player.player.equipment.useable);
		
		public void OnGUI()
		{
			if (!WeaponOptions.ShowWeaponInfo) return;

			if (!Provider.isConnected || Provider.isLoading)
				return;

			if (!(Player.player.equipment.asset is ItemGunAsset))
				return;

			GUI.depth = 0;
			ItemGunAsset PAsset = (ItemGunAsset) Player.player.equipment.asset;
			string text = "<size=15>";
			text += PAsset.itemName + "\n";
			text += PAsset.range;
			text += "</size>";

			DrawUtilities.DrawLabel(ESPComponent.ESPFont, LabelLocation.MiddleLeft, new Vector2(Screen.width - 20, Screen.height / 2), text, Color.black, Color.green, 4);
		}

		public void Update()
		{
			if (!Provider.isConnected || Provider.isLoading)
				return;

			if (!(Player.player.equipment.asset is ItemGunAsset))
				return;

			ItemGunAsset PAsset = (ItemGunAsset) Player.player.equipment.asset;
			
			if (!AssetBackups.ContainsKey(PAsset.id))
			{
				float[] Backups = new float[7]
				{
					PAsset.recoilAim,
					PAsset.recoilMax_x,
					PAsset.recoilMax_y,
					PAsset.recoilMin_x,
					PAsset.recoilMin_y,
					PAsset.spreadAim,
					PAsset.spreadHip
				};
				
				Backups[6] = PAsset.spreadHip;

				AssetBackups.Add(PAsset.id, Backups);

				SwayBackup = Player.player.animator.viewSway == Vector3.zero ? SwayBackup : Player.player.animator.viewSway;
			}

			if (WeaponOptions.NoRecoil)
			{
				PAsset.recoilAim = 0;
				PAsset.recoilMax_x = 0;
				PAsset.recoilMax_y = 0;
				PAsset.recoilMin_x = 0;
				PAsset.recoilMin_y = 0;
			}
			else
			{
				PAsset.recoilAim = AssetBackups[PAsset.id][0];
				PAsset.recoilMax_x = AssetBackups[PAsset.id][1];
				PAsset.recoilMax_y = AssetBackups[PAsset.id][2];
				PAsset.recoilMin_x = AssetBackups[PAsset.id][3];
				PAsset.recoilMin_y = AssetBackups[PAsset.id][4];
			}

			if (WeaponOptions.NoSpread)
			{
				PAsset.spreadAim = 0;
				PAsset.spreadHip = 0;

				PlayerUI.updateCrosshair(0);
			}
			else
			{
				PAsset.spreadAim = AssetBackups[PAsset.id][5];
				PAsset.spreadHip = AssetBackups[PAsset.id][6];

				PlayerUI.updateCrosshair(AssetBackups[PAsset.id][Player.player.equipment.secondary ? 5 : 6]);
			}
			
			#region AutoReload
			
			if (WeaponOptions.AutoReload && Player.player != null && Player.player.equipment != null &&
			    Player.player.equipment.useable is UseableGun && Ammo() == 0)
			{
				InventorySearch[] magazineSearch = Player.player.inventory.search(
					Player.player.inventory.search(EItemType.MAGAZINE,
						((ItemGunAsset) Player.player.equipment.asset).magazineCalibers)).ToArray();
				
				if (magazineSearch.Length > 0)
				{
					Byte b = 0;
					Byte b2 = 255;

					for (Byte i = 0; i < magazineSearch.Length; i++)
					{
						if (magazineSearch[i].jar.item.amount > b)
						{
							b = magazineSearch[i].jar.item.amount;
							b2 = i;
						}
					}

					if (b2 != 255)
					{
						Player.player.channel.send("askAttachMagazine", ESteamCall.SERVER,
							ESteamPacket.UPDATE_UNRELIABLE_BUFFER, magazineSearch[b2].page, magazineSearch[b2].jar.x,
							magazineSearch[b2].jar.y);
					}
				}
			}
			
			#endregion

			Player.player.animator.viewSway = WeaponOptions.NoSway ? Vector3.zero : SwayBackup;
		}
	}
}
