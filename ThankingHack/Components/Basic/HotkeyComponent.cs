using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using Thanking.Attributes;
using Thanking.Components.UI.Menu.Tabs;
using Thanking.Options;
using UnityEngine;

namespace Thanking.Components.Basic
{
    /// <summary>
    /// Used in order to check for ref
    /// </summary>
    [Component]
    public class HotkeyComponent : MonoBehaviour
    {
        public static Dictionary<string, Action> ActionDict = new Dictionary<string, Action>();

        public void Update()
        {
            if (!HotkeyTab.IsInitialized)
                return;
            
            foreach (KeyValuePair<string, Action> kvp in ActionDict)
                if (HotkeyOptions.HotkeyDict.ContainsKey(kvp.Key) && Input.GetKeyDown(HotkeyOptions.HotkeyDict[kvp.Key]))
                    kvp.Value();
        }
    }
}