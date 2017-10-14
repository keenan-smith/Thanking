using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Thanking.Attributes;
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
