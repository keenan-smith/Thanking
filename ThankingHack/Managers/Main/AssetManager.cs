using Thinking.Components.Basic;
using Thinking.Coroutines;
using Thinking.Utilities;
using Thnkng;

namespace Thinking.Managers.Main
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
