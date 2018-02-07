using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Options.AimOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;
using System.Linq;
using Thanking.Coroutines;

namespace Thanking.Components.UI
{
	[Component]
	[SpyComponent]
	public class WeaponComponent : MonoBehaviour
	{
		public static Dictionary<ushort, float[]> AssetBackups = new Dictionary<ushort, float[]>();
		public static FieldInfo AmmoInfo;

		public static byte Ammo() => 
			(byte)AmmoInfo.GetValue(OptimizationVariables.MainPlayer.equipment.useable);

		public void Start()
		{
			AmmoInfo = typeof(UseableGun).GetField("ammo", BindingFlags.NonPublic | BindingFlags.Instance);
			StartCoroutine(UpdateWeapon());
		}
		
		public void OnGUI()
		{
			if (WeaponOptions.NoSway)
				if (OptimizationVariables.MainPlayer != null && OptimizationVariables.MainPlayer.animator != null)
					OptimizationVariables.MainPlayer.animator.viewSway = Vector3.zero;
			
			if (Event.current.type != EventType.Repaint)
				return;
			
			if (!WeaponOptions.ShowWeaponInfo)
				return;

			if (!DrawUtilities.ShouldRun())
				return;

			if (!(OptimizationVariables.MainPlayer.equipment.asset is ItemGunAsset))
				return;

			GUI.depth = 0;
			ItemGunAsset PAsset = (ItemGunAsset) OptimizationVariables.MainPlayer.equipment.asset;
			string text = $"<size=15>{PAsset.itemName}\nRange: {PAsset.range}</size>";

			DrawUtilities.DrawLabel(ESPComponent.ESPFont, LabelLocation.MiddleLeft, new Vector2(Screen.width - 20, Screen.height / 2), text, Color.black, Color.green, 4);
		}
		
		public static IEnumerator UpdateWeapon()
		{
			while (true)
			{
				yield return new WaitForSeconds(0.1f);
				if (!DrawUtilities.ShouldRun())
					continue;

				if (!(OptimizationVariables.MainPlayer.equipment.asset is ItemGunAsset PAsset))
					continue;
			
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
				}

				if (WeaponOptions.NoRecoil && !PlayerCoroutines.IsSpying)
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

				if (WeaponOptions.NoSpread && !PlayerCoroutines.IsSpying)
				{
					PAsset.spreadAim = 0;
					PAsset.spreadHip = 0;

					PlayerUI.updateCrosshair(0);
				}
				else
				{
					PAsset.spreadAim = AssetBackups[PAsset.id][5];
					PAsset.spreadHip = AssetBackups[PAsset.id][6];

					PlayerUI.updateCrosshair(AssetBackups[PAsset.id][OptimizationVariables.MainPlayer.equipment.secondary ? 5 : 6]);
				}
			
				Reload();
			}
		}

		public static void Reload()
		{
			#if DEBUG
			DebugUtilities.Log($"Ammo: {Ammo()}");
			#endif
			
			if (!WeaponOptions.AutoReload || Ammo() > 0) return;
			
			#if DEBUG
			DebugUtilities.Log("Ammo less than or equal to 0");
			#endif
			
			IEnumerable<InventorySearch> magazineSearch = 
				OptimizationVariables.MainPlayer.inventory.search(EItemType.MAGAZINE,
					((ItemGunAsset) OptimizationVariables.MainPlayer.equipment.asset).magazineCalibers)
					.Where(i => i.jar.item.amount > 0);

			if (!magazineSearch.Any()) return;
			
			InventorySearch search = magazineSearch
					.OrderByDescending(i => i.jar.item.amount)
					.First();

			#if DEBUG
			DebugUtilities.Log("Magazine reloaded");
				#endif

			OptimizationVariables.MainPlayer.channel.send("askAttachMagazine", ESteamCall.SERVER,
				ESteamPacket.UPDATE_UNRELIABLE_BUFFER, search.page, search.jar.x,
				search.jar.y);
		}
	}
}
