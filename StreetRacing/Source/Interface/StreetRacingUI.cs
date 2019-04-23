using GTA;
using NativeUI;
using System;
using System.Windows.Forms;

namespace StreetRacing.Source.Interface
{
    public class StreetRacingUI
    {
        protected readonly MenuPool menuPool = new MenuPool();
        protected UIMenu mainMenu;

        public StreetRacingUI(Script script)
        {
            script.Tick += OnTick;
            script.KeyUp += OnKeyUp;

            mainMenu = new UIMenu("Street Racing", "Configuration");
            menuPool.Add(mainMenu);
            
            mainMenu.AddCheckbox(Active, nameof(Active));
            mainMenu.AddCheckbox(Spawn, nameof(Spawn), "Spawn a vehicle to start a race with");
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

        public bool Active { get; protected set; } = true;

        public bool Spawn { get; protected set; } = true;
    }
}