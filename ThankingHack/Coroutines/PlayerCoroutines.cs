using System.Collections;
using SDG.Unturned;
using Thanking.Utilities;
using Thanking.Components.UI;
using Thanking.Managers.Submanagers;
using UnityEngine;

namespace Thanking.Coroutines
{
	public static class PlayerCoroutines
	{
		public static bool IsSpying;

		public static IEnumerator TakeScreenshot()
		{
			IsSpying = true;

			#if DEBUG
            DebugUtilities.Log("TAKING SCREENSHOT");
			#endif

			SpyManager.InvokePre();
			SpyManager.DestroyComponents();

			if (Player.player.equipment.asset is ItemGunAsset pAsset)
			{
				UseableGun PGun = Player.player.equipment.useable as UseableGun;

				PlayerUI.updateCrosshair(PGun.isAiming
					? WeaponComponent.AssetBackups[pAsset.id][5]
					: WeaponComponent.AssetBackups[pAsset.id][6]);
			}

			yield return new WaitForFixedUpdate();

			#region Take Screenshot

			yield return new WaitForEndOfFrame();
			Texture2D screenshotRaw =
				new Texture2D(Screen.width, Screen.height, (TextureFormat) 3, false)
				{
					name = "Screenshot_Raw",
					hideFlags = (HideFlags) 61
				};

			screenshotRaw.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0, false);

			Texture2D screenshotFinal = new Texture2D(640, 480, (TextureFormat) 3, false)
			{
				name = "Screenshot_Final",
				hideFlags = (HideFlags) 61
			};

			Color[] oldColors = screenshotRaw.GetPixels();
			Color[] newColors = new Color[screenshotFinal.width * screenshotFinal.height];
			float widthRatio = screenshotRaw.width / (float) screenshotFinal.width;
			float heightRatio = screenshotRaw.height / (float) screenshotFinal.height;

			for (int i = 0; i < screenshotFinal.height; i++)
			{
				int num = (int) (i * heightRatio) * screenshotRaw.width;
				int num2 = i * screenshotFinal.width;
				for (int j = 0; j < screenshotFinal.width; j++)
				{
					int num3 = (int) (j * widthRatio);
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
				Player.player.channel.closeWrite("tellScreenshotRelay", ESteamCall.SERVER,
					ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
				Player.player.channel.longBinaryData = false;
			}

			#endregion

			yield return new WaitForFixedUpdate();

			SpyManager.AddComponents();

			SpyManager.InvokePost();

			IsSpying = false;
		}
	}
}
