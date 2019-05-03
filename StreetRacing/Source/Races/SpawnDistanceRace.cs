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

        protected override void Tick()
        {
            foreach (var driver in Racers.ToList())
            {
                if (driver.IsPlayer)
                {
                    if (driver.RacePosition == 1)
                    {
                        if (Racers.Count > 1)
                        {
                            var distance = driver.Distance(Racers.ElementAtOrDefault(1));
                        }
                    }
                    else
                    {
                        var distance = driver.Distance(Racers.First());
                    }
                }

                if (driver.RacePosition != 1)
                {
                    if (driver.Distance(Racers.First()) > configuration.WinDistance)
                    {
                        driver.Lost();
                        Racers.Remove(driver);

                        UI.Notify($"{driver.ToString()} lose");

                        if (driver.IsPlayer)
                        {
                            IsRacing = false;
                        }
                    }
                }

                if (Racers.Count == 1 || !IsRacing)
                {
                    IsRacing = false;
                    if (Racers.FirstOrDefault().IsPlayer)
                    {
                        UI.Notify($"You win");
                        Game.Player.Money += configuration.Money;
                    }
                    else
                    {
                        UI.Notify($"You lose");
                        Game.Player.Money -= configuration.Money;
                    }
                }
            }
        }
    }
}