using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
using Thanking.Options;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
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

		public void OnGUI()
		{
			if (!WeaponOptions.ShowWeaponInfo)
				return;

			if (!Provider.isConnected || Provider.isLoading)
				return;

			if (!(Player.player.equipment.asset is ItemGunAsset))
				return;

			ItemGunAsset PAsset = Player.player.equipment.asset as ItemGunAsset;
			string text = "<size=15>";
			text += (PAsset.itemName + "\n");
			text += PAsset.range;
			text += "</size>";

			DrawUtilities.DrawLabel(Variables.LabelLocation.MiddleLeft, new Vector2(Screen.width - 20, Screen.height / 2), text, Color.black, Color.green, 4);
		}

		public void Update()
		{
			if (!Provider.isConnected || Provider.isLoading)
				return;

			if (!(Player.player.equipment.asset is ItemGunAsset))
				return;

			ItemGunAsset PAsset = Player.player.equipment.asset as ItemGunAsset;
			
			if (!AssetBackups.ContainsKey(PAsset.id))
			{
				float[] Backups = new float[7];
				Backups[0] = PAsset.recoilAim;
				Backups[1] = PAsset.recoilMax_x;
				Backups[2] = PAsset.recoilMax_y;
				Backups[3] = PAsset.recoilMin_x;
				Backups[4] = PAsset.recoilMin_y;
				Backups[5] = PAsset.spreadAim;
				Backups[6] = PAsset.spreadHip;

				AssetBackups.Add(PAsset.id, Backups);

				SwayBackup = (Player.player.animator.viewSway == Vector3.zero ? SwayBackup : Player.player.animator.viewSway);
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

			if (WeaponOptions.NoSway)
				Player.player.animator.viewSway = Vector3.zero;
			else
				Player.player.animator.viewSway = SwayBackup;
		}
	}
}
