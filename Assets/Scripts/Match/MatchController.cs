

using System;
using Assets.Scripts.Maps;
using Assets.Scripts.Maps.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Match
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Match controller for the gameplay.
    /// </summary>
    public class MatchController : MonoBehaviour
    {
        /// <summary>
        /// Spawns enemies.
        /// </summary>
        [SerializeField] private EnemySpawnerRectangular _enemySpawner;
        /// <summary>
        /// By how many seconds will the spawn cooldown decrease after certain time passes?
        /// </summary>
        [SerializeField] private float _spawnAccelerationStep = 0.05f;
        /// <summary>
        /// Minimal allowed spawn cooldown. Player is not a robot, after all.
        /// </summary>
        [SerializeField] private float _spawnMinTime = 0.1f;
        /// <summary>
        /// Every how many seconds will the spawn timer be decreased?
        /// </summary>
        [SerializeField] private float _spawnTimeChangeInterval = 5.0f;
        /// <summary>
        /// Constant force value. Constant force is applied to each obstacle to make it move towards the player.
        /// </summary>
        [SerializeField] private float _constantForceStrength = 10.0f;
        /// <summary>
        /// By how much will be the constant force increased each passed time interval.
        /// </summary>
        [SerializeField] private float _constantForceStrengthStep = 1.0f;
        /// <summary>
        /// Direction of constant force. Constant force is applied to each obstacle to make it move towards the player.
        /// </summary>
        [SerializeField] private Vector3 _constantForceDirection = Vector3.forward;
        /// <summary>
        /// Stores the time that passed since last spawn time modification.
        /// </summary>
        private float _spawnTimeChangeIntervalTimer = 0.0f;
        /// <summary>
        /// Normalizes the force direction vector.
        /// </summary>
        void Start()
        {
            if (_enemySpawner == null)
            {
                throw new Exception("No enemy spawner applied to match controller!");
            }
            _constantForceDirection = _constantForceDirection.normalized;
        }
        void Update()
        {
            float lastFrameTime = Time.deltaTime;
            UpdateSpawnValues(lastFrameTime);
            
        }
        /// <summary>
        /// Updates spawn time and connected with it constant force strength.
        /// </summary>
        /// <param name="lastFrameTime">Time it took to complete last frame.</param>
        private void UpdateSpawnValues(float lastFrameTime)
        {
            if (_enemySpawner.SpawnCooldown >= _spawnMinTime)
            {
                if (_spawnTimeChangeIntervalTimer >= _spawnTimeChangeInterval)
                {
                    _spawnTimeChangeIntervalTimer = 0.0f;
                    _enemySpawner.SpawnCooldown = _enemySpawner.SpawnCooldown - _spawnAccelerationStep;

                    _constantForceStrength += _constantForceStrengthStep;
                    Vector3 constantForceVector = _constantForceDirection * _constantForceStrength;
                    _enemySpawner.EnemiesConstantForce = constantForceVector;
                }
                else
                {
                    _spawnTimeChangeIntervalTimer += lastFrameTime;
                }
            }
        }
    }
}
