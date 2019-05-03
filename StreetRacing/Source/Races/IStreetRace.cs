using StreetRacing.Source.Racers;
using System;
using System.Collections.Generic;

namespace StreetRacing.Source.Races
{
    public interface IStreetRace
    {
        bool IsRacing { get; }

        IList<IRacingDriver> Racers { get; }

        void OnTick(object sender, EventArgs e);

        void Finish();
    }
}