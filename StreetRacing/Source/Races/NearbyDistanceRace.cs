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
            //var distance = Drivers.First().Distance(Drivers.Last());
            //if (distance > configuration.WinDistance || (Drivers.Count == 1 && Drivers.FirstOrDefault().IsPlayer))
            //{
            //    if (PlayerDriver.RacePosition == 1 || (Drivers.Count == 1 && Drivers.FirstOrDefault().IsPlayer))
            //    {
            //        var money = configuration.Money;
            //        UI.Notify($"You win: {money}");
            //        Game.Player.Money += money;
            //    }
            //    else
            //    {
            //        UI.Notify("You lose");
            //        Game.Player.Money -= configuration.Money;
            //    }

            //    Finish();
            //}
        }
    }
}