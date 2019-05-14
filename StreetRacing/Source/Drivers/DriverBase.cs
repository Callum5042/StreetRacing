using GTA;
using GTA.Math;
using System;

namespace StreetRacing.Source.Drivers
{
    public abstract class DriverBase : IDriver
    {
        public virtual bool IsPlayer { get; protected set; }

        public int Checkpoint { get; set; }

        public int RacePosition { get; set; }

        public bool InRace { get; protected set; } = true;

        public Vector3 Position => Vehicle.Position;

        public Vector3 ForwardVector => Vehicle.ForwardVector;

        public virtual Vehicle Vehicle { get; protected set; }

        public void CheckVehicleState()
        {
            if (!Vehicle.IsAlive)
            {
                InRace = false;
            }

            if (!Vehicle.Driver.IsAlive)
            {
                InRace = false;
            }
        }

        public virtual void Dispose()
        {
            
        }

        public float DistanceTo(Vector3 position)
        {
            return Vehicle.Position.DistanceTo(position);
        }

        public virtual void Finish()
        {
            InRace = false;
        }

        public bool InFront(IDriver driver)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver));
            }

            var heading = Vehicle.Position - driver.Position;
            return Vector3.Dot(heading.Normalized, driver.ForwardVector.Normalized) > 0;
        }

        public virtual void UpdateBlip()
        {

        }

        public override string ToString()
        {
            return Vehicle.FriendlyName;
        }
    }
}