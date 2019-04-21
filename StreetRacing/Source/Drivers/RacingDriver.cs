using GTA;
using StreetRacing.Source.Tasks;

namespace StreetRacing.Source.Drivers
{
    public abstract class RacingDriver : IRacingDriver
    {
        ~RacingDriver()
        {
            Lost();
        }

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
    }
}