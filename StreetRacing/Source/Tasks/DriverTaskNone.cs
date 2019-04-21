using StreetRacing.Source.Drivers;

namespace StreetRacing.Source.Tasks
{
    public class DriverTaskNone : IDriverTask
    {
        public DriverTask DriverTask => DriverTask.None;

        public void Handle(IRacingDriver vehicle)
        {
        }
    }
}