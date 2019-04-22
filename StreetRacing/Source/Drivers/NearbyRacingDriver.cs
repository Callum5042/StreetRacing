using GTA;
using GTA.Math;
using System.Collections.Generic;

namespace StreetRacing.Source.Drivers
{
    public class NearbyRacingDriver : RacingDriver
    {
        private readonly float startDistance = 20f;

        public NearbyRacingDriver()
        {
            Vehicle = GetClosestVehicleToPlayer(radius: startDistance);
            Vehicle.Driver.Delete();

            Driver = Vehicle.CreateRandomPedOnSeat(VehicleSeat.Driver);
            Driver.DrivingStyle = DrivingStyle.Normal | DrivingStyle.Rushed;
            Driver.AlwaysKeepTask = true;

            Vehicle.AddBlip();
            Vehicle.CurrentBlip.Color = BlipColor.Blue;
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