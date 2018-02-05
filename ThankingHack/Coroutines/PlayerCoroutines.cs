using System.Collections;
using SDG.Unturned;
using Thanking.Utilities;
using Thanking.Components.UI;
using Thanking.Managers.Submanagers;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Coroutines
{
	public static class PlayerCoroutines
	{
		public static float LastSpy;
		public static bool IsSpying;

		public static IEnumerator TakeScreenshot()
		{
			if (Time.realtimeSinceStartup - LastSpy < 0.5f) // Checks for spam spy 
			{
				yield return new WaitForSeconds(0.5f); // Make sure they can't fuck us over
				yield break;
			}

			IsSpying = true;
			LastSpy = Time.realtimeSinceStartup;

			#if DEBUG
            DebugUtilities.Log("TAKING SCREENSHOT");
			#endif

			SpyManager.InvokePre();
			SpyManager.DestroyComponents();

			LevelLighting.updateLighting();

			if (OptimizationVariables.MainPlayer.equipment.asset is ItemGunAsset pAsset)
			{
				UseableGun PGun = OptimizationVariables.MainPlayer.equipment.useable as UseableGun;

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
				OptimizationVariables.MainPlayer.channel.longBinaryData = true;
				OptimizationVariables.MainPlayer.channel.openWrite();
				OptimizationVariables.MainPlayer.channel.write(data);
				OptimizationVariables.MainPlayer.channel.closeWrite("tellScreenshotRelay", ESteamCall.SERVER,
					ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
				OptimizationVariables.MainPlayer.channel.longBinaryData = false;
			}

			#endregion

			yield return new WaitForFixedUpdate();

			SpyManager.AddComponents();
			SpyManager.InvokePost();

			LevelLighting.updateLighting();

			IsSpying = false;
		}
	}
}
