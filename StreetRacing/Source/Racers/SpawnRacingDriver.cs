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
            var vehicles = new List<VehicleHash>()
            {
                VehicleHash.Pfister811,
                VehicleHash.Adder,
                VehicleHash.Dominator,
                VehicleHash.Dominator2,
                VehicleHash.Dominator3,
                VehicleHash.Dominator4,
                VehicleHash.Dominator5,
                VehicleHash.HotringSabre,
                VehicleHash.SabreGT,
                VehicleHash.SabreGT2,
                VehicleHash.Tyrant,
                VehicleHash.Deveste,
                VehicleHash.EntityXF,
                VehicleHash.EntityXXR,
                VehicleHash.Cyclone,
                VehicleHash.ItaliGTB,
                VehicleHash.ItaliGTB2,
                VehicleHash.ItaliGTO,
                VehicleHash.Nero,
                VehicleHash.Nero2,
                VehicleHash.Tyrus,
                VehicleHash.Pfister811,
                VehicleHash.Banshee,
                VehicleHash.Banshee2,
                VehicleHash.Reaper,
                VehicleHash.SultanRS,
                VehicleHash.Sultan,
                VehicleHash.FlashGT,
                VehicleHash.Neon,
                VehicleHash.Nero,
                VehicleHash.Nero2,
                VehicleHash.Comet2,
                VehicleHash.Comet3,
                VehicleHash.Comet4,
                VehicleHash.Comet5,
                VehicleHash.Elegy,
                VehicleHash.Elegy2,
                VehicleHash.Specter,
                VehicleHash.Specter2,
                VehicleHash.Seven70,
                VehicleHash.Lynx,
                VehicleHash.Omnis
            };

            var random = new Random();
            var vehicleIndex = random.Next(vehicles.Count);
            return vehicles[vehicleIndex];
        }
    }
}