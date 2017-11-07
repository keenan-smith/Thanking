using System.Collections.Generic;

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
            if (!Contains(tab))
                tabs.Add(tab);
        }

        public static bool Contains(MenuTabOption tab)
        {
            bool contains = false;
            foreach (MenuTabOption menutab in tabs)
            {
                if (tab.name == menutab.name) contains = true;
            }
            return contains;
        }

        public MenuTabOption(string name, MenuTab tab)
        {
            this.tab = tab;
            this.name = name;
        }
    }
}
