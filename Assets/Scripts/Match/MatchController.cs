

using System;
using Assets.Scripts.Maps;
using Assets.Scripts.Maps.Interfaces;
using Assets.Scripts.Player.GUI;
using UnityEngine;
using UnityEngine.Events;

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
        /// Communication with GUI.
        /// </summary>
        [SerializeField] private GuiManager _guiManager;
        /// <summary>
        /// Reference to the player.
        /// </summary>
        [SerializeField] private PlayerController _player;
        /// <summary>
        /// By how many seconds will the spawn cooldown decrease after certain time passes?
        /// </summary>
        [SerializeField] private float _spawnAccelerationStep = 0.05f;
        /// <summary>
        /// Starting spawn cooldown.
        /// </summary>
        [SerializeField] private readonly float _spawnMaxTime = 2f;
        /// <summary>
        /// Minimal allowed spawn cooldown. Player is not a robot, after all.
        /// </summary>
        [SerializeField] private float _spawnMinTime = 0.1f;
        /// <summary>
        /// Every how many seconds will the spawn timer be decreased?
        /// </summary>
        [SerializeField] private float _spawnTimeChangeInterval = 5.0f;
        /// <summary>
        /// Value of the constant force at the beginning of the round.
        /// </summary>
        [SerializeField] private float _constantForceStartStrength = 10.0f;
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
        /// Constant force value. Constant force is applied to each obstacle to make it move towards the player.
        /// </summary>
        private float _constantForceStrength;
        /// <summary>
        /// Normalizes the force direction vector.
        /// </summary>
        void Start()
        {
            ResetLocalValues();
            if (_enemySpawner == null)
            {
                throw new Exception("No enemy spawner applied to match controller!");
            }
            _enemySpawner.EnemiesConstantForce = GetConstantForceVector();
        }
        void Update()
        {
            float lastFrameTime = Time.deltaTime;
            UpdateSpawnValues(lastFrameTime);
            
        }
        /// <summary>
        /// Calculates constant force vector.
        /// </summary>
        /// <returns></returns>
        private Vector3 GetConstantForceVector()
        {
            _constantForceDirection = _constantForceDirection.normalized;
            return _constantForceDirection * _constantForceStrength;
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
                    _enemySpawner.EnemiesConstantForce = GetConstantForceVector();
                }
                else
                {
                    _spawnTimeChangeIntervalTimer += lastFrameTime;
                }
            }
        }
        /// <summary>
        /// Resets local values.
        /// </summary>
        private void ResetLocalValues()
        {
            _constantForceStrength = _constantForceStartStrength;
        }
        /// <summary>
        /// The round has ended.
        /// </summary>
        public void RoundEnded()
        {
            _guiManager.RoundEnded();
        }
        /// <summary>
        /// Callback method - user decided to leave the level.
        /// </summary>
        public void RoundExit()
        {
            //TODO make it return to menu, when it will be created.
            Application.Quit();
        }
        /// <summary>
        /// Callback method - user decided to restart the level.
        /// </summary>
        public void RoundRestart()
        {
            ResetLocalValues();
            _enemySpawner.ResetSpawner(_spawnMaxTime, GetConstantForceVector());
            _player.ResetPlayer();
            _guiManager.RestartRound();
        }
    }
}
