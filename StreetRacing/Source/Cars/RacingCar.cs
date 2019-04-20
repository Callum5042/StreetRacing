using GTA;
using GTA.Native;
using StreetRacing.Source.Cars.Tasks;
using System;
using System.Collections.Generic;

namespace StreetRacing.Source.Cars
{
    public class RacingCar
    {
        public Ped Driver { get; private set; }

        public Vehicle Vehicle { get; private set; }

        public IDriverTask DriverTask { get; private set; } = new DriverTaskNone();

        public RacingCar(VehicleHash vehicle)
        {
            InitialiseVehicle(vehicle);
            InitialiseDriver();
        }

        public void SetTask(IDriverTask task)
        {
            task.Handle(this);
            DriverTask = task;
        }

        private void InitialiseDriver()
        {
            Driver = Vehicle.CreateRandomPedOnSeat(VehicleSeat.Driver);
            Driver.DrivingStyle = DrivingStyle.AvoidTrafficExtremely;
            Driver.AlwaysKeepTask = true;
        }

        private void InitialiseVehicle(VehicleHash vehicle)
        {
            var vehicleSpawnPosition = Game.Player.Character.Position + Game.Player.Character.RightVector * 3.0f;
            Vehicle = World.CreateVehicle(vehicle, vehicleSpawnPosition, Game.Player.Character.Heading);

            foreach (VehicleMod mod in Enum.GetValues(typeof(VehicleMod)))
            {
                SetMod(Vehicle, mod);
            }
        }

        private void SetMod(Vehicle vehicle, VehicleMod mod)
        {
            var random = new Random();
            vehicle.SetMod(mod, random.Next(-1, vehicle.GetModCount(mod)), true);
        }
    }
}