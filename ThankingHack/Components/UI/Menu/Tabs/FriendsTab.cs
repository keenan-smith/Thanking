using System;
using Thanking.Options;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class FriendsTab
    {
        public static void Tab()
        {
            Prefab.MenuArea(new Rect(0, 0, 466, 436), "FRIENDS", () =>
            {
                GUILayout.Label("You have none.", Prefab._TextStyle);
            });
        }
    }
}