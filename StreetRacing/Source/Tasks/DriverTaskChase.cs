using GTA;
using StreetRacing.Source.Vehicles;
using System;

namespace StreetRacing.Source.Tasks
{
    public class DriverTaskChase : IDriverTask
    {
        private readonly Ped ped;

        public DriverTaskChase(Ped ped)
        {
            this.ped = ped ?? throw new ArgumentNullException();
        }

        public DriverTask DriverTask => DriverTask.Chase;

        public void Handle(IRacingVehicle vehicle)
        {
            if (vehicle.DriverTask.DriverTask != DriverTask)
            {
                vehicle.Driver.Task.VehicleChase(ped);
            }
        }
    }
}