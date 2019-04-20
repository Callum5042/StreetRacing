using GTA;
using GTA.Math;
using StreetRacing.Source.Cars;

namespace StreetRacing.Source.Races
{
    public class SprintRace : IRace
    {
        private RacingCar racingCar;

        private bool playerWinning = false;

        private float overtakeDistance = 40f;

        public SprintRace()
        {
            UI.Notify("SprintRace started");

            racingCar = new RacingCar(GTA.Native.VehicleHash.Comet3);

            racingCar.Driver.Task.DriveTo(racingCar.Vehicle, World.GetWaypointPosition(), 1f, 100f);
        }

        public bool IsRacing { get; protected set; } = true;

        public void Tick()
        {
            var player = Game.Player.Character;
            var distance = Game.Player.Character.Position.DistanceTo(racingCar.Driver.Position);
            UI.ShowSubtitle($"Distance to racer: {distance} - Winning: {playerWinning}");


            if (GetDistance() < overtakeDistance)
            {
                var heading = player.Position - racingCar.Driver.Position;
                var dot = Vector3.Dot(heading.Normalized, racingCar.Driver.ForwardVector.Normalized);

                if (dot > 0)
                {
                    playerWinning = true;
                }
                else
                {
                    playerWinning = false;
                }
            }

            if (GetDistance() > 300f)
            {
                UI.ShowSubtitle($"Race over");
                IsRacing = false;
            }
        }

        private float GetDistance()
        {
            return Game.Player.Character.Position.DistanceTo(racingCar.Driver.Position);
        }
    }
}
