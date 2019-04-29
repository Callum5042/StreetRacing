using GTA;
using GTA.Math;
using GTA.Native;
using StreetRacing.Source.Racers;
using StreetRacing.Source.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetRacing.Source.Races
{
    public class SpawnRandomRace : StreetRace
    {
        private readonly IConfiguration configuration;

        public SpawnRandomRace(IConfiguration configuration)
        {
            this.configuration = configuration;

            Drivers.Add(PlayerDriver);
            for (int i = 1; i <= configuration.SpawnCount; i++)
            {
                var position = Game.Player.Character.Position + (Game.Player.Character.ForwardVector * (6.0f * i));
                Drivers.Add(new SpawnRacingDriver(configuration, SpawnRandomVehicle(), position));
            }

            Drivers.Last().SetTask(new DriverTaskCruise());
            CalculateDriversPosition();

            UI.Notify($"SpawnRandomRace started - {configuration.SpawnCount} spawned");
        }

        protected override void Other()
        {
            foreach (var driver in Drivers.ToList())
            {
                if (driver.IsPlayer)
                {
                    if (driver.RacePosition == 1)
                    {
                        if (Drivers.Count > 1)
                        {
                            var distance = driver.Distance(Drivers.ElementAtOrDefault(1));
                            UI.ShowSubtitle($"Position: {PlayerDriver.RacePosition} - Distance: {distance}");
                        }
                    }
                    else
                    {
                        var distance = driver.Distance(Drivers.First());
                        UI.ShowSubtitle($"Position: {PlayerDriver.RacePosition} - Distance: {distance}");
                    }
                }

                if (driver.RacePosition != 1)
                {
                    if (driver.Distance(Drivers.First()) > configuration.WinDistance)
                    {
                        driver.Lost();
                        Drivers.Remove(driver);

                        UI.Notify($"{driver.ToString()} lose");
                    }
                }
            }
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
                VehicleHash.FlashGT
            };

            var random = new Random();
            var vehicleIndex = random.Next(vehicles.Count);
            return vehicles[vehicleIndex];
        }
    }
}