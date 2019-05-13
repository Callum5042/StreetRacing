using GTA;
using GTA.Math;
using System;

namespace StreetRacing.Source.New.Drivers
{
    public class PlayerDriver : IDriver
    {
        private readonly IConfiguration configuration;

        public PlayerDriver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public bool IsPlayer => true;

        public int Checkpoint { get; set; }

        public int RacePosition { get; set; }

        public float DistanceTo(Vector3 position)
        {
            return Game.Player.Character.CurrentVehicle.Position.DistanceTo(position);
        }

        public void Dispose()
        {
            
        }

        public void Finish()
        {
            
        }

        public bool InFront(IDriver driver)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver));
            }

            var heading = Game.Player.Character.Position - driver.Position;
            return Vector3.Dot(heading.Normalized, driver.ForwardVector.Normalized) > 0;
        }

        public void UpdateBlip()
        {

        }

        public bool InRace { get; protected set; } = true;

        public Vector3 Position => Game.Player.Character.Position;

        public Vector3 ForwardVector => Game.Player.Character.ForwardVector;

        public Vehicle Vehicle => Game.Player.Character.CurrentVehicle;
    }
}