using StreetRacing.Source.Drivers;

namespace StreetRacing.Source.Tasks
{
    public interface IDriverTask
    {
        DriverTask DriverTask { get; }

        void Handle(IRacingDriver vehicle);
    }
}