using System.Collections;
using System.IO;
using SDG.Unturned;
using Thanking.Components.UI;
using Thanking.Managers.Submanagers;
using Thanking.Options;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Coroutines
{
	public static class PlayerCoroutines
	{
		public static float LastSpy;
		public static bool IsSpying;
		public static Player SpecPlayer;

		public static IEnumerator TakeScreenshot()
		{
			Player plr = OptimizationVariables.MainPlayer;
			SteamChannel channel = plr.channel;

			switch (MiscOptions.AntiSpyMethod)
			{
				case 0:
				{
					if (Time.realtimeSinceStartup - LastSpy < MiscOptions.MinTimeBetweenSpy || IsSpying) // Checks for spam spy 
						yield break;

					IsSpying = true;

					LastSpy = Time.realtimeSinceStartup;
					
					if (!MiscOptions.PanicMode)
						DisableAllVisuals();

					yield return new WaitForFixedUpdate();
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
						channel.longBinaryData = true;
						channel.openWrite();
						channel.write(data);
						channel.closeWrite("tellScreenshotRelay", ESteamCall.SERVER,
							ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
						channel.longBinaryData = false;
					}	

					yield return new WaitForFixedUpdate();
					yield return new WaitForEndOfFrame();
					IsSpying = false;
					if (!MiscOptions.PanicMode)
						EnableAllVisuals();
					break;
				}
				case 1:
				{
					System.Random r = new System.Random();
					string[] files = Directory.GetFiles(MiscOptions.AntiSpyPath);
					byte[] dataRaw = File.ReadAllBytes(files[r.Next(files.Length)]);
					
					Texture2D texRaw = new Texture2D(2, 2);
					texRaw.LoadImage(dataRaw);
					
					Texture2D screenshotFinal = new Texture2D(640, 480, (TextureFormat) 3, false)
					{
						name = "Screenshot_Final",
						hideFlags = (HideFlags) 61
					};

					Color[] oldColors = texRaw.GetPixels();
					Color[] newColors = new Color[screenshotFinal.width * screenshotFinal.height];
					float widthRatio = texRaw.width / (float) screenshotFinal.width;
					float heightRatio = texRaw.height / (float) screenshotFinal.height;

					for (int i = 0; i < screenshotFinal.height; i++)
					{
						int num = (int) (i * heightRatio) * texRaw.width;
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
						channel.longBinaryData = true;
						channel.openWrite();
						channel.write(data);
						channel.closeWrite("tellScreenshotRelay", ESteamCall.SERVER,
							ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
						channel.longBinaryData = false;
					}
					break;
				}
				case 2:
					break;
				case 3:
				{
					yield return new WaitForFixedUpdate();
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
						channel.longBinaryData = true;
						channel.openWrite();
						channel.write(data);
						channel.closeWrite("tellScreenshotRelay", ESteamCall.SERVER,
							ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
						channel.longBinaryData = false;
					}

					yield return new WaitForFixedUpdate();
					yield return new WaitForEndOfFrame();
					break;
				}
			}

            if (MiscOptions.AlertOnSpy)
                NotificationUtilities.DisplayNotification(EPlayerMessage.INTERACT, "Warning! Your game client was spied.", Color.red, 3);
		}

		public static void DisableAllVisuals()
		{	
			SpyManager.InvokePre();
			if (DrawUtilities.ShouldRun())
				if (OptimizationVariables.MainPlayer.equipment.asset is ItemGunAsset pAsset)
				{
					UseableGun PGun = OptimizationVariables.MainPlayer.equipment.useable as UseableGun;

					PlayerUI.updateCrosshair(PGun.isAiming
						? WeaponComponent.AssetBackups[pAsset.id][5]
						: WeaponComponent.AssetBackups[pAsset.id][6]);
				}

			SpyManager.DestroyComponents();
		}

		public static void EnableAllVisuals()
		{
			SpyManager.AddComponents();
			SpyManager.InvokePost();
		}
	}
}
