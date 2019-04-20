using GTA;

namespace StreetRacing.Source.Cars.Tasks
{
    public class DriverTaskChase : IDriverTask
    {
        public DriverTask DriverTask => DriverTask.Chase;

        public void Handle(RacingCar racingCar)
        {
            if (racingCar.DriverTask.DriverTask != DriverTask)
            {
                UI.Notify($"DriverTaskChase");
                racingCar.Driver.Task.VehicleChase(Game.Player.Character);
            }
        }
    }
}
