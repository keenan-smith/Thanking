using UnityEngine;

namespace Thanking.Managers
{
    public static class AttributeManager
    {
        public static void Init()
        {
            Debug.Log("Initializing attribute manager...");
            ComponentManager.Load();
            OverrideManager.Load();
			ThreadManager.Load();
        }
    }
}
