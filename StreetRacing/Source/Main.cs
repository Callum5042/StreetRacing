using GTA;
using GTA.Math;
using StreetRacing.Source.Interface;
using StreetRacing.Source.Races;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace StreetRacing.Source
{
    public class Main : Script
    {
        private readonly ConfigurationMenu configMenu = new ConfigurationMenu();
        private IStreetRace race;

        public IList<(Vector3 position, Blip blip)> Checkpoints { get; protected set; } = new List<(Vector3, Blip)>();

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
            UI.Notify($"{Name} has loaded: {version.Major}.{version.Minor}.{version.Build}");
        }

        private void OnTick(object sender, EventArgs e)
        {
            // Unload
            if (race?.IsRacing == false)
            {
                Tick -= race.OnTick;
            }

            // Trace checkpoints
            //if (trace)
            //{
            //    Wait(1000);

            //    var currentPosition = Game.Player.Character.Position;
            //    if ((!Checkpoints.Any()) || (Checkpoints.Last().position.DistanceTo(currentPosition) > 50f))
            //    {
            //        var blip = World.CreateBlip(currentPosition);
            //        blip.ShowNumber(Checkpoints.Count + 1);

            //        Checkpoints.Add((currentPosition, blip));
            //    }
            //}
        }

        private bool trace = false;

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.O)
            {
                if (!trace)
                {
                    trace = true;
                    UI.Notify("Start Trace");
                }
                else
                {
                    trace = false;
                    UI.Notify("End Trace");

                    foreach (var checkpoint in Checkpoints)
                    {
                        checkpoint.blip.Remove();
                    }
                }
            }

            if (configMenu.Active)
            {
                if (e.KeyCode == Keys.T)
                {
                    race = new SpawnRandomRace(configMenu);
                    Tick += race.OnTick;
                }

                if (e.KeyCode == Keys.E)
                {
                    race = new RandomRace(configMenu);
                    Tick += race.OnTick;
                }
            }
        }
    }
}