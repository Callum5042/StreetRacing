using StreetRacing.Source.Racers;
using System;

namespace StreetRacing.Source.Tasks
{
    public class DriverTaskChase : IDriverTask
    {
        private readonly IRacingDriver racingDriver;

        public DriverTaskChase(IRacingDriver racingDriver)
        {
            this.racingDriver = racingDriver ?? throw new ArgumentNullException();
        }

        public DriverTask DriverTask => DriverTask.Chase;

        public void Handle(IRacingDriver vehicle)
        {
            if (vehicle.DriverTask.DriverTask != DriverTask)
            {
                vehicle.Driver.Task.VehicleChase(racingDriver.Driver);
            }
        }
    }
}