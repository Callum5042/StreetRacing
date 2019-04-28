using GTA;
using StreetRacing.Source.Interface;
using StreetRacing.Source.Races;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace StreetRacing.Source
{
    public class Main : Script
    {
        private readonly ConfigurationMenu configMenu = new ConfigurationMenu();
        private IStreetRace race;
        
        public Main()
        {
            Tick += OnTick;
            KeyUp += OnKeyUp;
            Aborted += (o, e) =>
            {
                race.Finish();
                UI.Notify("StreetRacing has aborted");
            };

            Tick += configMenu.OnTick;
            KeyUp += configMenu.OnKeyUp;

            Start();
        }

        private void Start()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            UI.Notify($"{Name} has loaded: v{version.Major}.{version.Minor}.{version.Build}");
        }

        private void OnTick(object sender, EventArgs e)
        {
            // Unload
            if (race?.IsRacing == false)
            {
                race.Finish();
                Tick -= race.OnTick;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (configMenu.Active)
            {
                if (e.KeyCode == configMenu.StartSpawnKey)
                {
                    race = new SpawnRandomRace(configMenu);
                    Tick += race.OnTick;
                }

                if (e.KeyCode == configMenu.StartNearbyKey)
                {
                    race = new RandomRace(configMenu);
                    Tick += race.OnTick;
                }
            }
        }
    }
}