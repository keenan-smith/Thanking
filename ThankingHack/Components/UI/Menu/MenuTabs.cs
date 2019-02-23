using Thinking.Components.UI.Menu.Tabs;
using Thinking.Options.UIVariables;

namespace Thinking.Components.UI.Menu
{
    public static class MenuTabs
    {
        public static void AddTabs()
        {
            MenuTabOption.Add(new MenuTabOption("visuals", VisualsTab.Tab));
            MenuTabOption.Add(new MenuTabOption("aimbot", AimbotTab.Tab));
            MenuTabOption.Add(new MenuTabOption("weapons", WeaponsTab.Tab));
	        MenuTabOption.Add(new MenuTabOption("silent aim", SilentAimTab.Tab));
            MenuTabOption.Add(new MenuTabOption("players", PlayersTab.Tab));
            MenuTabOption.Add(new MenuTabOption("skins", SkinsTab.Tab));
            MenuTabOption.Add(new MenuTabOption("misc", MiscTab.Tab));
            MenuTabOption.Add(new MenuTabOption("more misc", MoreMiscTab.Tab));
	        MenuTabOption.Add(new MenuTabOption("info", InfoTab.Tab));
	        
            MenuTabOption.Add(new MenuTabOption("colors", ColorsTab.Tab));
	        MenuTabOption.Add(new MenuTabOption("hotkeys", HotkeyTab.Tab));
	        MenuTabOption.Add(new MenuTabOption("stats", StatsTab.Tab));
		}
    }
}
