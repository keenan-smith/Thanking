using System;
using System.Collections;
using System.IO;
using System.Net;
using Thanking.Components.UI;
using Thanking.Components.UI.Menu;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Coroutines
{
	public static class LoaderCoroutines
	{
		public static bool IsLoaded;

		public static String AssetPath = $"{Application.dataPath}/assets";

		public static IEnumerator LoadAssets()
		{
			#if DEBUG
			DebugUtilities.Log("Loading assets");
			#endif

			yield return new WaitForSeconds(1);

			Byte[] Loader;

			if (!File.Exists(AssetPath))
			{
				#if DEBUG
				DebugUtilities.Log("Assets not downloaded, downloading now.");
				#endif


				WWW loader = new WWW("http://ironic.services/client/304930/assets");
				yield return loader;

				File.WriteAllBytes(AssetPath, loader.bytes);

				Loader = loader.bytes;
			}
			else
			{
				Loader = File.ReadAllBytes(AssetPath);

				if (HashUtilities.GetSHA2HashString(Loader) !=
				    new WebClient().DownloadString("http://ironic.services/client/304930/Hash").Trim())
				{
					#if DEBUG
					DebugUtilities.Log("Hash mismatch, updating assets");
					#endif

					WWW loader = new WWW("http://ironic.services/client/304930/assets");
					yield return loader;

					File.WriteAllBytes(AssetPath, loader.bytes);

					Loader = File.ReadAllBytes(AssetPath); //loader.bytes;
				}
			}

			AssetBundle bundle = AssetBundle.LoadFromMemory(Loader);
			AssetVariables.ABundle = bundle;

			foreach (Shader s in bundle.LoadAllAssets<Shader>())
				AssetVariables.Materials.Add(s.name, new Material(s) {hideFlags = HideFlags.HideAndDontSave});

			foreach (Shader s in bundle.LoadAllAssets<Shader>())
				AssetVariables.Shaders.Add(s.name, s);

			foreach (Font f in bundle.LoadAllAssets<Font>())
				AssetVariables.Fonts.Add(f.name, f);

			foreach (AudioClip ac in bundle.LoadAllAssets<AudioClip>())
				AssetVariables.Audio.Add(ac.name, ac);

			foreach (Texture2D t in bundle.LoadAllAssets<Texture2D>())
				if (t.name != "Font Texture")
					AssetVariables.Textures.Add(t.name, t);

			ESPComponent.GLMat = AssetVariables.Materials["ESP"];
			ESPComponent.ESPFont = AssetVariables.Fonts["Roboto-Light"];

			MenuComponent._TabFont = AssetVariables.Fonts["Anton-Regular"];
			MenuComponent._TextFont = AssetVariables.Fonts["CALIBRI"];
			MenuComponent._LogoTexLarge = AssetVariables.Textures["thanking_logo_large"];

			ESPCoroutines.Normal = Shader.Find("Standard");
			ESPCoroutines.LitChams = AssetVariables.Shaders["chamsLit"];
			ESPCoroutines.UnlitChams = AssetVariables.Shaders["chamsUnlit"];

			IsLoaded = true;
		}
	}
}
