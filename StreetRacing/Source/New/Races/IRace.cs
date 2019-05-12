using System;

namespace StreetRacing.Source.New.Races
{
    public interface IRace : IDisposable
    {
        bool IsRacing { get; }

        void Tick();
    }
}