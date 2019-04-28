using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;

namespace StreetRacing.Source.Racers
{
    public class NearbyRacingDriver : RacingDriver
    {
        private readonly float startRadius = 20f;

        public NearbyRacingDriver(IConfiguration configuration)
        {
            Vehicle = GetClosestVehicleToPlayer(radius: startRadius);
            Vehicle.Driver.Delete();

            Driver = Vehicle.CreateRandomPedOnSeat(VehicleSeat.Driver);
            Driver.DrivingStyle = DrivingStyle.Normal | DrivingStyle.Rushed;
            Driver.AlwaysKeepTask = true;

            Vehicle.AddBlip();
            Vehicle.CurrentBlip.Color = BlipColor.Blue;

            if (configuration.MaxMods)
            {
                SetModsMax();
            }
        }

        private Vehicle GetClosestVehicleToPlayer(float radius)
        {
            IList<Vehicle> vehicles = World.GetNearbyVehicles(Game.Player.Character.Position, radius);
            Vehicle closestVehicle = null;
            float? closestDistance = null;

            var playerPosition = Game.Player.Character.Position;
            foreach (var vehicle in vehicles)
            {
                if (!vehicle.Driver.IsPlayer && vehicle.IsAlive)
                {
                    if (closestVehicle == null)
                    {
                        closestVehicle = vehicle;
                        closestDistance = Vector3.Distance(playerPosition, vehicle.Position);
                        continue;
                    }

                    var distance = Vector3.Distance(playerPosition, vehicle.Position);
                    if (distance < closestDistance)
                    {
                        closestVehicle = vehicle;
                        closestDistance = distance;
                    }
                }
            }

            return closestVehicle;
        }
    }
}