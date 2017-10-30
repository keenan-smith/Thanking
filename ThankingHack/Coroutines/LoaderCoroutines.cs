using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Variables;
using UnityEngine;
using UnityEngine.Rendering;

namespace Thanking.Coroutines
{
	public static class LoaderCoroutines
	{
		public static IEnumerator LoadAssets()
		{
			yield return new WaitForSeconds(1);

			WWW loader = new WWW("https://debug.ironic.services/client/ThankingAssets.unity3d");
			yield return loader;
			
			AssetBundle bundle = AssetBundle.LoadFromMemory(loader.bytes);
			AssetVariables.ABundle = bundle;

			AssetVariables.GLMaterial = new Material(bundle.LoadAsset<Shader>("Solid")) { hideFlags = HideFlags.HideAndDontSave };
			AssetVariables.Roboto = bundle.LoadAsset<Font>("Roboto-Light");
            AssetVariables.Anton_Regular = bundle.LoadAsset<Font>("Anton-Regular");
            AssetVariables.Calibri = bundle.LoadAsset<Font>("CALIBRI");
            AssetVariables.ThankingLogoLarge = bundle.LoadAsset<Texture2D>("thanking_logo_large");
            AssetVariables.ChamsLit = bundle.LoadAsset<Shader>("chamsLit");
            AssetVariables.ChamsUnlit = bundle.LoadAsset<Shader>("chamsUnlit");
            AssetVariables.WireFrame = bundle.LoadAsset<Shader>("WireframeTransparent");
            AssetVariables.WireFrameCulled = bundle.LoadAsset<Shader>("WireframeTransparentCulled");
        }
	}
}
