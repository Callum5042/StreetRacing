using GTA;
using GTA.Math;
using GTA.Native;
using NativeUI;
using StreetRacing.Source;
using StreetRacing.Source.Races;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StreetRacing
{
    public class Main : Script
    {
        private bool isActive = true;

        private IRace race;

        private MenuPool menuPool;

        public Main()
        {
            Tick += OnTick;
            KeyUp += OnKeyUp;
            KeyDown += OnKeyDown;
            
            UI.Notify("StreetRacing Loaded");

            menuPool = new MenuPool();
            var mainMenu = new UIMenu("Street Racing", "Options");
            menuPool.Add(mainMenu);
            AddMenuActive(mainMenu);
            AddMenuFoods(mainMenu);
            AddMenuCook(mainMenu);
            AddMenuAnotherMenu(mainMenu);
            menuPool.RefreshIndex();
            
            KeyDown += (o, e) =>
            {
                if (e.KeyCode == Keys.F8 && !menuPool.IsAnyMenuOpen()) // Our menu on/off switch
                    mainMenu.Visible = !mainMenu.Visible;
            };
        }

        private void OnTick(object sender, EventArgs e)
        {
            menuPool.ProcessMenus();

            if (isActive)
            {
                if (race != null && race.IsRacing)
                {
                    race.Tick();
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (isActive)
            {
                switch (e.KeyCode)
                {
                    case Keys.E:
                        if (Game.IsWaypointActive)
                        {
                            race = new SprintRace();
                        }
                        else
                        {
                            race = new DistanceRace();
                        }
                        break;
                }
            }
        }

        private bool ketchup = false;
        private string dish = "Banana";
        

        public void AddMenuActive(UIMenu menu)
        {
            var newitem = new UIMenuCheckboxItem("Active", isActive);
            menu.AddItem(newitem);
            menu.OnCheckboxChange += (sender, item, @checked) =>
            {
                if (item == newitem)
                {
                    isActive = @checked;
                }
            };
        }

        public void AddMenuFoods(UIMenu menu)
        {
            var foods = new List<dynamic>
        {
            "Banana",
            "Apple",
            "Pizza",
            "Quartilicious",
            0xF00D, // Dynamic!
        };
            var newitem = new UIMenuListItem("Food", foods, 0);
            menu.AddItem(newitem);
            menu.OnListChange += (sender, item, index) =>
            {
                if (item == newitem)
                {
                    dish = item.IndexToItem(index).ToString();
                    UI.Notify("Preparing ~b~" + dish + "~w~...");
                }

            };
        }

        public void AddMenuCook(UIMenu menu)
        {
            var newitem = new UIMenuItem("Cook!", "Cook the dish with the appropiate ingredients and ketchup.");
            newitem.SetLeftBadge(UIMenuItem.BadgeStyle.Star);
            newitem.SetRightBadge(UIMenuItem.BadgeStyle.Tick);
            menu.AddItem(newitem);
            menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == newitem)
                {
                    string output = ketchup ? "You have ordered ~b~{0}~w~ ~r~with~w~ ketchup." : "You have ordered ~b~{0}~w~ ~r~without~w~ ketchup.";
                    UI.ShowSubtitle(String.Format(output, dish));
                }
            };
            menu.OnIndexChange += (sender, index) =>
            {
                if (sender.MenuItems[index] == newitem)
                    newitem.SetLeftBadge(UIMenuItem.BadgeStyle.None);
            };
        }

        public void AddMenuAnotherMenu(UIMenu menu)
        {
            var submenu = menuPool.AddSubMenu(menu, "Another Menu");
            for (int i = 0; i < 20; i++)
                submenu.AddItem(new UIMenuItem("PageFiller", "Sample description that takes more than one line. Moreso, it takes way more than two lines since it's so long. Wow, check out this length!"));
        }
    }
}