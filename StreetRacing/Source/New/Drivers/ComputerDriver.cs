using GTA;
using GTA.Math;
using GTA.Native;
using StreetRacing.Source.Tasks;
using System;

namespace StreetRacing.Source.New.Drivers
{
    public class ComputerDriver : IDriver, ITask
    {
        private readonly IConfiguration configuration;
        private const int drivingStyle = 262204;

        public ComputerDriver(IConfiguration configuration, Vector3 position)
        {
            this.configuration = configuration;

            Vehicle = World.CreateVehicle(VehicleHash.Adder, position, Game.Player.Character.Heading);
            var ped = Vehicle.CreateRandomPedOnSeat(VehicleSeat.Driver);
            ped.DrivingStyle = (DrivingStyle)drivingStyle;
            ped.AlwaysKeepTask = true;
            ped.DrivingSpeed = 200f;

            Vehicle.AddBlip();
            Vehicle.CurrentBlip.Color = BlipColor.Blue;
            Vehicle.CurrentBlip.IsFlashing = false;
        }

        public bool IsPlayer => false;

        public int Checkpoint { get; set; }

        public int RacePosition { get; set; }

        public bool InRace { get; protected set; } = true;

        public Vector3 Position => Vehicle.Position;

        public Vector3 ForwardVector => Vehicle.ForwardVector;

        public DriverTask DriverTask { get; protected set; } = DriverTask.None;

        public Vehicle Vehicle { get; protected set; }

        public void Dispose()
        {
            Vehicle.Driver.Delete();
            Vehicle.Delete();
            Vehicle.CurrentBlip.Remove();
            InRace = false;
        }

        public float DistanceTo(Vector3 position)
        {
            return Vehicle.Position.DistanceTo(position);
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
                Vehicle.Driver.Task.CruiseWithVehicle(Vehicle, 200f);
            }
        }

        public void Finish()
        {
            Dispose();
        }

        public bool InFront(IDriver driver)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver));
            }

            var heading = Vehicle.Position - driver.Position;
            return Vector3.Dot(heading.Normalized, driver.ForwardVector.Normalized) > 0;
        }

        public void UpdateBlip()
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