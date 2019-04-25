namespace StreetRacing.Source
{
    public class Configuration : IConfiguration
    {
        public bool Active { get; protected set; } = true;

        public int SpawnCount { get; protected set; } = 5;

        public bool MaxMods { get; protected set; }
    }
}