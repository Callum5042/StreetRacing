using GTA;

namespace StreetRacing.Source.Drivers
{
    public class PlayerDriver : DriverBase
    {
        private readonly IConfiguration configuration;

        public PlayerDriver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override bool IsPlayer => true;

        public override void Finish()
        {
            InRace = false;

            if (RacePosition == 1)
            {
                UI.Notify("You win");
                Game.Player.Money += configuration.Money;
            }
            else
            {
                UI.Notify("You lose");
                Game.Player.Money -= configuration.Money;
            }
        }

        public override Vehicle Vehicle => Game.Player.Character.CurrentVehicle;
    }
}