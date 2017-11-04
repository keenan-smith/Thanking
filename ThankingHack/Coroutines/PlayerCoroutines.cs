﻿using SDG.Unturned;
using System;
using System.Collections;
using System.Reflection;
using Thanking.Attributes;
using Thanking.Components.UI;
using UnityEngine;

namespace Thanking.Coroutines
{
	public static class PlayerCoroutines
	{
        public static bool IsSpying = false;

		public static IEnumerator TakeScreenshot()
		{
            IsSpying = true;

            Debug.Log("TAKING SCREENSHOT");
			foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
				foreach (Type tClass in asm.GetTypes())
					if (tClass.IsClass)
						if (tClass.IsDefined(typeof(SpyComponentAttribute), false))
							UnityEngine.Object.Destroy(Loader.HookObject.GetComponent(tClass));

			if (Player.player.equipment.asset is ItemGunAsset)
			{
				ItemGunAsset PAsset = Player.player.equipment.asset as ItemGunAsset;
				UseableGun PGun = Player.player.equipment.useable as UseableGun;
				if (PGun.isAiming)
					PlayerUI.updateCrosshair(WeaponComponent.AssetBackups[PAsset.id][5]);
				else
					PlayerUI.updateCrosshair(WeaponComponent.AssetBackups[PAsset.id][6]);
			}

			yield return new WaitForFixedUpdate();

			#region Take Screenshot
			yield return new WaitForEndOfFrame();
			Texture2D screenshotRaw = new Texture2D(Screen.width, Screen.height, (TextureFormat)3, false);
			screenshotRaw.name = "Screenshot_Raw";
			screenshotRaw.hideFlags = (HideFlags)61;
			screenshotRaw.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0, false);

			Texture2D screenshotFinal = new Texture2D(640, 480, (TextureFormat)3, false);
			screenshotFinal.name = "Screenshot_Final";
			screenshotFinal.hideFlags = (HideFlags)61;

			Color[] oldColors = screenshotRaw.GetPixels();
			Color[] newColors = new Color[screenshotFinal.width * screenshotFinal.height];
			float widthRatio = (float)screenshotRaw.width / (float)screenshotFinal.width;
			float heightRatio = (float)screenshotRaw.height / (float)screenshotFinal.height;
			for (int i = 0; i < screenshotFinal.height; i++)
			{
				int num = (int)((float)i * heightRatio) * screenshotRaw.width;
				int num2 = i * screenshotFinal.width;
				for (int j = 0; j < screenshotFinal.width; j++)
				{
					int num3 = (int)((float)j * widthRatio);
					newColors[num2 + j] = oldColors[num + num3];
				}
			}
			screenshotFinal.SetPixels(newColors);
			byte[] data = screenshotFinal.EncodeToJPG(33);
			if (data.Length < 30000)
			{
				Player.player.channel.longBinaryData = true;
				Player.player.channel.openWrite();
				Player.player.channel.write(data);
				Player.player.channel.closeWrite("tellScreenshotRelay", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
				Player.player.channel.longBinaryData = false;
			}
			#endregion

			yield return new WaitForFixedUpdate();

			foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
				foreach (Type tClass in asm.GetTypes())
					if (tClass.IsClass)
						if (tClass.IsDefined(typeof(SpyComponentAttribute), false))
							Loader.HookObject.AddComponent(tClass);

            IsSpying = false;
		}
	}
}
