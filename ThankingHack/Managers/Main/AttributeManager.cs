using Thanking.Managers.Submanagers;

namespace Thanking.Managers.Main
{
    public static class AttributeManager
    {
        public static void Init()
        {
	        #if DEBUG
			DebugUtilities.Log("Initializing attribute manager...");
	        #endif
	        
			InitializationManager.Load();
			ComponentManager.Load();
            OverrideManager.Load();
			ThreadManager.Load();
	        SpyManager.Load();
		}
    }
}
