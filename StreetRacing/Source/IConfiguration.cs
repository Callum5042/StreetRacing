using System.Windows.Forms;

namespace StreetRacing.Source
{
    public interface IConfiguration
    {
        void Load();

        void Save();

        Keys MenuKey { get; }

        Keys StartNearbyKey { get; }

        Keys StartSpawnKey { get; }

        bool Active { get; }

        int SpawnCount { get; }

        bool MaxMods { get; }

        float WinDistance { get; }

        int Money { get; }

        bool PolicePursuit { get; }
    }
}