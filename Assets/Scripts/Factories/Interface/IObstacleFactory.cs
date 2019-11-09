using Assets.Scripts.Enemies.Interface;
using Assets.Scripts.Shared.Enums;

namespace Assets.Scripts.Factories.Interface
{
    public interface IObstacleFactory
    {
        IObstacle CreateObstacle(ObstacleTypeEnum type, ObstacleIniData iniData);
    }
}
