using GTA;
using NativeUI;
using System;
using System.Windows.Forms;

namespace StreetRacing.Source.Interface
{
    public class ConfigurationMenu : Configuration
    {
        protected readonly MenuPool menuPool = new MenuPool();
        protected UIMenu mainMenu;

        public ConfigurationMenu()
        {
            Load();

            mainMenu = new UIMenu("Street Racing", "Configuration");
            menuPool.Add(mainMenu);
            
            AddMenuActive(mainMenu);
            AddMenuSetMax(mainMenu);

            // Add save config
            AddSaveConfig(mainMenu);
            
            menuPool.RefreshIndex();
        }

        public void OnTick(object sender, EventArgs e)
        {
            menuPool.ProcessMenus();
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F8 && !menuPool.IsAnyMenuOpen())
            {
                mainMenu.Visible = !mainMenu.Visible;
            }
        }

        public void AddMenuActive(UIMenu menu)
        {
            var newitem = new UIMenuCheckboxItem("Active", Active);
            menu.AddItem(newitem);
            menu.OnCheckboxChange += (sender, item, @checked) =>
            {
                if (item == newitem)
                {
                    Active = @checked;
                }
            };
        }

        public void AddMenuSetMax(UIMenu menu)
        {
            var newitem = new UIMenuCheckboxItem("Set Max", MaxMods);
            menu.AddItem(newitem);
            menu.OnCheckboxChange += (sender, item, @checked) =>
            {
                if (item == newitem)
                {
                    MaxMods = @checked;
                }
            };
        }

        public void AddSaveConfig(UIMenu menu)
        {
            var newitem = new UIMenuItem("Save configuration");
            menu.AddItem(newitem);
            menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == newitem)
                {
                    UI.Notify("Saved config");
                    Save();
                }
            };
        }
    }
}