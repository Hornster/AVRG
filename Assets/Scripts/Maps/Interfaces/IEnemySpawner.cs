using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Data;

namespace Assets.Scripts.Maps.Interfaces
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// An interface defining behavior for obstacle spawners. Keep in mind that a spawner needs to have its parameters set
    /// and then to be initialized the with Initialize() method before being used.
    /// </summary>
    public interface IEnemySpawner
    {
        /// <summary>
        /// Defines amount of time, in seconds, between each spawn. Counts for all pools and all types of obstacles.
        /// </summary>
        float SpawnCooldown { get; set; };
        /// <summary>
        /// Called when all parameters are set. Begins process of initializing the object pools of the spawner.
        /// </summary>
        void Initialize(int maxObstaclesPerPool = SpawnerConstants.MaxPoolObstacles, int startingObstaclesPerPool = SpawnerConstants.StartingPoolObstacles);
    }
}
