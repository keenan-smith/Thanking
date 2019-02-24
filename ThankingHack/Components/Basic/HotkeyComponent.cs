using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;
using Thanking.Attributes;
using Thanking.Components.UI.Menu;
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
        public static bool NeedsKeys;
        public static bool StopKeys;
        public static int CurrentKeyCount;
        public static List<KeyCode> CurrentKeys;
        
        public static Dictionary<string, Action> ActionDict = new Dictionary<string, Action>();
        public static KeyCode[] Keys = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));
        
        public void Update()
        {
            if (NeedsKeys)
            {
                List<KeyCode> ClonedKeys = CurrentKeys.ToList();
                CurrentKeys.Clear();
            
                foreach (KeyCode k in Keys)
                    if (Input.GetKey(k))
                        CurrentKeys.Add(k);

                if (CurrentKeys.Count < CurrentKeyCount && CurrentKeyCount > 0)
                {
                    CurrentKeys = ClonedKeys;
                    StopKeys = true;
                }

                CurrentKeyCount = CurrentKeys.Count;
            }

            if (MenuComponent.IsInMenu)
                return;
            
            foreach (KeyValuePair<string, Action> kvp in ActionDict)
                if (HotkeyUtilities.IsHotkeyDown(kvp.Key))
                    kvp.Value();
        }

        public static void Clear()
        {
            NeedsKeys = false;
            StopKeys = false;
            CurrentKeyCount = 0;
            CurrentKeys = new List<KeyCode>();
        }
    }
}