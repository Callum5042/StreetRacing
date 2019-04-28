namespace StreetRacing.Source
{
    public interface IConfiguration
    {
        void Load();

        void Save();

        bool Active { get; }

        int SpawnCount { get; }

        bool MaxMods { get; }
    }
}