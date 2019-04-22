using GTA;
using GTA.Native;
using StreetRacing.Source.Interface;
using StreetRacing.Source.Races;
using System;
using System.Windows.Forms;

namespace StreetRacing
{
    public class Main : Script
    {
        private readonly StreetRacingUI streetRacingUI;
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
            
            streetRacingUI = new StreetRacingUI(this);

            UI.Notify($"{Name} has loaded");
        }

        private void OnTick(object sender, EventArgs e)
        {
            // Unload
            if (race?.IsRacing == false)
            {
                Tick -= race.OnTick;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            // only for testing
            if (e.KeyCode == Keys.T)
            {
                var position = Game.Player.Character.Position + (Game.Player.Character.ForwardVector * (12.0f * 1));
                var vehicle = World.CreateVehicle(VehicleHash.ItaliGTO, position, Game.Player.Character.Heading);
                vehicle.PlaceOnGround();

                vehicle.CreateRandomPedOnSeat(VehicleSeat.Driver);
            }

            if (e.KeyCode == Keys.E)
            {
                StartRace<RandomRace>();
            }
        }

        private void StartRace<TRace>() where TRace : IStreetRace, new()
        {
            race = new TRace();
            Tick += race.OnTick;
        }
    }
}