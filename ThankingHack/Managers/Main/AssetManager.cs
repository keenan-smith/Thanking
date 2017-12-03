using Thanking.Components.Basic;
using Thanking.Coroutines;
using Thanking.Utilities;

namespace Thanking.Managers.Main
{
	public static class AssetManager
	{
		public static void Init()
		{
			#if DEBUG
			DebugUtilities.Log("Starting asset manager...");
			#endif
			
			// Start the LoadAssets coroutine
			Loader.HookObject.GetComponent<CoroutineComponent>().StartCoroutine(LoaderCoroutines.LoadAssets());
		}
	}
}
