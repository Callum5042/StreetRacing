using StreetRacing.Source.Vehicles;
using System.Collections.Generic;

namespace StreetRacing.Source.Races
{
    public interface IRace
    {
        void Tick();

        bool IsRacing { get; }
    }
}