using GTA;

namespace StreetRacing.Source.Drivers
{
    public class NearbyDriver : ComputerDriver
    {
        public NearbyDriver(IConfiguration configuration, Vehicle vehicle) : base(configuration)
        {
            Vehicle = vehicle;
            Vehicle.Driver.Delete();

            Start(Vehicle);
        }

        public override void Dispose()
        {
            Cruise();
            Vehicle.CurrentBlip.Remove();
            InRace = false;
        }
    }
}