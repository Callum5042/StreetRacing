using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetRacing.Source.New.Drivers
{
    public interface ITask
    {
        void DriveTo(Vector3 position);

        void Cruise();
    }
}