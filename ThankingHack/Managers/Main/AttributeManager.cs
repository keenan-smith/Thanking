using Thanking.Managers.Submanagers;
using Thanking.Utilities;

namespace Thanking.Managers.Main
{
    public static class AttributeManager
    {
        public static void Init()
        {
	        #if DEBUG
			DebugUtilities.Log("Initializing attribute manager...");
#endif

            DebugUtilities.Log("Initializing InitializationManager");
            InitializationManager.Load();
            DebugUtilities.Log("Initialized InitializationManager");
            ComponentManager.Load();
            DebugUtilities.Log("Initialized ComponentManager");
            OverrideManager.Load();
            DebugUtilities.Log("Initialized OverrideManager");
            ThreadManager.Load();
            DebugUtilities.Log("Initialized ThreadManager");
            SpyManager.Load();
            DebugUtilities.Log("Initialized SpyManager");
        }
    }
}
