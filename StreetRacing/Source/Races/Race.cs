namespace StreetRacing.Source.Races
{
    public abstract class Race : IRace
    {
        public abstract bool IsRacing { get; protected set; }

        public abstract void Tick();

        protected readonly float overtakeDistance = 40f;
    }
}