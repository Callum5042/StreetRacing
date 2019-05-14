using GTA;
using GTA.Math;
using StreetRacing.Source.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetRacing.Source.Races
{
    public class NearbyRace : DistanceRace
    {
        public NearbyRace(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void LoadVehicles()
        {
            var vehicle = GetClosestVehicleToPlayer(radius: 20f);
            if (vehicle == null)
            {
                IsRacing = false;
                throw new InvalidOperationException();
            }
            
            Drivers.Add(new PlayerDriver(configuration) { RacePosition = 2 });
            Drivers.Add(new NearbyDriver(configuration, vehicle) { RacePosition = 1 });
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