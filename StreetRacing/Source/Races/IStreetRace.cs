using System;

namespace StreetRacing.Source.Races
{
    public interface IStreetRace
    {
        bool IsRacing { get; }

        void OnTick(object sender, EventArgs e);

        void Finish();
    }
}