using GTA;
using StreetRacing.Source.Tasks;

namespace StreetRacing.Source.Racers
{
    public interface IRacingDriver
    {
        Vehicle Vehicle { get; }

        int RacePosition { get; set; }

        Ped Driver { get; }

        IDriverTask DriverTask { get; }

        void SetTask(IDriverTask task);

        bool InRace { get; }

        void Lost();

        bool IsPlayer { get; }

        float DistanceTo(IRacingDriver driver);

        bool InFront(IRacingDriver driver);

        void Tick();
    }
}