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

            Drivers.Add(PlayerDriver);
            Drivers.Add(new NearbyRacingDriver(configuration));
            
            UI.Notify("RandomRace started");
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
            if (distance > configuration.WinDistance)
            {
                if (PlayerDriver.RacePosition == 1)
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

            // Display
            UI.ShowSubtitle($"Position: {PlayerDriver.RacePosition} - Distance: {distance}");
        }
    }
}