using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.Factories.Interface;
using Assets.Scripts.Maps.Interfaces;
using Assets.Scripts.Shared.Enums;
using UnityEngine;

namespace Assets.Scripts.Maps
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Enemy spawner that uses a rectangular surface.
    /// </summary>
    public class EnemySpawnerRectangular : MonoBehaviour, IEnemySpawner
    {

        /// <summary>
        /// Defines amount of time, in seconds, between each spawn. Counts for all pools and all types of obstacles.
        /// By default set to 2 seconds.
        /// </summary>
        public float SpawnCooldown { get; set; } = 2;
        
        /// <summary>
        /// Factory that will be used to generate objects.
        /// </summary>
        [SerializeField]
        private IObstacleFactory _obstacleFactory;
        /// <summary>
        /// Stores the time that lasted from the most recent obstacle spawn.
        /// </summary>
        private float _currentCooldown = 0.0f;

        /// <summary>
        /// Stores all available enemy pools.
        /// </summary>
        [SerializeField]
        private Dictionary<ObstacleTypeEnum, ObstaclesPool> _enemiesPools = new Dictionary<ObstacleTypeEnum, ObstaclesPool>();


        private void SpawnObstacle()
        {
            //TODO ProbabilityDecisionMaker has been recently created.
        }
        /// <summary>
        /// Shall be called when the parameters are set. Creates all pools for the spawner.
        /// </summary>
        public void Initialize(int maxObstaclesPerPool = SpawnerConstants.MaxPoolObstacles, int startingObstaclesPerPool = SpawnerConstants.StartingPoolObstacles)
        {
            var newPool = new ObstaclesPool(ObstacleTypeEnum.EnergyBlock, _obstacleFactory, maxObstaclesPerPool, startingObstaclesPerPool);
            _enemiesPools.Add(ObstacleTypeEnum.EnergyBlock, newPool);
            newPool = new ObstaclesPool(ObstacleTypeEnum.PhysicalBlock, _obstacleFactory, maxObstaclesPerPool, startingObstaclesPerPool);
            _enemiesPools.Add(ObstacleTypeEnum.EnergyBlock, newPool);
        }
        
        
        void Update()
        {
            _currentCooldown += Time.deltaTime;
            if (_currentCooldown >= SpawnCooldown)
            {

            }
        }
    }
}
