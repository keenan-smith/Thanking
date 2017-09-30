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
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                Attribute[] attributes = Attribute.GetCustomAttributes(type);

                for (int i = 0; i < attributes.Length; i++)
                {
                    Attribute attribute = attributes[i];
                    
                    if (attribute is OverrideAttribute)
                    {
                        OverrideAttribute a = (OverrideAttribute) attribute;
                        OverrideManager.OverrideClass(a.OverrideClass, type);
                        return;
                    }

                    if (attribute is ComponentAttribute)
                        LOADING.Loader.HookObject.AddComponent(type);
                    
                    //more shit to come here probably
                }
            }
        }
    }
}
