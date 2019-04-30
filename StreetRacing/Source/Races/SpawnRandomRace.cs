using GTA;
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
                        }
                    }
                    else
                    {
                        var distance = driver.Distance(Drivers.First());
                    }
                }

                if (driver.RacePosition != 1)
                {
                    if (driver.Distance(Drivers.First()) > configuration.WinDistance)
                    {
                        driver.Lost();
                        Drivers.Remove(driver);

                        UI.Notify($"{driver.ToString()} lose");

                        if (driver.IsPlayer)
                        {
                            IsRacing = false;
                        }
                    }
                }

                if (Drivers.Count == 1 || !IsRacing)
                {
                    IsRacing = false;
                    if (Drivers.FirstOrDefault().IsPlayer)
                    {
                        UI.Notify($"You win");
                        Game.Player.Money += configuration.Money;
                    }
                    else
                    {
                        UI.Notify($"You lose");
                        Game.Player.Money -= configuration.Money;
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