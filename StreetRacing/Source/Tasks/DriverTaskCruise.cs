using StreetRacing.Source.Vehicles;

namespace StreetRacing.Source.Tasks
{
    public class DriverTaskCruise : IDriverTask
    {
        public DriverTask DriverTask => DriverTask.Cruise;

        public void Handle(IRacingVehicle vehicle)
        {
            if (vehicle.DriverTask.DriverTask != DriverTask)
            {
                vehicle.Driver.Task.CruiseWithVehicle(vehicle.Vehicle, 150f);
            }
        }
    }
}