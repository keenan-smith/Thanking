using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using Thanking.Attributes;
using Thanking.Components.UI.Menu.Tabs;
using Thanking.Options;
using Thanking.Utilities;
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
        public static int[] Keys = (int[])System.Enum.GetValues(typeof(KeyCode));
        
        public void Update()
        {
            if (HotkeyUtilities.NeedsKey)
            {
                for(int i = 0; i < Keys.Length; i++) 
                {
                    if (!Input.GetKeyDown((KeyCode) Keys[i]))
                        continue;
                    
                    HotkeyUtilities.ReturnKey = (KeyCode) Keys[i];
                    HotkeyUtilities.NeedsKey = false;

                    break;
                }
            }
            
            foreach (KeyValuePair<string, Action> kvp in ActionDict)
                if (HotkeyOptions.HotkeyDict.ContainsKey(kvp.Key) && Input.GetKeyDown(HotkeyOptions.HotkeyDict[kvp.Key]))
                    kvp.Value();
        }
    }
}