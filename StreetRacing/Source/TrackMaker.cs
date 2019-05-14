using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetRacing.Source
{
    public class TrackMaker
    {
        private readonly IConfiguration configuration;

        public TrackMaker(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Tick()
        {

        }
    }
}