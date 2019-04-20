using GTA;
using GTA.Math;
using StreetRacing.Source.Tasks;
using StreetRacing.Source.Vehicles;
using System.Linq;
using System.Text;

namespace StreetRacing.Source.Races
{
    public class DistanceRace : Race
    {
        public IRacingVehicle First { get; set; }

        public DistanceRace(bool spawn, int numberOfVehicles)
        {
            if (spawn)
            {
                for (int i = 1; i <= numberOfVehicles; ++i)
                {
                    var position = Game.Player.Character.Position + (Game.Player.Character.ForwardVector * (6.0f * i));// + (Game.Player.Character.RightVector * 3.0f);
                    Vehicles.Add(new SpawnRacingVehicle(SpawnRandomVehicle(), position));
                }
            }
            else
            {
                Vehicles.Add(new NearbyRacingVehicle());
                UI.Notify($"DistanceRace started again: {Vehicles.First().Vehicle.DisplayName}");
            }
        }

        public override bool IsRacing { get; protected set; } = true;
        
        public override void Tick()
        {



            /*
             * 
             * 
             * */

            //float distanceToFirst = 0;
            //if (First != null)
            //{
            //    distanceToFirst = Game.Player.Character.Position.DistanceTo(First.Vehicle.Position);
            //}
            //else
            //{
            //    distanceToFirst = Game.Player.Character.Position.DistanceTo(Vehicles.First().Vehicle.Position);
            //}


            //if (distanceToFirst < 20f)
            //{
            //    // Calculate player position
            //    PlayerPosition = Vehicles.Count + 1;
            //    foreach (var vehicle in Vehicles)
            //    {
            //        var heading = Game.Player.Character.Position - vehicle.Driver.Position;
            //        var dot = Vector3.Dot(heading.Normalized, vehicle.Driver.ForwardVector.Normalized);

            //        if (dot > 0)
            //        {
            //            PlayerPosition--;
            //        }
            //    }

            //    // Calculate bots position
            //    foreach (var vehicle in Vehicles)
            //    {
            //        vehicle.Position = Vehicles.Count + 1;
            //        foreach (var otherVehicle in Vehicles)
            //        {
            //            var heading = vehicle.Vehicle.Position - otherVehicle.Driver.Position;
            //            var dot = Vector3.Dot(heading.Normalized, otherVehicle.Driver.ForwardVector.Normalized);

            //            if (dot > 0)
            //            {
            //                vehicle.Position--;
            //            }
            //        }

            //        // Check player
            //        var xheading = vehicle.Vehicle.Position - Game.Player.Character.Position;
            //        var xdot = Vector3.Dot(xheading.Normalized, Game.Player.Character.ForwardVector.Normalized);

            //        if (xdot > 0)
            //        {
            //            vehicle.Position--;
            //        }

            //        if (vehicle.Position == 1)
            //        {
            //            First = vehicle;
            //        }
            //    }
            //}

            //// Give tasks
            //foreach (var vehicle in Vehicles)
            //{
            //    if (PlayerPosition == 1)
            //    {
            //        SetTaskChase(vehicle, Game.Player.Character);
            //    }
            //    else
            //    {
            //        if (vehicle == First)
            //        {
            //            vehicle.SetTask(new DriverTaskCruise());
            //        }
            //        else
            //        {
            //            SetTaskChase(vehicle, First.Driver);
            //        }
            //    }

            //    vehicle.Blip.Position = vehicle.Vehicle.Position;

            //    var distanceToFirstVehicle = vehicle.Vehicle.Position.DistanceTo(First.Vehicle.Position);
            //    if (distanceToFirst > 100f)
            //    {
            //        vehicle.Lost();
            //        UI.Notify($"{vehicle.Vehicle.DisplayName} has lost");
            //    }
            //}

            //if (Vehicles.All(x => !x.InRace))
            //{
            //    IsRacing = false;
            //    UI.Notify($"You win");
            //}

            //// show
            //var message = new StringBuilder();
            //message.Append($"Position: {PlayerPosition}");

            
            //message.Append($" - Distance: {distanceToFirst}");

            //UI.ShowSubtitle(message.ToString());
        }

        private void SetTaskChase(IRacingVehicle vehicle, Ped target)
        {
            var distanceToFirst = vehicle.Vehicle.Position.DistanceTo(target.Position);
            if (distanceToFirst < 20f)
            {
                vehicle.SetTask(new DriverTaskCruise());
            }
            else
            {
                vehicle.SetTask(new DriverTaskChase(target));
            }
        }
    }
}