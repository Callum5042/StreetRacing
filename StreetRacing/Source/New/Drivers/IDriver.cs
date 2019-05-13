using GTA;
using GTA.Math;
using System;

namespace StreetRacing.Source.New.Drivers
{
    public interface IDriver : IDisposable
    {
        bool IsPlayer { get; }

        int Checkpoint { set; get; }

        int RacePosition { set; get; }

        float DistanceTo(Vector3 position);

        bool InRace { get; }

        void Finish();

        bool InFront(IDriver driver);

        Vector3 Position { get; }

        Vector3 ForwardVector { get; }

        void UpdateBlip();

        Vehicle Vehicle { get; }
    }
}