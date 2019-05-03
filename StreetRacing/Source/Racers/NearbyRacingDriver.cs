using GTA;
using GTA.Math;
using System.Collections.Generic;
using System.Linq;

namespace StreetRacing.Source.Racers
{
    public class NearbyRacingDriver : RacingDriver
    {
        private readonly float startRadius = 20f;

        public NearbyRacingDriver(IConfiguration configuration) : base(configuration)
        {
            Vehicle = GetClosestVehicleToPlayer(radius: startRadius);
            if (Vehicle == null)
            {
                throw new System.InvalidOperationException("No vehicle in range");
            }

            Vehicle.Driver.Delete();
            ConfigureDefault();
        }

        private Vehicle GetClosestVehicleToPlayer(float radius)
        {
            IList<(float distance, Vehicle vehicle)> vehicles = new List<(float distance, Vehicle vehicle)>();
            foreach (var vehicle in World.GetNearbyVehicles(Game.Player.Character.Position, radius).Where(x => !x.Driver.IsPlayer && x.IsAlive))
            {
                var distance = Vector3.Distance(Game.Player.Character.Position, vehicle.Position);
                vehicles.Add((distance, vehicle));
            }

            return vehicles.OrderBy(x => x.distance).FirstOrDefault().vehicle;
        }
    }
}