using GTA;
using GTA.Math;
using GTA.Native;
using StreetRacing.Source.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetRacing.Source.Races
{
    public abstract class Race : IRace
    {
        public IRacingDriver PlayerDriver { get; } = new PlayerRacingDriver();

        public abstract bool IsRacing { get; protected set; }

        public abstract void Tick();

        protected readonly float overtakeDistance = 40f;

        protected IList<IRacingDriver> Drivers = new List<IRacingDriver>();

        public VehicleHash SpawnRandomVehicle()
        {
            var vehicles = new List<VehicleHash>()
            {
                VehicleHash.Comet2,
                VehicleHash.Comet3,
                VehicleHash.Comet4,
                VehicleHash.Elegy,
                VehicleHash.Elegy2,
                VehicleHash.Feltzer2,
                VehicleHash.Feltzer3,
                VehicleHash.Schwarzer,
                VehicleHash.Banshee,
                VehicleHash.Banshee2,
                VehicleHash.Buffalo,
                VehicleHash.Buffalo2,
                VehicleHash.Buffalo3,
                VehicleHash.Massacro,
                VehicleHash.Massacro2,
                VehicleHash.Jester,
                VehicleHash.Jester2,
                VehicleHash.Jester3,
                VehicleHash.Omnis,
                VehicleHash.Lynx,
                VehicleHash.Tropos,
                VehicleHash.FlashGT
            };

            var random = new Random();
            var vehicleIndex = random.Next(vehicles.Count);
            return vehicles[vehicleIndex];
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

        protected float GetDistance(IRacingDriver vehicle)
        {
            return Game.Player.Character.Position.DistanceTo(vehicle.Driver.Position);
        }
    }
}