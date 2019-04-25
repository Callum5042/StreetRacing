namespace StreetRacing.Source
{
    public interface IConfiguration
    {
        bool Active { get; }

        int SpawnCount { get; }

        bool MaxMods { get; }
    }
}