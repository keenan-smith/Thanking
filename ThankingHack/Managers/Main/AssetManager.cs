using Thanking.Components.Basic;
using Thanking.Coroutines;
using Thanking.Misc;
using Thanking.Utilities;
using Thnkng;

namespace Thanking.Managers.Main
{
	public static class AssetManager
	{
		public static void Init()
		{
			//#if DEBUG
			DebugUtilities.Log("Starting asset manager...");
			//#endif
			
			// Start the LoadAssets coroutine
			Ldr.HookObject.GetComponent<CoroutineComponent>().StartCoroutine(LoaderCoroutines.LoadAssets());
		}
	}
}
