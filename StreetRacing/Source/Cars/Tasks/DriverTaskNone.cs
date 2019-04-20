namespace StreetRacing.Source.Cars.Tasks
{
    public class DriverTaskNone : IDriverTask
    {
        public DriverTask DriverTask => DriverTask.None;

        public void Handle(RacingCar racingCar)
        {
        }
    }
}