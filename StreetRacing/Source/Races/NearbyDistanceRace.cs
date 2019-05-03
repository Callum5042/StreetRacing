using GTA;
using StreetRacing.Source.Racers;
using StreetRacing.Source.Tasks;
using System.Linq;

namespace StreetRacing.Source.Races
{
    public class NearbyDistanceRace : DistanceRace
    {
        public NearbyDistanceRace(IConfiguration configuration) : base(configuration)
        {
            Racers.Add(new PlayerRacingDriver());
            Racers.Add(new NearbyRacingDriver(configuration));

            CalculateStartPositions();
        }

        public override void Finish()
        {
            base.Finish();
            foreach (var driver in Racers.Where(x => !x.IsPlayer))
            {
                driver.Vehicle.CurrentBlip.Remove();
                driver.SetTask(new DriverTaskCruise());
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