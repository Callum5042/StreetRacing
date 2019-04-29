using GTA;
using GTA.Math;
using StreetRacing.Source.Racers;
using StreetRacing.Source.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreetRacing.Source.Races
{
    public class StreetRace : IStreetRace
    {
        public IRacingDriver PlayerDriver { get; protected set; } = new PlayerRacingDriver();

        public IList<IRacingDriver> Drivers { get; protected set; } = new List<IRacingDriver>();

        public bool IsRacing { get; protected set; } = true;

        public virtual void Finish()
        {
            IsRacing = false;
            foreach (var driver in Drivers.Where(x => !x.IsPlayer))
            {
                driver.Vehicle.CurrentBlip.Remove();
                driver.Driver.Delete();
                driver.Vehicle.Delete();
            }
        }

        public virtual void OnTick(object sender, EventArgs e)
        {
            if (IsRacing)
            {
                //CalculateDriversPosition();
                CalculateDriversPosition();
                UpdateBlipNumber();
                SetTask();

                CheckCarState();
                // Other();

                UI.ShowSubtitle($"Position: {Drivers.FirstOrDefault(x => x.IsPlayer).RacePosition}");
            }
        }

        protected virtual void Other()
        {
            
        }

        protected void UpdateBlipNumber()
        {
            for (int i = 0; i < Drivers.Count; i++)
            {
                Drivers[i].Vehicle.CurrentBlip.ShowNumber(i + 1);
            }
        }

        protected void SetTask()
        {
            foreach (var driver in Drivers.Where(x => !x.IsPlayer))
            {
                if (driver.RacePosition == 1)
                {
                    driver.SetTask(new DriverTaskCruise());
                }
                else
                {
                    if (driver.Distance(Drivers.First()) < 20f)
                    {
                        driver.SetTask(new DriverTaskCruise());
                    }
                    else
                    {
                        driver.SetTask(new DriverTaskChase(Drivers.First()));
                    }
                }
            }
        }

        protected void CheckCarState()
        {
            foreach (var driver in Drivers.Where(x => !x.IsPlayer).ToList())
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
                    driver.Lost();
                    Drivers.Remove(driver);
                }
            }
        }

        protected void CalculateDriversPosition()
        {
            foreach (var driver in Drivers.Where(x => x.RacePosition != 1))
            {
                driver.RacePosition = Drivers.Count;
                foreach (var otherDriver in Drivers.Where(x => x.RacePosition != 1))
                {
                    if (driver != otherDriver)
                    {
                        if (driver.IsInFront(otherDriver))
                        {
                            driver.RacePosition--;
                        }
                    }
                }
            }

            // Special distance checking for first place
            var first = Drivers.FirstOrDefault(x => x.RacePosition == 1);
            foreach (var driver in Drivers.Where(x => x.RacePosition != 1 && x.Distance(first) < 20f))
            {
                if (driver.IsInFront(first))
                {
                    first.RacePosition = driver.RacePosition;
                    driver.RacePosition = 1;
                }
            }

            Drivers = Drivers.OrderBy(x => x.RacePosition).ToList();
        }
    }
}