using UnityEngine;

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
        float SpawnCooldown { get; set; }
        /// <summary>
        /// A constant force applied to each enemy. Makes them move in given direction.
        /// </summary>
        Vector3 EnemiesConstantForce { get; set; }
    }
}
