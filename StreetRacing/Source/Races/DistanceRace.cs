using GTA;
using GTA.Math;
using GTA.Native;
using StreetRacing.Source.Cars;
using StreetRacing.Source.Cars.Tasks;
using System;
using System.Collections.Generic;

namespace StreetRacing.Source.Races
{
    public class DistanceRace : Race
    {
        private RacingCar racingCar;

        private bool playerWinning = false;

        public DistanceRace()
        {
            UI.Notify("DistanceRace started");

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

            racingCar = new RacingCar(vehicles[vehicleIndex]);
            racingCar.SetTask(new DriverTaskCruise());
        }

        public override bool IsRacing { get; protected set; } = true;
        
        public override void Tick()
        {
            var player = Game.Player.Character;
            var distance = Game.Player.Character.Position.DistanceTo(racingCar.Driver.Position);
            UI.ShowSubtitle($"Distance to racer: {distance} - Winning: {playerWinning}");

            Overtake(player);
            Finish();
        }

        private void Finish()
        {
            if (GetDistance() > 300f)
            {
                if (playerWinning)
                {
                    UI.Notify($"Race over - You won");
                }
                else
                {
                    UI.Notify($"Race over - You lost");
                }

                IsRacing = false;
            }
        }

        private void Overtake(Ped player)
        {
            if (GetDistance() < overtakeDistance)
            {
                var heading = player.Position - racingCar.Driver.Position;
                var dot = Vector3.Dot(heading.Normalized, racingCar.Driver.ForwardVector.Normalized);

                if (dot > 0)
                {
                    playerWinning = true;

                    if (GetDistance() > 20f)
                    {
                        racingCar.SetTask(new DriverTaskChase());
                    }
                    else
                    {
                        racingCar.SetTask(new DriverTaskCruise());
                    }
                }
                else
                {
                    playerWinning = false;
                    racingCar.SetTask(new DriverTaskCruise());
                }
            }
        }

        private float GetDistance()
        {
            return Game.Player.Character.Position.DistanceTo(racingCar.Driver.Position);
        }
    }
}