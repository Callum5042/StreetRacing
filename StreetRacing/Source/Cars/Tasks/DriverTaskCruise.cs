using GTA;

namespace StreetRacing.Source.Cars.Tasks
{
    public class DriverTaskCruise : IDriverTask
    {
        public DriverTask DriverTask => DriverTask.Cruise;

        public void Handle(RacingCar racingCar)
        {
            if (racingCar.DriverTask.DriverTask != DriverTask)
            {
                UI.Notify($"DriverTaskCruise");
                racingCar.Driver.Task.CruiseWithVehicle(racingCar.Vehicle, 150f);
            }
        }
    }
}