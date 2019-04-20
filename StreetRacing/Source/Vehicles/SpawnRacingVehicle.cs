using GTA;
using GTA.Math;
using GTA.Native;

namespace StreetRacing.Source.Vehicles
{
    public class SpawnRacingVehicle : RacingVehicle
    {
        public SpawnRacingVehicle(VehicleHash vehicle, Vector3 spawnPosition)
        {
            Vehicle = World.CreateVehicle(vehicle, spawnPosition, Game.Player.Character.Heading);

            Driver = Vehicle.CreateRandomPedOnSeat(VehicleSeat.Driver);
            Driver.DrivingStyle = DrivingStyle.Normal | DrivingStyle.Rushed;
            Driver.AlwaysKeepTask = true;

            Blip = World.CreateBlip(Driver.Position);
            Blip.Name = "Racer";
        }
    }
}