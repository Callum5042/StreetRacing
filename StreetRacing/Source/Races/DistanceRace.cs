using GTA;
using GTA.Math;
using NativeUI;
using StreetRacing.Source.Drivers;
using StreetRacing.Source.Gui;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetRacing.Source.Races
{
    public class DistanceRace : RaceBase
    {
        protected readonly IConfiguration configuration;
        private readonly DistanceRaceGui distanceRaceGUI = new DistanceRaceGui();
        
        public DistanceRace(IConfiguration configuration) : base(configuration)
        {
            this.configuration = configuration;

            LoadVehicles();
            CalculateStartPositions();
            UI.Notify($"Race Started: Distance");
        }

        public override void Tick()
        {
            base.Tick();
            CheckPlayState();

            try
            {
                distanceRaceGUI.Draw(Drivers.FirstOrDefault(x => x.IsPlayer)?.RacePosition, time, GetDistance() / configuration.WinDistance);
            }
            catch (DivideByZeroException)
            {
                distanceRaceGUI.Draw(Drivers.FirstOrDefault(x => x.IsPlayer)?.RacePosition, time, 1f);
            }
        }

        private void CheckPlayState()
        {
            var firstPlace = Drivers.FirstOrDefault(x => x.RacePosition == 1);
            foreach (var driver in Drivers.Where(x => x.RacePosition != 1 && x.InRace))
            {
                if (driver.DistanceTo(firstPlace.Position) > configuration.WinDistance)
                {
                    if (!driver.IsPlayer)
                    {
                        UI.Notify($"{driver.ToString()} lose");
                    }

                    driver.Finish();
                }
            }

            // If 1 driver left then finish the race
            if (Drivers.Count(x => x.InRace) == 1 || !Drivers.FirstOrDefault(x => x.IsPlayer).InRace)
            {
                Finish();
            }
        }

        private float GetDistance()
        {
            var player = Drivers.FirstOrDefault(x => x.IsPlayer);
            if (player != null)
            {
                if (player.RacePosition == 1)
                {
                    var secondPlace = Drivers.FirstOrDefault(x => x.RacePosition == 2);
                    if (secondPlace != null)
                    {
                        return player.DistanceTo(secondPlace.Position);
                    }
                }
                else
                {
                    return player.DistanceTo(Drivers.FirstOrDefault(x => x.RacePosition == 1).Position);
                }
            }

            return configuration.WinDistance;
        }

        protected override void CalculatePositions()
        {
            foreach (var driver in Drivers)
            {
                foreach (var otherRacer in Drivers.Where(x => x != driver && driver.DistanceTo(x.Position) < 50f))
                {
                    if (driver.InFront(otherRacer))
                    {
                        if (driver.RacePosition > otherRacer.RacePosition)
                        {
                            int tmp = driver.RacePosition;
                            driver.RacePosition = otherRacer.RacePosition;
                            otherRacer.RacePosition = tmp;
                        }
                    }
                }
            }
        }

        protected override void ComputerAI()
        {
            // Set first to cruise
            var firstPlace = Drivers.FirstOrDefault(x => x.RacePosition == 1);
            if (!firstPlace.IsPlayer)
            {
                if (firstPlace is ITask task)
                {
                    task.Cruise();
                }
            }

            // Set others to chase first
            foreach (var driver in Drivers.Where(x => !x.IsPlayer || x.RacePosition != 1))
            {
                if (driver is ITask task)
                {
                    if (driver.DistanceTo(firstPlace.Position) < 20f)
                    {
                        task.Cruise();
                    }
                    else
                    {
                        task.Chase(firstPlace);
                    }
                }
            }
        }

        private void Finish()
        {
            foreach (var driver in Drivers.Where(x => x.InRace))
            {
                driver.Finish();
            }

            IsRacing = false;
            var player = Drivers.FirstOrDefault(x => x.IsPlayer);
            BigMessageThread.MessageInstance.ShowRankupMessage("Finish", time.ToString(@"mm\:ss\:fff"), player.RacePosition);
        }

        protected virtual void LoadVehicles()
        {
            Drivers.Add(new PlayerDriver(configuration));
            for (int i = 1; i <= configuration.SpawnCount; i++)
            {
                var position = Game.Player.Character.Position + (Game.Player.Character.ForwardVector * (20.0f * i));
                Drivers.Add(new ComputerDriver(configuration, position));
            }
        }
    }
}