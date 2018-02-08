using SDG.Provider;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Managers.Main;
using Thanking.Options;
using Thanking.Options.UIVariables;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Components.UI.Menu
{
    [SpyComponent]
    [Component]
    public class MenuComponent : MonoBehaviour
    {
        public static float version = 2.0f;
        
        public static Font _TabFont;
        public static Font _TextFont;
        public static Texture2D _LogoTexLarge;

        public static bool IsInMenu;
        public static KeyCode MenuKey = KeyCode.F1;

        public static Rect MenuRect = new Rect(29, 29, 640, 480);

        public static Color32 _OutlineBorderBlack;
        public static Color32 _OutlineBorderLightGray;
        public static Color32 _OutlineBorderDarkGray;
        public static Color32 _FillLightBlack;
        public static Color32 _Accent1;
        public static Color32 _Accent2;

        private Rect _cursor = new Rect(0, 0, 20f, 20f);
        private Texture _cursorTexture;


        // Use this for initialization
        void Start()
        {

            ColorUtilities.addColor(new ColorVariable("_OutlineBorderBlack", "Menu - Black Outline", new Color32(3, 3, 3, 255)));
            ColorUtilities.addColor(new ColorVariable("_OutlineBorderLightGray", "Menu - Light Gray Outline", new Color32(75, 75, 75, 255)));
            ColorUtilities.addColor(new ColorVariable("_OutlineBorderDarkGray", "Menu - Dark Gray Outline", new Color32(55, 55, 55, 255)));
            ColorUtilities.addColor(new ColorVariable("_FillLightBlack", "Menu - Light Black Filler", new Color32(30, 30, 30, 255)));
            ColorUtilities.addColor(new ColorVariable("_Accent1", "Menu - Accent 1", new Color32(244, 155, 66, 255)));
            ColorUtilities.addColor(new ColorVariable("_Accent2", "Menu - Accent 2", new Color32(160, 107, 54, 255)));

            UpdateColors();

            MenuTabs.AddTabs();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(MenuKey))
            {
                IsInMenu = !IsInMenu;
                
                if (IsInMenu)
                    SectionTab.CurrentSectionTab = null;
            }
        }

        void OnGUI()
        {
            if (IsInMenu && _LogoTexLarge != null)
            {
                if (PlayerUI.window != null)
                    PlayerUI.window.showCursor = true;

                try
                {
                    if (_cursorTexture == null)
                        _cursorTexture = Resources.Load("UI/Cursor") as Texture;

                    PlayerUI.window.showCursor = true;
                    _cursor.x = Input.mousePosition.x;
                    _cursor.y = Screen.height - Input.mousePosition.y;
                    GUI.depth = 0;
                    GUI.DrawTexture(_cursor, _cursorTexture);
                }
                catch { }
                MenuRect = GUI.Window(0, MenuRect, DoMenu, "Ironic");
            }
        }


        #region Backend
        void DoMenu(int id)
        {
            if (SectionTab.CurrentSectionTab == null)
            {
                DoBorder();
                DoTabs();
                DrawTabs();
                DoConfigButtons();
            }
            else
            {
                DoSectionTab();
            }
            GUI.DragWindow(new Rect(0, 0, MenuRect.width, 25));
        }

        void DoBorder()
        {
            Rect BlackOutline = new Rect(0, 0, MenuRect.width, MenuRect.height);
            Rect LGrayOutline_1 = MenuUtilities.Inline(BlackOutline);
            Rect DGrayOutline = MenuUtilities.Inline(LGrayOutline_1);
            Rect LGrayOutline_2 = MenuUtilities.Inline(DGrayOutline, 3);
            Rect Fill = MenuUtilities.Inline(LGrayOutline_2);
            Rect TopLine1 = new Rect(Fill.x + 2, Fill.y + 2, Fill.width - 4, 2);
            Rect TopLine2 = new Rect(Fill.x + 2, Fill.y + 4, Fill.width - 4, 2);

            Drawing.DrawRect(BlackOutline, _OutlineBorderBlack);
            Drawing.DrawRect(LGrayOutline_1, _OutlineBorderLightGray);
            Drawing.DrawRect(DGrayOutline, _OutlineBorderDarkGray);
            Drawing.DrawRect(LGrayOutline_2, _OutlineBorderLightGray);
            Drawing.DrawRect(Fill, _FillLightBlack);
            Drawing.DrawRect(TopLine1, _Accent1);
            Drawing.DrawRect(TopLine2, _Accent2);
        }

        void DoTabs()
        {
            GUILayout.BeginArea(new Rect(15, 25, 130, 325));
            GUILayout.BeginVertical();
            for (int i = 0; i < MenuTabOption.tabs.Count; i++)
            {
                if (Prefab.MenuTab(MenuTabOption.tabs[i].name, ref MenuTabOption.tabs[i].enabled))
                {
                    MenuTabOption.CurrentTab = MenuTabOption.tabs[i].enabled ? MenuTabOption.tabs[i] : null;
                }
                GUILayout.Space(-11);

                if (MenuTabOption.tabs[i] != MenuTabOption.CurrentTab)
                    MenuTabOption.tabs[i].enabled = false;
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        void DrawTabs()
        {
            GUILayout.BeginArea(new Rect(160, 25, 466, 436));
            if (MenuTabOption.CurrentTab != null)
            {
                MenuTabOption.CurrentTab.tab();
            }
            else
            {
                LogoTab();
            }
            GUILayout.EndArea();
        }

        void DoSectionTab()
        {
            if (SectionTab.CurrentSectionTab != null)
            {
                DoBorder();

                Prefab.MenuArea(new Rect(10, 20, MenuRect.width - 10 * 2, MenuRect.height - 20 - 10), SectionTab.CurrentSectionTab.name.ToUpper(), SectionTab.CurrentSectionTab.code);
                bool temp = false;
                if (Prefab.MenuTabAbsolute(new Vector2(17, 428), "back", ref temp))
                {
                    SectionTab.CurrentSectionTab = null;
                }
            }
        }

        void DoConfigButtons()
        {
            Prefab.MenuArea(new Rect(18, 370, 125, 91), "CONFIG", () =>
            {
                GUILayout.Space(5);
                if (Prefab.Button("Save", 90))
                {
                    ConfigManager.SaveConfig(ConfigManager.CollectConfig());
                }
                GUILayout.Space(5);
                if (Prefab.Button("Load", 90))
                {
                    ConfigManager.Init();
                    SetGUIColors();
                    SkinsUtilities.ApplyFromConfig();
                }
            });
        }
        #endregion

        static void LogoTab()
        {
            Prefab.MenuArea(new Rect(0, 0, 466, 436), ":thanking: v" + version, () =>
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(_LogoTexLarge, GUILayout.Height(436), GUILayout.Width(436));
                GUILayout.Space(10);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            });
        }

        public static void UpdateColors()
        {
            _OutlineBorderBlack = ColorUtilities.getColor("_OutlineBorderBlack");
            _OutlineBorderLightGray = ColorUtilities.getColor("_OutlineBorderLightGray");
            _OutlineBorderDarkGray = ColorUtilities.getColor("_OutlineBorderDarkGray");
            _FillLightBlack = ColorUtilities.getColor("_FillLightBlack");
            _Accent1 = ColorUtilities.getColor("_Accent1");
            _Accent2 = ColorUtilities.getColor("_Accent2");
        }

        public static void SetGUIColors()
        {
            UpdateColors();
            Prefab.UpdateColors();
        }
    }
}
