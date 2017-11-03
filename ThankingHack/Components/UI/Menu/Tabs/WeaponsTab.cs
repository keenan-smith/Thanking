using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Options.AimOptions;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public static class WeaponsTab
    {
        public static void Tab()
        {
            Prefab.MenuArea(new Rect(0, 0, 466, 436), "WEAPONS", () =>
            {
                Prefab.Toggle("Silent Aimbot", ref RaycastOptions.Enabled);
                Prefab.Toggle("Triggerbot", ref TriggerbotOptions.Enabled);
                Prefab.Toggle("No Recoil", ref WeaponOptions.NoRecoil);
                Prefab.Toggle("No Spread", ref WeaponOptions.NoSpread);
                Prefab.Toggle("No Sway", ref WeaponOptions.NoSway);
            });
        }
    }
}
