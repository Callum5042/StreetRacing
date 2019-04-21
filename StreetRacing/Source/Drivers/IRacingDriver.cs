using GTA;
using StreetRacing.Source.Tasks;

namespace StreetRacing.Source.Drivers
{
    public interface IRacingDriver
    {
        Ped Driver { get; }

        Vehicle Vehicle { get; }

        int RacePosition { get; set; }

        IDriverTask DriverTask { get; }

        void SetTask(IDriverTask task);

        Blip Blip { get; set; }

        bool InRace { get; }

        void Lost();

        bool IsPlayer { get; }

        float Distance(IRacingDriver driver);
    }
}