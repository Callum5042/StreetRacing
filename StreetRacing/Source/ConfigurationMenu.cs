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
            mainMenu = new UIMenu("Street Racing", "Configuration");
            menuPool.Add(mainMenu);

            AddMenuActive(mainMenu);
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
            menu.OnCheckboxChange += (sender, item, checked_) =>
            {
                if (item == newitem)
                {
                    Active = checked_;
                }
            };
        }
    }
}