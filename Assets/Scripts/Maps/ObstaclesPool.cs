using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Enemies.Interface;
using Assets.Scripts.Factories;
using Assets.Scripts.Factories.Interface;
using Assets.Scripts.Maps.Interfaces;
using Assets.Scripts.Shared.Enums;
using UnityEngine;

namespace Assets.Scripts.Maps
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Pool for provided group of obstacles. 
    /// </summary>
    public class ObstaclesPool
    {
        /// <summary>
        /// Defines max amount of obstacles held by this pool.
        /// </summary>
        public int MaxObstacles { get; set; }
        /// <summary>
        /// Stores amount of obstacles created in total.
        /// </summary>
        private int _obstaclesInUse = 0;
        /// <summary>
        /// Amount of obstacles created at the beginning.
        /// </summary>
        public int BeginningObstacles { get; }
        
        /// <summary>
        /// Defines what group of obstacles will be stored in the pool.
        /// </summary>
        public ObstacleTypeEnum StoredObstaclesType { get; }
        /// <summary>
        /// Factory that will be used to generate objects.
        /// </summary>
        private IObstacleFactory _obstacleFactory;
        /// <summary>
        /// Stores obstacles in this pool.
        /// </summary>
        private List<IPoolableObstacle> _obstacles = new List<IPoolableObstacle>();
        /// <summary>
        /// Stores obstacles in this pool.
        /// </summary>
        private Dictionary<uint, IPoolableObstacle> _activeObstacles = new Dictionary<uint, IPoolableObstacle>();
        
        /// <summary>
        /// Stores the last assigned index to an activated obstacle.
        /// </summary>
        private uint _lastAssignedIndex = 0;

        /// <summary>
        /// Activates the obstacle.
        /// </summary>
        /// <param name="index">Index of activated obstacle in _obstacles list.</param>
        private void ActivateObstacle(int index)
        {
            _obstacles[index].Activate(_lastAssignedIndex);
            _activeObstacles.Add(_lastAssignedIndex, _obstacles[index]);
            _obstacles.RemoveAt(index);

            _lastAssignedIndex++;
        }
        /// <summary>
        /// Adds new obstacle to the pool.
        /// </summary>
        /// <param name="obstacleData">Data defining the obstacle.</param>
        private void AddNewObstacle(ObstacleIniData obstacleData)
        {
            var newObstacle = _obstacleFactory.CreateObstacle(StoredObstaclesType, obstacleData);
            newObstacle.DeactivationCallback = DeactivateObstacle;
            _obstacles.Add(newObstacle);
        }
        /// <summary>
        /// Creates new obstacles pool. If maxObstacles is smaller than startingObstaclesAmount - the latter will
        /// have assigned the value of the first one.
        /// </summary>
        /// <param name="obstacleTypeEnum">Type of the obstacle that this pool will be storing.</param>
        /// <param name="obstacleFactory">Factory used to create  the obstacles.</param>
        /// <param name="maxObstacles">Maximal amount of obstacles this pool can store.</param>
        /// <param name="startingObstaclesAmount">Starting amount of obstacles that will be instantly ready.</param>
        public ObstaclesPool(ObstacleTypeEnum obstacleTypeEnum, IObstacleFactory obstacleFactory, int maxObstacles,
            int startingObstaclesAmount)
        {
            if (maxObstacles < startingObstaclesAmount)
            {
                startingObstaclesAmount = maxObstacles;
            }

            StoredObstaclesType = obstacleTypeEnum;
            _obstacleFactory = obstacleFactory;
            MaxObstacles = maxObstacles;
            BeginningObstacles = startingObstaclesAmount;

            int beginningObstaclesCount = MaxObstacles > BeginningObstacles ? BeginningObstacles : MaxObstacles;
            var obstacleData = new ObstacleIniData();
            for (int i = 0; i < beginningObstaclesCount; i++)
            {
                AddNewObstacle(obstacleData);
                _obstaclesInUse++;
            }
        }

        /// <summary>
        /// Spawns obstacle using provided obstacle data, if possible.
        /// </summary>
        /// <param name="obstacleData">Data concerning the obstacle.</param>
        /// <param name="constantForce">Constant force that will push the object onwards.</param>
        public void SpawnObstacle(ObstacleIniData obstacleData)
        {
            //If there are no free obstacles left - create new one. Do not use it yet - it has to be initialized first
            //what will happen  during next frame.
            if (_obstacles.Count <= 0)
            {
                if (_obstaclesInUse < MaxObstacles)
                {
                    AddNewObstacle(obstacleData);
                    _obstaclesInUse++;
                }

            }
            //Otherwise mutate first object in the free list and use it.
            else
            {
                var obstacleToActivate = _obstacles[0];
                obstacleToActivate.Mutate(obstacleData);
                ActivateObstacle(0);
            }
        }
        /// <summary>
        /// Event handler used for the obstacles themselves - when an obstacle wants to become deactivated, it can call this
        /// method to be deactivated and brought back to awaiting objects pool.
        /// </summary>
        /// <param name="obstacleIndex">ID of the obstacle assigned upon activation.</param>
        public void DeactivateObstacle(uint obstacleIndex)
        {
            IPoolableObstacle obstacle;

            if (_activeObstacles.TryGetValue(obstacleIndex, out obstacle) == false)
            {
                throw new Exception($"Tried to deactivate non-existing obstacle of index {obstacleIndex}!");
            }
            
            _activeObstacles.Remove(obstacleIndex);
            _obstacles.Add(obstacle);
        }
        /// <summary>
        /// Resets positions of all obstacles in the pool and deactivates them.
        /// </summary>
        public void ResetPool()
        {
            var obstacleKeys = _activeObstacles.Keys.ToList();
            foreach (var obstacleKey in obstacleKeys)
            {
                IPoolableObstacle obstacle;
                _activeObstacles.TryGetValue(obstacleKey, out obstacle);
                obstacle.Deactivate();
            }
        }
    }
}
//TODO: Add callback to IPoolables so they can deactivate themselves when necessary. DUN