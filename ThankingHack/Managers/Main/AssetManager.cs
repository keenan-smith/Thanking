using Thanking.Components.Basic;
using Thanking.Coroutines;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Managers.Main
{
	public static class AssetManager
	{
		public static void Init()
		{
			#if DEBUG
			DebugUtilities.Log("Starting asset manager...");
			#endif
			
			Loader.HookObject.GetComponent<CoroutineComponent>().StartCoroutine(LoaderCoroutines.LoadAssets());
		}
	}
}
