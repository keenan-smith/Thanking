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
			AssetBundle bundle = AssetBundle.LoadFromFile(Application.dataPath + "/ThankingAssets.unity3d");
			AssetVariables.ABundle = bundle;

			AssetVariables.GLMaterial = new Material(bundle.LoadAsset<Shader>("Solid")) { hideFlags = HideFlags.HideAndDontSave };
			AssetVariables.GLMaterial.SetInt("_SrcBlend", (int)BlendMode.One);
			AssetVariables.GLMaterial.SetInt("_DstBlend", (int)BlendMode.One);
			AssetVariables.GLMaterial.SetInt("_Cull", 0);
			AssetVariables.GLMaterial.SetInt("_ZWrite", 0);

			AssetVariables.Roboto = bundle.LoadAsset<Font>("Roboto-Medium");
		}
	}
}
