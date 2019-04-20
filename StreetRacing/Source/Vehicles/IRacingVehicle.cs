using GTA;
using StreetRacing.Source.Tasks;

namespace StreetRacing.Source.Vehicles
{
    public interface IRacingVehicle
    {
        Ped Driver { get; }

        Vehicle Vehicle { get; }

        int Position { get; set; }

        IDriverTask DriverTask { get; }

        void SetTask(IDriverTask task);

        Blip Blip { get; set; }

        bool InRace { get; }

        void Lost();

        bool IsPlayer { get; }
    }
}