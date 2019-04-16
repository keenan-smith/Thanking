using System;
using System.Collections;
using System.IO;
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

		public static String AssetPath = $"{Application.dataPath}/sharedassets5.assets";

		/// <summary>
		/// Loads Thanking's assets from online or offline assetbundle
		/// </summary>
		/// <returns></returns>
		public static IEnumerator LoadAssets()
		{
			//#if DEBUG
			DebugUtilities.Log("Loading assets");
			//#endif

			yield return new WaitForSeconds(1);

			Byte[] Loader = File.ReadAllBytes(AssetPath);
			Console.WriteLine(AssetPath);

			/*if (!File.Exists(AssetPath)) // Assets don't exist, download them
			{
				#if DEBUG
				DebugUtilities.Log("Assets not downloaded, downloading now.");
				#endif

 
				WWW loader = new WWW("http://ironic.services/client/304930/assets"); // Download the assets
				yield return loader;

				File.WriteAllBytes(AssetPath, loader.bytes); // Save the assets for later use

				Loader = loader.bytes; // For actually loading assets from the bundle
			}
			else // Assets exist, check if they are updated
			{
				Loader = File.ReadAllBytes(AssetPath);

				if (HashUtilities.GetSHA2HashString(Loader) != // If there is a hash mismatch (usually outdated assets)
				    new WebClient().DownloadString("http://ironic.services/client/304930/Hash").Trim())
				{
					#if DEBUG		
					DebugUtilities.Log("Hash mismatch, updating assets");
					#endif

					WWW loader = new WWW("http://ironic.services/client/304930/assets"); // Download updated assets
					yield return loader;

					File.WriteAllBytes(AssetPath, loader.bytes); // Save assets

					Loader = loader.bytes;
				}
			}*/

			AssetBundle bundle = AssetBundle.LoadFromMemory(Loader); // Here and below are just loading assets from the bundle
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
			
			DebugUtilities.Log("Assets loaded!");
		}
	}
}
