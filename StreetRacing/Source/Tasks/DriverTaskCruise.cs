using StreetRacing.Source.Racers;

namespace StreetRacing.Source.Tasks
{
    public class DriverTaskCruise : IDriverTask
    {
        public DriverTask DriverTask => DriverTask.Cruise;

        public void Handle(IRacingDriver vehicle)
        {
            if (vehicle.DriverTask.DriverTask != DriverTask)
            {
                vehicle.Driver.Task.CruiseWithVehicle(vehicle.Vehicle, 200f, 262204);
            }
        }
    }
}