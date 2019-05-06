using GTA;
using NativeUI;
using System;
using System.Collections.Generic;
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
            AddMenuSpawnCount(mainMenu);
            AddMenuSpawnVehicleTypes(mainMenu);
            AddMenuPolicePursuit(mainMenu);

            // Add save config
            AddSaveConfig(mainMenu);
            
            menuPool.RefreshIndex();
        }

        private void AddMenuPolicePursuit(UIMenu menu)
        {
            var newitem = new UIMenuCheckboxItem("Police Pursit", PolicePursuit);
            menu.AddItem(newitem);
            menu.OnCheckboxChange += (sender, item, @checked) =>
            {
                if (item == newitem)
                {
                    PolicePursuit = @checked;
                }
            };
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

        private void AddMenuActive(UIMenu menu)
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

        private void AddMenuSetMax(UIMenu menu)
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

        private void AddSaveConfig(UIMenu menu)
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

        private void AddMenuSpawnVehicleTypes(UIMenu menu)
        {
            var vehicleTypes = new List<dynamic>
            {
                "All",
                "Sports",
                "Super",
                "Off-road"
            };

            var newitem = new UIMenuListItem("Vehicle Type", vehicleTypes, VehicleType.IndexOf(VehicleType));
            menu.AddItem(newitem);
            menu.OnListChange += (sender, item, index) =>
            {
                if (item == newitem)
                {
                    VehicleType = item.IndexToItem(index) as string;
                }
            };
        }
    }
}