using GTA;
using StreetRacing.Source.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetRacing.Source.Races
{
    public abstract class RaceBase : IRace
    {
        private readonly IConfiguration configuration;
        private readonly DateTime startTime = DateTime.Now;
        protected TimeSpan time;

        public RaceBase(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public bool IsRacing { get; protected set; } = true;

        public IList<IDriver> Drivers { get; protected set; } = new List<IDriver>();

        public virtual void Dispose()
        {
            foreach (var driver in Drivers.Where(x => !x.IsPlayer))
            {
                driver.Dispose();
            }
        }

        public virtual void Tick()
        {
            time = startTime.Subtract(DateTime.Now);

            CalculatePositions();
            UpdateBlips();
            ComputerAI();
            DeployPolice();
            CheckVehicleStates();
        }

        private void CheckVehicleStates()
        {
            foreach (var driver in Drivers)
            {
                driver.CheckVehicleState();
            }
        }

        private void UpdateBlips()
        {
            foreach (var driver in Drivers.Where(x => !x.IsPlayer))
            {
                driver.UpdateBlip();
            }
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

        private void DeployPolice()
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

        protected virtual void CalculatePositions()
        {

        }

        protected virtual void ComputerAI()
        {

        }
    }
}