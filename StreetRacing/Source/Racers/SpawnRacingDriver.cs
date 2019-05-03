using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;

namespace StreetRacing.Source.Racers
{
    public class SpawnRacingDriver : RacingDriver
    {
        public SpawnRacingDriver(IConfiguration configuration, Vector3 spawnPosition) : base(configuration)
        {
            Vehicle = World.CreateVehicle(SpawnRandomVehicle(), spawnPosition, Game.Player.Character.Heading);
            ConfigureDefault();
        }

        private VehicleHash SpawnRandomVehicle()
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
                VehicleHash.FlashGT,
                VehicleHash.GT500,
                VehicleHash.ItaliGTB,
                VehicleHash.ItaliGTB2,
                VehicleHash.ItaliGTO,
                VehicleHash.SultanRS,
                VehicleHash.Kamacho,
                VehicleHash.Kuruma,
                VehicleHash.Kuruma2,
                VehicleHash.Neon,
                VehicleHash.Ruston,
                VehicleHash.Schlagen,
                VehicleHash.Schafter2,
                VehicleHash.Schafter3,
                VehicleHash.Schafter4,
                VehicleHash.Schafter5,
                VehicleHash.Specter,
                VehicleHash.Specter2,
                VehicleHash.Surano,
                VehicleHash.Radi
            };

            var random = new Random();
            var vehicleIndex = random.Next(vehicles.Count);
            return vehicles[vehicleIndex];
        }
    }
}