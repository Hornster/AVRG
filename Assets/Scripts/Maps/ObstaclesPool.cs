using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Enemies.Interface;
using Assets.Scripts.Factories;
using Assets.Scripts.Factories.Interface;
using Assets.Scripts.Shared.Enums;
using UnityEngine;

namespace Assets.Scripts.Maps
{
    public class ObstaclesPool : MonoBehaviour
    {
        /// <summary>
        /// Defines max amount of obstacles held by this pool.
        /// </summary>
        [SerializeField] private int _maxObstacles = 30;
        /// <summary>
        /// Amount of obstacles created at the beginning.
        /// </summary>
        [SerializeField] private int _beginningObstacles = 10;
        public float SpawnCooldown
        {
            get => _spawnCooldown;
        }
        /// <summary>
        /// Defines amount of time, in seconds, between each spawn.
        /// </summary>
        [SerializeField] private float _spawnCooldown = 2;//
        /// <summary>
        /// Defines what group of obstacles will be stored in the pool.
        /// </summary>
        [SerializeField]
        private ObstacleTypeEnum _storedObstaclesType;
        /// <summary>
        /// Factory that will be used to generate objects.
        /// </summary>
        [SerializeField]
        private IObstacleFactory _obstacleFactory;
        /// <summary>
        /// Stores obstacles in this pool.
        /// </summary>
        private List<IObstacle> _obstacles = new List<IObstacle>();
        /// <summary>
        /// Stores obstacles in this pool.
        /// </summary>
        private Dictionary<uint, IObstacle> _activeObstacles = new Dictionary<uint, IObstacle>();
        /// <summary>
        /// Stores the time that lasted from the most recent obstacle spawn.
        /// </summary>
        private float _currentCooldown = 0.0f;//
        /// <summary>
        /// Stores the last assigned index to an activated obstacle.
        /// </summary>
        private uint _lastAssignedIndex = 0;

        void Start()
        {
            int beginningObstaclesCount = _maxObstacles > _beginningObstacles ? _beginningObstacles : _maxObstacles;
            var obstacleData = new ObstacleIniData();
            for (int i = 0; i < beginningObstaclesCount; i++)
            {
                _obstacles.Add(_obstacleFactory.CreateObstacle(_storedObstaclesType, obstacleData));
            }
        }
        void Update()
        {

        }
        /// <summary>
        /// Sets the value of spawn cooldown.
        /// </summary>
        /// <param name="newValue"></param>
        public void SetSpawnCooldown(float newValue)//
        {
            _spawnCooldown = newValue;
        }
        /// <summary>
        /// Spawns obstacle using provided obstacle data, if possible.
        /// </summary>
        /// <param name="obstacleData"></param>
        public void SpawnObstacle(ObstacleIniData obstacleData)
        {

        }
    }
}
