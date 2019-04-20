using StreetRacing.Source.Vehicles;

namespace StreetRacing.Source.Tasks
{
    public interface IDriverTask
    {
        DriverTask DriverTask { get; }

        void Handle(IRacingVehicle vehicle);
    }
}