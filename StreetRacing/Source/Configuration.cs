using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetRacing.Source
{
    public class Configuration : IConfiguration
    {
        public bool Active { get; protected set; } = true;

        public int SpawnCount { get; set; } = 5;
    }
}