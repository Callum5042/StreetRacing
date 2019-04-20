using StreetRacing.Source.Vehicles;

namespace StreetRacing.Source.Tasks
{
    public class DriverTaskNone : IDriverTask
    {
        public DriverTask DriverTask => DriverTask.None;

        public void Handle(IRacingVehicle vehicle)
        {
        }
    }
}