using GTA;
using GTA.Math;
using StreetRacing.Source.Tasks;
using StreetRacing.Source.Drivers;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;

namespace StreetRacing.Source.Races
{
    public class DistanceRace : Race
    {
        private readonly float loseDistance = 100f;

        public DistanceRace(bool spawn, int numberOfVehicles)
        {
            if (spawn)
            {
                SpawnVehicles(numberOfVehicles);
            }
            else
            {
                // Get closest vehicle
            }
        }

        private void SpawnVehicles(int numberOfVehicles)
        {
            for (int i = 1; i <= numberOfVehicles; ++i)
            {
                var position = Game.Player.Character.Position + (Game.Player.Character.ForwardVector * (6.0f * i));// + (Game.Player.Character.RightVector * 3.0f);
                Drivers.Add(new SpawnRacingDriver(SpawnRandomVehicle(), position));
            }
            
            Drivers.Add(PlayerDriver);
        }

        public override bool IsRacing { get; protected set; } = true;

        public override void Tick()
        {
            CalculateDriversPosition();
            SetDriversTask();
            UpdateDriversBlip();
            CheckIfDriverIsOutOfRace();
            CheckForWinner();

            // UI Test
            float distance = 0;
            if (Drivers.Count > 1)
            {
                distance = PlayerDriver.RacePosition == 1 ? PlayerDriver.Distance(Drivers.ElementAt(1)) : PlayerDriver.Distance(Drivers.First());
            }

            UI.ShowSubtitle($"Position: {PlayerDriver.RacePosition} - Distance: {distance} - First: {Drivers.First().Vehicle.DisplayName} - Count: {Drivers.Count}");
        }

        private void CheckForWinner()
        {
            if (Drivers.Count == 1)
            {
                if (Drivers.First().IsPlayer)
                {
                    FinishRace(win: true);
                }
                else
                {
                    FinishRace(win: false);
                }
            }
        }

        private void CheckIfDriverIsOutOfRace()
        {
            foreach (var driver in Drivers.ToList())
            {
                if (driver.RacePosition != 1)
                {
                    if (driver.Distance(Drivers.First()) > loseDistance)
                    {
                        UI.Notify($"{driver.Vehicle.DisplayName} has lost");
                        driver.Vehicle.Explode();
                        driver.Lost();
                        Drivers.Remove(driver);
                    }
                }
            }
        }

        private void FinishRace(bool win)
        {
            IsRacing = false;
            UI.ShowSubtitle($"Finish race: {win}");
            if (win)
            {
                UI.Notify("You have won");
            }
            else
            {
                UI.Notify("You have lost");
            }

            foreach (var driver in Drivers)
            {
                if (!driver.IsPlayer)
                {
                    driver.Lost();
                }
            }
        }

        private void UpdateDriversBlip()
        {
            foreach (var driver in Drivers)
            {
                if (!driver.IsPlayer)
                {
                    driver.Blip.Position = driver.Driver.Position;
                }
            }
        }

        private void SetDriversTask()
        {
            foreach (var driver in Drivers)
            {
                if (!driver.IsPlayer)
                {
                    if (driver.RacePosition == 1)
                    {
                        driver.SetTask(new DriverTaskCruise());
                    }
                    else
                    {
                        if (driver.Distance(Drivers.First()) > 20f)
                        {
                            driver.SetTask(new DriverTaskChase(Drivers.First().Driver));
                        }
                        else
                        {
                            driver.SetTask(new DriverTaskCruise());
                        }
                    }
                }
            }
        }

        private void SetTaskChase(IRacingDriver vehicle, Ped target)
        {
            var distanceToFirst = vehicle.Vehicle.Position.DistanceTo(target.Position);
            if (distanceToFirst < 20f)
            {
                vehicle.SetTask(new DriverTaskCruise());
            }
            else
            {
                vehicle.SetTask(new DriverTaskChase(target));
            }
        }
    }
}