using GTA;
using StreetRacing.Source.Racers;
using System.Linq;

namespace StreetRacing.Source.Races
{
    public class SpawnDistanceRace : DistanceRace
    {
        public SpawnDistanceRace(IConfiguration configuration) : base(configuration)
        {
            Racers.Add(new PlayerRacingDriver());
            for (int i = 1; i <= configuration.SpawnCount; i++)
            {
                var position = Game.Player.Character.Position + (Game.Player.Character.ForwardVector * (6.0f * i));
                Racers.Add(new SpawnRacingDriver(configuration, position));
            }

            CalculateStartPositions();
        }

        public override void Finish()
        {
            base.Finish();
            foreach (var driver in Racers.Where(x => !x.IsPlayer))
            {
                driver.Vehicle.CurrentBlip.Remove();
                driver.Driver.Delete();
                driver.Vehicle.Delete();
            }
        }
    }
}