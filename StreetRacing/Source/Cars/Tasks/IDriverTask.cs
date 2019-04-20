namespace StreetRacing.Source.Cars.Tasks
{
    public interface IDriverTask
    {
        DriverTask DriverTask { get; }

        void Handle(RacingCar racingCar);
    }
}