using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Components.Basic;
using Thanking.Coroutines;
using UnityEngine;

namespace Thanking.Managers.Main
{
	public static class AssetManager
	{
		public static void Init()
		{
			Debug.Log("Starting asset manager...");
			Loader.HookObject.GetComponent<CoroutineComponent>().StartCoroutine(LoaderCoroutines.LoadAssets());
		}
	}
}
