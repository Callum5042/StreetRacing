using GTA;
using GTA.Math;
using StreetRacing.Source.Tasks;
using System;
using System.Linq;

namespace StreetRacing.Source.Racers
{
    public class RacingDriver : IRacingDriver
    {
        protected readonly IConfiguration configuration;

        public RacingDriver() { }

        public RacingDriver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public int RacePosition { get; set; }

        public Ped Driver { get; protected set; }

        public Vehicle Vehicle { get; protected set; }

        public IDriverTask DriverTask { get; private set; } = new DriverTaskNone(); 

        public void SetTask(IDriverTask task)
        {
            task.Handle(this);
            DriverTask = task;
        }

        public bool InRace { get; protected set; } = true;

        public virtual bool IsPlayer { get; } = false;

        public virtual void Lost()
        {
            InRace = false;
            Vehicle.CurrentBlip.Remove();
        }

        public float DistanceTo(IRacingDriver driver)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver));
            }

            return Vehicle.Position.DistanceTo(driver.Driver.Position);
        }

        public override string ToString()
        {
            return Vehicle.FriendlyName;
        }

        public void SetModsMax()
        {
            Vehicle.InstallModKit();
            foreach (VehicleMod mod in Enum.GetValues(typeof(VehicleMod)))
            {
                int lastIndex = Vehicle.GetModCount(mod);
                Vehicle.SetMod(mod, lastIndex - 1, true);
            }
        }

        public bool InFront(IRacingDriver driver)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver));
            }

            var heading = Vehicle.Position - driver.Vehicle.Position;
            return Vector3.Dot(heading.Normalized, driver.Vehicle.Driver.ForwardVector.Normalized) > 0;
        }

        protected void ConfigureDefault()
        {
            Driver = Vehicle.CreateRandomPedOnSeat(VehicleSeat.Driver);
            Driver.DrivingStyle = DrivingStyle.Rushed;
            Driver.AlwaysKeepTask = true;
            Driver.DrivingSpeed = 200f;

            Vehicle.AddBlip();
            Vehicle.CurrentBlip.Color = BlipColor.Blue;
            Vehicle.CurrentBlip.IsFlashing = false;
            //Vehicle.MaxSpeed = 200f;
            //Vehicle.EnginePowerMultiplier = 120;
            //Vehicle.EngineTorqueMultiplier = 120;

            if (configuration.MaxMods)
            {
                SetModsMax();
            }
        }
        
        public void Tick()
        {
            Vehicle.CurrentBlip.ShowNumber(RacePosition);
            SetTasks();
            CheckDriverState();
        }

        private void SetTasks()
        {
            if (!IsPlayer)
            {
                if (RacePosition == 1)
                {
                    SetTask(new DriverTaskCruise());
                }
                else
                {
                    var firstPlace = Main.Race.Racers.FirstOrDefault(x => x.RacePosition == 1);
                    if (firstPlace != null)
                    {
                        if (DistanceTo(firstPlace) < 20f)
                        {
                            SetTask(new DriverTaskCruise());
                        }
                        else
                        {
                            SetTask(new DriverTaskChase(firstPlace));
                        }
                    }
                }
            }
        }

        private void CheckDriverState()
        {
            if (Vehicle.IsDead || Driver.IsDead || Driver.CurrentVehicle != Vehicle)
            {
                string message = "";
                if (!Vehicle.IsDriveable)
                {
                    message = "Vehicle is undriveable";
                }
                else if (Driver.IsDead)
                {
                    message = "Driver has died";
                }
                else if (Driver.CurrentVehicle != Vehicle)
                {
                    message = "Driver has left the vehicle";
                }

                UI.Notify($"{Vehicle.FriendlyName} has been disqualified - {message}");
                Lost();
            }
        }

        public int Checkpoint { get; set; }
    }
}