using GTA;
using StreetRacing.Source.Tasks;
using System;

namespace StreetRacing.Source.Racers
{
    public abstract class RacingDriver : IRacingDriver
    {
        public int RacePosition { get; set; }

        public Ped Driver { get; protected set; }

        public Vehicle Vehicle { get; protected set; }

        public IDriverTask DriverTask { get; private set; } = new DriverTaskNone(); 

        public Blip Blip { get; set; }

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
            Blip?.Remove();

            SetTask(new DriverTaskCruise());
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
    }
}