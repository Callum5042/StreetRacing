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
    public class DistanceRace : IRace
    {
        private readonly IConfiguration configuration;
        private readonly DistanceRaceGui distanceRaceGUI = new DistanceRaceGui();
        private readonly DateTime startTime = DateTime.Now;
        private TimeSpan time;

        public IList<IDriver> Drivers { get; protected set; } = new List<IDriver>();

        public DistanceRace(IConfiguration configuration)
        {
            this.configuration = configuration;

            LoadVehicles();
            CalculateStartPositions();
            UI.Notify($"Race Started: Distance");
        }

        public bool IsRacing { get; protected set; } = true;

        public void Dispose()
        {
            foreach (var driver in Drivers.Where(x => !x.IsPlayer))
            {
                driver.Dispose();
            }
        }

        public void Tick()
        {
            time = startTime.Subtract(DateTime.Now);

            CalculatePositions();
            ComputerAI();

            float distance = GetDistance();
            CheckPlayState(distance);

            DeployPolice();
            UpdateBlips();
            distanceRaceGUI.Draw(Drivers.FirstOrDefault(x => x.IsPlayer)?.RacePosition, time, distance / configuration.WinDistance);
        }

        private void CheckPlayState(float distance)
        {
            foreach (var driver in Drivers.ToList())
            {
                if (driver.RacePosition != 1)
                {
                    var first = Drivers.FirstOrDefault(x => x.RacePosition == 1);
                    if (first != null)
                    {
                        if (driver.DistanceTo(first.Position) > configuration.WinDistance)
                        {
                            UI.Notify($"{driver.ToString()} lose");

                            driver.Finish();
                            Drivers.Remove(driver);

                            if (driver.IsPlayer)
                            {
                                Finish();
                            }
                        }
                    }
                }

                if (Drivers.Count == 1 || !IsRacing)
                {
                    Finish();
                    if (Drivers.FirstOrDefault(x => x.RacePosition == 1)?.IsPlayer == true)
                    {
                        UI.Notify($"You win");
                        Game.Player.Money += configuration.Money;
                    }
                    else
                    {
                        UI.Notify($"You lose");
                        Game.Player.Money -= configuration.Money;
                    }
                }
            }
        }

        protected void DeployPolice()
        {
            if (configuration.PolicePursuit)
            {
                var vehicles = World.GetNearbyVehicles(Game.Player.Character.Position, 100f).Where(x => x.Driver.IsInPoliceVehicle);
                if (vehicles.Any(x => x.IsOnScreen))
                {
                    Game.Player.WantedLevel = 1;
                }
            }
        }

        private void UpdateBlips()
        {
            foreach (var driver in Drivers.Where(x => !x.IsPlayer))
            {
                driver.UpdateBlip();
            }
        }

        private float GetDistance()
        {
            var player = Drivers.FirstOrDefault(x => x.IsPlayer);
            float distance = 0;
            if (player != null)
            {
                if (player.RacePosition == 1)
                {
                    var secondPlace = Drivers.FirstOrDefault(x => x.RacePosition == 2);
                    if (secondPlace != null)
                    {
                        distance = player.DistanceTo(secondPlace.Position);
                    }
                }
                else
                {
                    distance = player.DistanceTo(Drivers.FirstOrDefault(x => x.RacePosition == 1).Position);
                }
            }

            return distance;
        }

        protected void CalculateStartPositions()
        {
            foreach (var driver in Drivers)
            {
                driver.RacePosition = Drivers.Count;
                foreach (var otherDriver in Drivers)
                {
                    if (driver != otherDriver)
                    {
                        if (driver.InFront(otherDriver))
                        {
                            driver.RacePosition--;
                        }
                    }
                }
            }
        }

        private void CalculatePositions()
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

        private void ComputerAI()
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
            IsRacing = false;
            var player = Drivers.FirstOrDefault(x => x.IsPlayer);
            BigMessageThread.MessageInstance.ShowRankupMessage("Finish", time.ToString(@"mm\:ss\:fff"), player.RacePosition);
            Dispose();
        }

        private void LoadVehicles()
        {
            Drivers.Add(new PlayerDriver(configuration));

            var closest = GetClosestVehicleToPlayer(radius: 20f);
            if (closest != null)
            {
                Drivers.FirstOrDefault(x => x.IsPlayer).RacePosition = 2;
                Drivers.Add(new ComputerDriver(configuration, closest) { RacePosition = 1 });
            }
            else
            {
                for (int i = 1; i <= configuration.SpawnCount; i++)
                {
                    var position = Game.Player.Character.Position + (Game.Player.Character.ForwardVector * (6.0f * i));
                    Drivers.Add(new ComputerDriver(configuration, position));
                }
            }
        }

        private Vehicle GetClosestVehicleToPlayer(float radius)
        {
            IList<(float distance, Vehicle vehicle)> vehicles = new List<(float distance, Vehicle vehicle)>();
            foreach (var vehicle in World.GetNearbyVehicles(Game.Player.Character.Position, radius).Where(x => !x.Driver.IsPlayer && x.IsAlive))
            {
                var distance = Vector3.Distance(Game.Player.Character.Position, vehicle.Position);
                vehicles.Add((distance, vehicle));
            }

            return vehicles.OrderBy(x => x.distance).FirstOrDefault().vehicle;
        }
    }
}