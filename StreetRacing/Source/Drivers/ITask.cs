using GTA.Math;

namespace StreetRacing.Source.Drivers
{
    public interface ITask
    {
        DriverTask DriverTask { get; }

        void DriveTo(Vector3 position);

        void Cruise();

        void Chase(IDriver driver);
    }
}