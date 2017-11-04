using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thanking.Options.UIVariables
{
    public class MenuTabOption
    {
        public delegate void MenuTab();
        public MenuTab tab;
        public string name;
        public bool enabled = false;
        public static MenuTabOption CurrentTab;

        public static List<MenuTabOption> tabs = new List<MenuTabOption>();

        public static void Add(MenuTabOption tab)
        {
            tabs.Add(tab);
        }

        public MenuTabOption(string name, MenuTab tab)
        {
            this.tab = tab;
            this.name = name;
        }
    }
}
