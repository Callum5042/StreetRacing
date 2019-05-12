using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreetRacing.Source.Racers;

namespace StreetRacing.Source.Tasks
{
    public class DriverTaskCheckpoint : IDriverTask
    {
        public DriverTask DriverTask => DriverTask.Checkpoint;

        public void Handle(IRacingDriver vehicle)
        {
            
        }
    }
}