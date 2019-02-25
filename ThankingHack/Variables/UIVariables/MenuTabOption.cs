using System.Collections.Generic;
using System.Linq;

namespace Thanking.Variables.UIVariables
{
    public class MenuTabOption
    {
        public delegate void MenuTab();
        public MenuTab tab;
        public string name;
        public bool enabled = false;
        public static MenuTabOption CurrentTab;
        public int page;

        private static int cTabIndex = 0;
        private static int cPageIndex = 0;

        public static List<MenuTabOption>[] tabs = new List<MenuTabOption>[2]
        {
            new List<MenuTabOption>(),
            new List<MenuTabOption>()
        };
        
        public static void Add(MenuTabOption tab)
        {
            if (!Contains(tab)) {
                tabs[cPageIndex].Add(tab);
                tab.page = cPageIndex;
                
                cTabIndex++;

                if (cTabIndex % 9 == 0)
                {
                    cTabIndex = 0;
                    cPageIndex++;
                }
            }
        }

        public static bool Contains(MenuTabOption tab)
        {
            bool contains = false;
            foreach (MenuTabOption menutab in tabs.SelectMany(t => t))
                if (tab.name == menutab.name) contains = true;
            return contains;
        }

        public MenuTabOption(string name, MenuTab tab)
        {
            this.tab = tab;
            this.name = name;
        }
    }
}
