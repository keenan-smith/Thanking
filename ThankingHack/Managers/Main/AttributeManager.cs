using UnityEngine;
using Thanking.Managers.Submanagers;

namespace Thanking.Managers.Main
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
