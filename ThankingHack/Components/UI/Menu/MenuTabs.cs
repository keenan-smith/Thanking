using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Options.UIVariables;

namespace Thanking.Components.UI.Menu
{
    public static class MenuTabs
    {
        public static void AddTabs()
        {
            MenuTabOption.Add(new MenuTabOption("colors", Tabs.ColorsTab.Tab));
        }
    }
}
