using System;

namespace StreetRacing.Source.Races
{
    public interface IRace : IDisposable
    {
        bool IsRacing { get; }

        void Tick();
    }
}