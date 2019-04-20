namespace StreetRacing.Source.Races
{
    public interface IRace
    {
        void Tick();

        bool IsRacing { get; }
    }
}