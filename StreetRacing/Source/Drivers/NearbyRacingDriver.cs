using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;

namespace StreetRacing.Source.Drivers
{
    public class NearbyRacingDriver : RacingDriver
    {
        public NearbyRacingDriver()
        {
            Vehicle = GetClosestVehicleToPlayer(radius: 20f);
            if (Vehicle == null)
            {
                throw new InvalidOperationException("Could not find vehicle in range");
            }

            Driver = Vehicle.Driver;
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