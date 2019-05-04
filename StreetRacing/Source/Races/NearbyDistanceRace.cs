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
    }
}