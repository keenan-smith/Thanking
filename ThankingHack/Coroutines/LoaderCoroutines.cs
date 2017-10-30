﻿using System;
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

			foreach (Shader s in bundle.LoadAllAssets<Shader>())
				AssetVariables.Materials.Add(s.name, new Material(s) { hideFlags = HideFlags.HideAndDontSave });

			foreach (Font f in bundle.LoadAllAssets<Font>())
				AssetVariables.Fonts.Add(f.name, f);

			foreach (AudioClip ac in bundle.LoadAllAssets<AudioClip>())
				AssetVariables.Audio.Add(ac.name, ac);
		}
	}
}
