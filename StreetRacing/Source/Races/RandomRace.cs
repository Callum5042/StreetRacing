using GTA;
using StreetRacing.Source.Racers;
using StreetRacing.Source.Tasks;
using System.Linq;

namespace StreetRacing.Source.Races
{
    public class RandomRace : StreetRace
    {
        private readonly IConfiguration configuration;

        public RandomRace(IConfiguration configuration)
        {
            this.configuration = configuration;

            try
            {
                Drivers.Add(PlayerDriver);
                Drivers.Add(new NearbyRacingDriver(configuration));
            }
            catch (System.InvalidOperationException)
            {
                UI.Notify("Unable to find car in range");
                IsRacing = false;
            }

            if (IsRacing)
            {
                UI.Notify("Race started");
            }
        }

        public override void Finish()
        {
            IsRacing = false;
            foreach (var driver in Drivers.Where(x => !x.IsPlayer))
            {
                driver.Vehicle.CurrentBlip.Remove();
                driver.SetTask(new DriverTaskCruise());
            }
        }

        protected override void Other()
        {
            var distance = Drivers.First().Distance(Drivers.Last());
            if (distance > configuration.WinDistance || (Drivers.Count == 1 && Drivers.FirstOrDefault().IsPlayer))
            {
                if (PlayerDriver.RacePosition == 1 || (Drivers.Count == 1 && Drivers.FirstOrDefault().IsPlayer))
                {
                    var money = configuration.Money;
                    UI.Notify($"You win: {money}");
                    Game.Player.Money += money;
                }
                else
                {
                    UI.Notify("You lose");
                    Game.Player.Money -= configuration.Money;
                }

                Finish();
            }
        }
    }
}