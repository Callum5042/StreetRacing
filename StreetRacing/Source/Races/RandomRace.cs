using GTA;
using GTA.Math;
using StreetRacing.Source.Drivers;
using StreetRacing.Source.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetRacing.Source.Races
{
    public class RandomRace : IStreetRace
    {
        public IRacingDriver PlayerDriver { get; protected set; }

        public IList<IRacingDriver> Drivers { get; protected set; } = new List<IRacingDriver>();

        public RandomRace()
        {
            PlayerDriver = new PlayerRacingDriver();
            Drivers.Add(PlayerDriver);
            Drivers.Add(new NearbyRacingDriver());

            UI.Notify("RandomRace started");
        }

        public bool IsRacing { get; protected set; } = true;

        public void OnTick(object sender, EventArgs e)
        {
            if (IsRacing)
            {
                CalculateDriversPosition();
                UpdateBlipNumber();
                SetTask();

                CheckCarState();

                var distance = Drivers.First().Distance(Drivers.Last());
                if (distance > 200f)
                {
                    if (PlayerDriver.RacePosition == 1)
                    {
                        var money = 1000;
                        UI.Notify($"You win: {money}");
                        Game.Player.Money += money;
                    }
                    else
                    {
                        UI.Notify("You lose");
                        Game.Player.Money -= 1000;
                    }

                    Finish();
                }

                // Display
                UI.ShowSubtitle($"Position: {PlayerDriver.RacePosition} - Distance: {distance}");
            }
        }

        private void UpdateBlipNumber()
        {
            for (int i = 0; i < Drivers.Count; i++)
            {
                Drivers[i].Vehicle.CurrentBlip.ShowNumber(i + 1);
            }
        }

        private void CheckCarState()
        {
            foreach (var driver in Drivers.Where(x => !x.IsPlayer))
            {
                if (driver.Vehicle.IsDead || driver.Driver.IsDead || driver.Driver.CurrentVehicle != driver.Vehicle)
                {
                    string message = "";
                    if (!driver.Vehicle.IsDriveable)
                    {
                        message = "Vehicle is undriveable";
                    }
                    else if (driver.Driver.IsDead)
                    {
                        message = "Driver has died";
                    }
                    else if (driver.Driver.CurrentVehicle != driver.Vehicle)
                    {
                        message = "Driver has left the vehicle";
                    }

                    UI.Notify($"{driver.Vehicle.FriendlyName} has been disqualified - {message}");
                }
            }
        }

        private void SetTask()
        {
            foreach (var driver in Drivers.Where(x => !x.IsPlayer))
            {
                if (driver.RacePosition == 1)
                {
                    driver.SetTask(new DriverTaskCruise());
                }
                else
                {
                    driver.SetTask(new DriverTaskChase(Drivers.First()));
                }
            }
        }

        protected void CalculateDriversPosition()
        {
            foreach (var driver in Drivers)
            {
                driver.RacePosition = Drivers.Count;
                foreach (var otherDriver in Drivers)
                {
                    if (driver != otherDriver)
                    {
                        var heading = driver.Vehicle.Position - otherDriver.Vehicle.Position;
                        var dot = Vector3.Dot(heading.Normalized, otherDriver.Vehicle.Driver.ForwardVector.Normalized);

                        if (dot > 0)
                        {
                            driver.RacePosition--;
                        }
                    }
                }
            }

            Drivers = Drivers.OrderBy(x => x.RacePosition).ToList();
        }

        public void Finish()
        {
            IsRacing = false;
            foreach (var driver in Drivers.Where(x => !x.IsPlayer))
            {
                driver.Vehicle.CurrentBlip.Remove();
                driver.SetTask(new DriverTaskCruise());
            }
        }
    }
}