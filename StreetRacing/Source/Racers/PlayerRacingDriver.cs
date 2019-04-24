using GTA;

namespace StreetRacing.Source.Racers
{
    public class PlayerRacingDriver : RacingDriver
    {
        public PlayerRacingDriver()
        {
            Vehicle = Game.Player.Character.CurrentVehicle;
            Driver = Game.Player.Character;
        }

        public override bool IsPlayer => true;
    }
}