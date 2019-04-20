using GTA;
using GTA.Math;
using GTA.Native;
using StreetRacing.Source.Vehicles;
using System;
using System.Collections.Generic;

namespace StreetRacing.Source.Races
{
    public abstract class Race : IRace
    {
        public int PlayerPosition { get; set; }

        public abstract bool IsRacing { get; protected set; }

        public abstract void Tick();

        protected readonly float overtakeDistance = 40f;

        protected IList<IRacingVehicle> Vehicles = new List<IRacingVehicle>();

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

        protected void CalculatePlayerPosition()
        {
            PlayerPosition = Vehicles.Count + 1;
            foreach (var vehicle in Vehicles)
            {
                if (GetDistance(vehicle) < overtakeDistance)
                {
                    var heading = Game.Player.Character.Position - vehicle.Driver.Position;
                    var dot = Vector3.Dot(heading.Normalized, vehicle.Driver.ForwardVector.Normalized);

                    if (dot > 0)
                    {
                        PlayerPosition--;
                    }
                }
            }
        }

        protected float GetDistance(IRacingVehicle vehicle)
        {
            return Game.Player.Character.Position.DistanceTo(vehicle.Driver.Position);
        }
    }
}