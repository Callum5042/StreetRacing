using StreetRacing.Source.Racers;

namespace StreetRacing.Source.Tasks
{
    public interface IDriverTask
    {
        DriverTask DriverTask { get; }

        void Handle(IRacingDriver vehicle);
    }
}