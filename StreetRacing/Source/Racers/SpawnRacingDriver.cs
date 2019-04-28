﻿using GTA;
using GTA.Math;
using GTA.Native;

namespace StreetRacing.Source.Racers
{
    public class SpawnRacingDriver : RacingDriver
    {
        public SpawnRacingDriver(IConfiguration configuration, VehicleHash vehicle, Vector3 spawnPosition)
        {
            Vehicle = World.CreateVehicle(vehicle, spawnPosition, Game.Player.Character.Heading);

            Driver = Vehicle.CreateRandomPedOnSeat(VehicleSeat.Driver);
            Driver.DrivingStyle = DrivingStyle.Normal | DrivingStyle.Rushed;
            Driver.AlwaysKeepTask = true;

            Vehicle.AddBlip();
            Vehicle.CurrentBlip.Color = BlipColor.Blue;
            Vehicle.CurrentBlip.IsFlashing = false;

            if (configuration.MaxMods)
            {
                SetModsMax();
            }
        }
    }
}