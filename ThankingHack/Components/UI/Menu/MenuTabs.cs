using Thanking.Options.UIVariables;

namespace Thanking.Components.UI.Menu
{
    public static class MenuTabs
    {
        public static void AddTabs()
        {
            MenuTabOption.Add(new MenuTabOption("visuals", Tabs.VisualsTab.Tab));
            MenuTabOption.Add(new MenuTabOption("weapons", Tabs.WeaponsTab.Tab));
            MenuTabOption.Add(new MenuTabOption("item filter", Tabs.ItemFilterTab.Tab));
            MenuTabOption.Add(new MenuTabOption("misc", Tabs.MiscTab.Tab));
            MenuTabOption.Add(new MenuTabOption("colors", Tabs.ColorsTab.Tab));
        }
    }
}
