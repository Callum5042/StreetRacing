using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetRacing.Source.Racers
{
    public class SpawnRacingDriver : RacingDriver
    {
        public SpawnRacingDriver(IConfiguration configuration, Vector3 spawnPosition) : base(configuration)
        {
            Vehicle = World.CreateVehicle(SpawnRandomVehicle(), spawnPosition, Game.Player.Character.Heading);
            ConfigureDefault();
        }

        public override void Lost()
        {
            base.Lost();
            Vehicle.Driver.Delete();
            Vehicle.Delete();
        }

        private VehicleHash SpawnRandomVehicle()
        {
            var vehicles = new Dictionary<string, IList<VehicleHash>>();
            vehicles.Add("Super", new List<VehicleHash>()
            {
                VehicleHash.Pfister811,
                VehicleHash.Adder
            });

            if (configuration.VehicleType == "All")
            {
                var allVehicles = vehicles.SelectMany(x => x.Value).ToList();

                var random = new Random();
                var vehicleIndex = random.Next(vehicles.Count);
                return allVehicles[vehicleIndex];
            }
            else
            {
                var random = new Random();
                var vehicleIndex = random.Next(vehicles[configuration.VehicleType].Count);
                return vehicles[configuration.VehicleType][vehicleIndex];
            }
        }
    }
}