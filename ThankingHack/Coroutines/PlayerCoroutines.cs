using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Thanking.Attributes;
using Thanking.Components.Basic;
using Thanking.Components.UI;
using Thanking.Options.VisualOptions;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Coroutines
{
	public static class PlayerCoroutines
	{
		public static IEnumerator TakeScreenshot()
		{
			Debug.Log("TAKING SCREENSHOT");
			foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
				foreach (Type tClass in asm.GetTypes())
					if (tClass.IsClass)
						if (tClass.IsDefined(typeof(SpyComponentAttribute), false))
							UnityEngine.Object.Destroy(Loader.HookObject.GetComponent(tClass));

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
		}
	}
}
