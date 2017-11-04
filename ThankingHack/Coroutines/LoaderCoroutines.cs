using System.Collections;
using Thanking.Components.UI;
using Thanking.Components.UI.Menu;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Coroutines
{
	public static class LoaderCoroutines
	{
		public static IEnumerator LoadAssets()
		{
			yield return new WaitForSeconds(1);

			WWW loader = new WWW("http://debug.ironic.services/client/ThankingAssets.unity3d");
			yield return loader;
			
			AssetBundle bundle = AssetBundle.LoadFromMemory(loader.bytes);
			AssetVariables.ABundle = bundle;
			
			foreach (Shader s in bundle.LoadAllAssets<Shader>())
				AssetVariables.Materials.Add(s.name, new Material(s) { hideFlags = HideFlags.HideAndDontSave });

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
        }
	}
}
