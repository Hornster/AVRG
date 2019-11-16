using System;
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
    [RequireComponent(typeof(ObstacleDataRandomizer))]
    public class EnemySpawnerRectangular : MonoBehaviour, IEnemySpawner
    {
        /// <summary>
        /// Factory that will be used to generate objects.
        /// </summary>
        private IObstacleFactory _obstacleFactory;
        /// <summary>
        /// Defines amount of time, in seconds, between each spawn. Counts for all pools and all types of obstacles.
        /// By default set to 2 seconds.
        /// </summary>
        public float SpawnCooldown { get; set; } = 2;
        /// <summary>
        /// A constant force applied to each enemy. Makes them move in given direction.
        /// </summary>
        public Vector3 EnemiesConstantForce { get; set; }

        [SerializeField]
        private int _maxObstaclesPerPool = GameConstants.MaxPoolObstacles;
        [SerializeField]
        private int _startingObstaclesPerPool = GameConstants.StartingPoolObstacles;

        /// <summary>
        /// Stores all available enemy pools.
        /// </summary>
        [SerializeField]
        private readonly Dictionary<ObstacleTypeEnum, ObstaclesPool> _enemiesPools = new Dictionary<ObstacleTypeEnum, ObstaclesPool>();

        [SerializeField] private ObstacleDataRandomizer _obstacleDataRandomizer;
        /// <summary>
        /// Stores the time that lasted from the most recent obstacle spawn.
        /// </summary>
        private float _currentCooldown = 0.0f;

        /// <summary>
        /// Spawns randomly selected obstacle.
        /// </summary>
        private void SpawnObstacle()
        {
            var decisionMaker = ProbabilityDecisionMaker.GetInstance();
            var obstacleToSpawn = decisionMaker.SpawnWhatObstacle();
            ObstaclesPool poolToUse;
            if (_enemiesPools.TryGetValue(obstacleToSpawn, out poolToUse) == false)
            {
                throw new Exception($"Tried to spawn obstacle from non-existent pool: {obstacleToSpawn}!");
            }

            var obstacleData = _obstacleDataRandomizer.GetRandomizedObstacleData(obstacleToSpawn);
            obstacleData.ConstantForce = EnemiesConstantForce;

            poolToUse.SpawnObstacle(obstacleData);
        }
        /// <summary>
        /// Creates all pools for the spawner.
        /// </summary>
        void Start()
        {
            _obstacleFactory = gameObject.GetComponent<IObstacleFactory>();

            if (_obstacleFactory == null)
            {
                throw new Exception("Error: Obstacle factory not found! You must provide a script that inherits after IObstacleFactory!");
            }

            var newPool = new ObstaclesPool(ObstacleTypeEnum.EnergyBlock, _obstacleFactory, _maxObstaclesPerPool, _startingObstaclesPerPool);
            _enemiesPools.Add(ObstacleTypeEnum.EnergyBlock, newPool);
            newPool = new ObstaclesPool(ObstacleTypeEnum.PhysicalBlock, _obstacleFactory, _maxObstaclesPerPool, _startingObstaclesPerPool);
            _enemiesPools.Add(ObstacleTypeEnum.PhysicalBlock, newPool);
        }
        
        
        void Update()
        {
            _currentCooldown += Time.deltaTime;
            if (_currentCooldown >= SpawnCooldown)
            {
                SpawnObstacle();
                _currentCooldown = 0.0f;
            }
        }

        public void ResetSpawner()
        {
            //TODO - modify spawner - make it use local variables during the match, the values provided during startup should
            //TODO remembered and restored upon stage reset. Add resetting methods to pools (reset obstacles positions and spawning
            //TODO values). Connect callbacks from MatchController to ResultsMenuController and player.
        }
    }
}
