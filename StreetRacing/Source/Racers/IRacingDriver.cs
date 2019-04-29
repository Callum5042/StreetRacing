using GTA;
using StreetRacing.Source.Tasks;

namespace StreetRacing.Source.Racers
{
    public interface IRacingDriver
    {
        Ped Driver { get; }

        Vehicle Vehicle { get; }

        int RacePosition { get; set; }

        IDriverTask DriverTask { get; }

        void SetTask(IDriverTask task);

        bool InRace { get; }

        void Lost();

        bool IsPlayer { get; }

        float Distance(IRacingDriver driver);

        bool IsInFront(IRacingDriver driver);
    }
}