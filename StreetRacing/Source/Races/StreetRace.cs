using GTA;
using GTA.Math;
using StreetRacing.Source.Racers;
using StreetRacing.Source.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

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
                CalculateDriversPosition();
                UpdateBlipNumber();
                SetTask();

                CheckCarState();
                Other();
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
    }
}