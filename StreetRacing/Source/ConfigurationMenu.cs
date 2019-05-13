using GTA;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StreetRacing.Source
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

            AddCheckbox(nameof(Active), Active, x => Active = x);
            AddCheckbox("Police Pursit", PolicePursuit, x => PolicePursuit = x);

            AddButton("Save configuration", () => 
            {
                UI.Notify("Saved config");
                Save();
            });
            
            // Old
            AddMenuSpawnCount(mainMenu);
            AddMenuAnotherMenu(mainMenu);
            
            menuPool.RefreshIndex();
        }

        public void OnTick(object sender, EventArgs e)
        {
            menuPool.ProcessMenus();
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == MenuKey && !menuPool.IsAnyMenuOpen())
            {
                mainMenu.Visible = !mainMenu.Visible;
            }
        }

        private void AddButton(string name, Action action)
        {
            var newitem = new UIMenuItem(name);
            mainMenu.AddItem(newitem);
            mainMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == newitem)
                {
                    action?.Invoke();
                }
            };
        }

        public void AddMenuAnotherMenu(UIMenu menu)
        {
            var submenu = menuPool.AddSubMenu(menu, "Customize");
            //AddRecordTrackToggle(submenu);
        }

        //private void AddRecordTrackToggle(UIMenu menu)
        //{
        //    var newitem = new UIMenuCheckboxItem("Record Track", RecordTrack);
        //    menu.AddItem(newitem);
        //    menu.OnCheckboxChange += (sender, item, @checked) =>
        //    {
        //        if (item == newitem)
        //        {
        //            RecordTrack = @checked;
        //        }
        //    };
        //}

        private void AddCheckbox(string name, bool value, Func<bool, bool> func)
        {
            var newitem = new UIMenuCheckboxItem(name, value);
            mainMenu.AddItem(newitem);
            mainMenu.OnCheckboxChange += (sender, item, @checked) =>
            {
                if (item == newitem)
                {
                    func?.Invoke(@checked);
                }
            };
        }

        private void AddMenuSpawnCount(UIMenu menu)
        {
            var spawnCount = new List<dynamic>
            {
                1, 2, 3, 4, 5, 6, 7
            };

            var newitem = new UIMenuListItem("Spawn Count" , spawnCount, SpawnCount - 1);
            menu.AddItem(newitem);
            menu.OnListChange += (sender, item, index) =>
            {
                if (item == newitem)
                {
                    SpawnCount = (int)item.IndexToItem(index);
                }
            };
        }
    }
}