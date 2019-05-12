using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;

namespace StreetRacing.Source.New.Drivers
{
    public class ComputerDriver : IDriver, ITask
    {
        private readonly IConfiguration configuration;
        private Ped ped;
        private Vehicle vehicle;
        private const int drivingStyle = 262204;

        public ComputerDriver(IConfiguration configuration, Vector3 position)
        {
            this.configuration = configuration;

            vehicle = World.CreateVehicle(VehicleHash.Adder, position, Game.Player.Character.Heading);
            ped = vehicle.CreateRandomPedOnSeat(VehicleSeat.Driver);
            ped.DrivingStyle = DrivingStyle.Rushed;
            ped.AlwaysKeepTask = true;
            ped.DrivingSpeed = 200f;

            vehicle.AddBlip();
            vehicle.CurrentBlip.Color = BlipColor.Blue;
            vehicle.CurrentBlip.IsFlashing = false;
        }

        public bool IsPlayer => false;

        public int Checkpoint { get; set; }

        public int RacePosition { get; set; }

        public bool InRace { get; protected set; } = true;

        public Vector3 Position => vehicle.Position;

        public Vector3 ForwardVector => vehicle.ForwardVector;

        public void Dispose()
        {
            vehicle.Driver.Delete();
            vehicle.Delete();
            vehicle.CurrentBlip.Remove();
            InRace = false;
        }

        public float DistanceTo(Vector3 position)
        {
            return vehicle.Position.DistanceTo(position);
        }

        public void DriveTo(Vector3 position)
        {
            ped.Task.DriveTo(ped.CurrentVehicle, position, 20f, 200f, drivingStyle);
        }

        public void Cruise()
        {
            ped.Task.CruiseWithVehicle(ped.CurrentVehicle, 200f);
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

            var heading = vehicle.Position - driver.Position;
            return Vector3.Dot(heading.Normalized, driver.ForwardVector.Normalized) > 0;
        }

        public void UpdateBlip()
        {
            vehicle.CurrentBlip.ShowNumber(RacePosition);
        }
    }
}