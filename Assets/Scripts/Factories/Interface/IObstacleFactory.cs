using Assets.Scripts.Enemies.Interface;
using Assets.Scripts.Shared.Enums;

namespace Assets.Scripts.Factories.Interface
{
    /// <summary>
    /// Interface for obstacle factories.
    /// </summary>
    public interface IObstacleFactory
    {
        /// <summary>
        /// Creates new obstacle basing on provided data.
        /// </summary>
        /// <param name="type">Type of the obstacle.</param>
        /// <param name="iniData">Initialization data for the obstacle.</param>
        /// <returns></returns>
        IObstacle CreateObstacle(ObstacleTypeEnum type, ObstacleIniData iniData);
    }
}
