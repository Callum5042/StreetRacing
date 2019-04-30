using GTA;
using GTA.Math;
using StreetRacing.Source.Tasks;
using System;

namespace StreetRacing.Source.Racers
{
    public class RacingDriver : IRacingDriver
    {
        public RacingDriver() { }

        public RacingDriver(IRacingDriver driver)
        {
            RacePosition = driver.RacePosition;
            Driver = driver.Driver;
            Vehicle = driver.Vehicle;
            DriverTask = driver.DriverTask;
            IsPlayer = driver.IsPlayer;
            InRace = driver.InRace;
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

        public void Lost()
        {
            InRace = false;
            Vehicle.CurrentBlip.Remove();

            if (!IsPlayer)
            {
                SetTask(new DriverTaskCruise());
            }
        }

        public float Distance(IRacingDriver driver)
        {
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

        public bool IsInFront(IRacingDriver driver)
        {
            var heading = Vehicle.Position - driver.Vehicle.Position;
            return Vector3.Dot(heading.Normalized, driver.Vehicle.Driver.ForwardVector.Normalized) > 0;
        }
    }
}