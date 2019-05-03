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

        public static IStreetRace Race { get; protected set; }
        
        public Main()
        {
            Tick += OnTick;
            KeyUp += OnKeyUp;
            Aborted += (o, e) =>
            {
                Race.Finish();
                UI.Notify("StreetRacing has aborted");
            };

            Tick += configMenu.OnTick;
            KeyUp += configMenu.OnKeyUp;

            Start();
        }

        private void Start()
        {
            var assembly = Assembly.GetExecutingAssembly().GetName();
            UI.Notify($"{assembly.Name} has loaded: v{assembly.Version.Major}.{assembly.Version.Minor}.{assembly.Version.Build}");
        }

        private void OnTick(object sender, EventArgs e)
        {
            // Unload
            if (Race?.IsRacing == false)
            {
                Race.Finish();
                Tick -= Race.OnTick;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (configMenu.Active)
            {
                try
                {
                    if (e.KeyCode == configMenu.StartSpawnKey)
                    {
                        Race = new SpawnDistanceRace(configMenu);
                        Tick += Race.OnTick;
                    }

                    if (e.KeyCode == configMenu.StartNearbyKey)
                    {
                        Race = new NearbyDistanceRace(configMenu);
                        Tick += Race.OnTick;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    UI.Notify(ex.Message);
                }
            }
        }
    }
}