using GTA;
using GTA.Math;
using GTA.Native;
using System;

namespace StreetRacing.Source.Drivers
{
    public class ComputerDriver : DriverBase, ITask
    {
        private readonly IConfiguration configuration;
        private const int drivingStyle = 262204;

        public ComputerDriver(IConfiguration configuration, Vector3 position)
        {
            this.configuration = configuration;

            Vehicle = World.CreateVehicle(VehicleHash.Adder, position, Game.Player.Character.Heading);
            Start(Vehicle);
        }

        protected ComputerDriver(IConfiguration configuration) => this.configuration = configuration;

        protected void Start(Vehicle vehicle)
        {
            var ped = vehicle.CreateRandomPedOnSeat(VehicleSeat.Driver);
            ped.DrivingStyle = (DrivingStyle)drivingStyle;
            ped.AlwaysKeepTask = true;
            ped.DrivingSpeed = 200f;

            vehicle.AddBlip();
            vehicle.CurrentBlip.Color = BlipColor.Blue;
            vehicle.CurrentBlip.IsFlashing = false;
        }

        public override string ToString()
        {
            return Vehicle.FriendlyName;
        }

        public override bool IsPlayer => false;
        
        public DriverTask DriverTask { get; protected set; } = DriverTask.None;

        public override void Dispose()
        {
            Vehicle.Driver.Delete();
            Vehicle.Delete();
            Vehicle.CurrentBlip.Remove();
            InRace = false;
        }

        public void DriveTo(Vector3 position)
        {
            Vehicle.Driver.Task.DriveTo(Vehicle, position, 20f, 200f, drivingStyle);
        }

        public void Cruise()
        {
            if (DriverTask != DriverTask.Cruise)
            {
                DriverTask = DriverTask.Cruise;
                Vehicle.Driver.Task.CruiseWithVehicle(Vehicle, 200f, drivingStyle);
            }
        }

        public override void Finish()
        {
            Dispose();
        }

        public override void UpdateBlip()
        {
            Vehicle.CurrentBlip.ShowNumber(RacePosition);
        }

        public void Chase(IDriver driver)
        {
            if (DriverTask != DriverTask.Chase)
            {
                DriverTask = DriverTask.Chase;
                Vehicle.Driver.Task.VehicleChase(driver.Vehicle.Driver);
            }
        }
    }
}