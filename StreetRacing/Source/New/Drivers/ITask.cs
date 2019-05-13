using GTA.Math;
using StreetRacing.Source.Tasks;

namespace StreetRacing.Source.New.Drivers
{
    public interface ITask
    {
        DriverTask DriverTask { get; }

        void DriveTo(Vector3 position);

        void Cruise();

        void Chase(IDriver driver);
    }
}