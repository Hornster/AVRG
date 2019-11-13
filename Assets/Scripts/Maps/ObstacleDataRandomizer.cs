using System;
using Assets.Scripts.Factories;
using Assets.Scripts.Shared.Enums;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Maps
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Class that randomizes obstacle data.
    /// </summary>
    public class ObstacleDataRandomizer : MonoBehaviour
    {
        /// <summary>
        /// Transform of the spawner.
        /// </summary>
        private Transform _spawnerTransform;
        /// <summary>
        /// Max position that an obstacle can spawn at.
        /// </summary>
        private Vector3 _maxPosition;
        /// <summary>
        /// Min position that an obstacle can spawn at.
        /// </summary>
        private Vector3 _minPosition;

        /// <summary>
        /// Max scale of a physical cube.
        /// </summary>
        [SerializeField] private Vector3 _maxPhysicalCubeScale = Vector3.one;
        /// <summary>
        /// Min scale of a physical cube.
        /// </summary>
        [SerializeField] private Vector3 _minPhysicalCubeScale = Vector3.one;
        /// <summary>
        /// Max scale of energy cube.
        /// </summary>
        [SerializeField] private Vector3 _maxEnergyCubeScale = Vector3.one;
        /// <summary>
        /// Min scale of a physical cube.
        /// </summary>
        [SerializeField] private Vector3 _minEnergyCubeScale = Vector3.one;
        /// <summary>
        /// Density of energy cube. Measured in Unity units.
        /// </summary>
        [SerializeField] private float _energyCubeDensity = 10.0f;
        /// <summary>
        /// Density of physical cube. Measured in Unity units.
        /// </summary>
        [SerializeField] private float _physicalCubeDensity = 10.0f;

        /// <summary>
        /// Defines minimal step value used when randomizing position and scale.
        /// </summary>
        [SerializeField] private float _smallestLengthUnit = 0.01f;
        /// <summary>
        /// Transform of the parent which the spawned objects will be hooked to.
        /// </summary>
        [SerializeField] private Transform _parentTransform;
        private Random _randomGenerator = new Random();
        /// <summary>
        /// Checks whether provided mins and maxes for scales of cubes are proper. If there's a ny minimum
        /// that's bigger than maximum - swaps the two.
        /// </summary>
        private void ChkObstacleSizes()
        {
            float temp;
            if (_maxEnergyCubeScale.x < _minEnergyCubeScale.x)
            {
                temp = _maxEnergyCubeScale.x;
                _maxEnergyCubeScale.x = _minEnergyCubeScale.x;
                _minEnergyCubeScale.x = temp;
            }
            if (_maxEnergyCubeScale.y < _minEnergyCubeScale.y)
            {
                temp = _maxEnergyCubeScale.y;
                _maxEnergyCubeScale.y = _minEnergyCubeScale.y;
                _minEnergyCubeScale.y = temp;
            }
            if (_maxEnergyCubeScale.z < _minEnergyCubeScale.z)
            {
                temp = _maxEnergyCubeScale.z;
                _maxEnergyCubeScale.z = _minEnergyCubeScale.z;
                _minEnergyCubeScale.z = temp;
            }

            if (_maxPhysicalCubeScale.x < _minPhysicalCubeScale.x)
            {
                temp = _maxPhysicalCubeScale.x;
                _maxPhysicalCubeScale.x = _minPhysicalCubeScale.x;
                _minPhysicalCubeScale.x = temp;
            }
            if (_maxPhysicalCubeScale.y < _minPhysicalCubeScale.y)
            {
                temp = _maxPhysicalCubeScale.y;
                _maxPhysicalCubeScale.y = _minPhysicalCubeScale.y;
                _minPhysicalCubeScale.y = temp;
            }
            if (_maxPhysicalCubeScale.z < _minPhysicalCubeScale.z)
            {
                temp = _maxPhysicalCubeScale.z;
                _maxPhysicalCubeScale.z = _minPhysicalCubeScale.z;
                _minPhysicalCubeScale.z = temp;
            }
        }
        void Start()
        {
            _spawnerTransform = gameObject.transform;
            var offset = _spawnerTransform.localScale * 0.5f; //Scale defines whole width. We need half of it.
            offset = VectorManipulator.RotateVector(_spawnerTransform.rotation, offset);
            _maxPosition = (_spawnerTransform.position + offset) / _smallestLengthUnit;
            _minPosition = (_spawnerTransform.position - offset) / _smallestLengthUnit;

            ChkObstacleSizes();
        }

        void Update()
        {
            _spawnerTransform = gameObject.transform;
            var offset = _spawnerTransform.localScale * 0.5f; //Scale defines whole width. We need half of it.
            offset = VectorManipulator.RotateVector(_spawnerTransform.rotation, offset);
            _maxPosition = (_spawnerTransform.position + offset) / _smallestLengthUnit;
            _minPosition = (_spawnerTransform.position - offset) / _smallestLengthUnit;
        }
        /// <summary>
        /// Calculates mass for freshly randomized obstacle.
        /// </summary>
        /// <param name="obstacleType">Type of the obstacle which the mass is calculated for.</param>
        /// <param name="scale">Scale of the obstacle - defines its size, and refers to a 1x1x1 units cube.</param>
        /// <returns></returns>
        private float CalculateMass(ObstacleTypeEnum obstacleType, Vector3 scale)
        {
            float mass = 1.0f;
            switch (obstacleType)
            {
                case ObstacleTypeEnum.PhysicalBlock:
                    mass = scale.x * scale.y * scale.z * _physicalCubeDensity;
                    break;
                case ObstacleTypeEnum.EnergyBlock:
                    mass = scale.x * scale.y * scale.z * _energyCubeDensity;
                    break;
            }

            if (mass < 0.0f)
            {
                //In case someone makes a mistake and sets negative scale somewhere... And by "someone" I mean myself.
                mass = -mass;
            }

            return mass;
        }
        /// <summary>
        /// Returns the scale vector for provided obstacle type.
        /// </summary>
        /// <param name="obstacleType">Type of the obstacle.</param>
        /// <returns></returns>
        private Vector3 GetScaleVector(ObstacleTypeEnum obstacleType)
        {
            var scaleVector = Vector3.one;
            switch (obstacleType)
            {
                case ObstacleTypeEnum.PhysicalBlock:
                    scaleVector.x = GetRandomNumber(_maxPhysicalCubeScale.x, _minPhysicalCubeScale.x) * _smallestLengthUnit;
                    scaleVector.y = GetRandomNumber(_maxPhysicalCubeScale.y, _minPhysicalCubeScale.y) * _smallestLengthUnit;
                    scaleVector.z = GetRandomNumber(_maxPhysicalCubeScale.z, _minPhysicalCubeScale.z) * _smallestLengthUnit;
                    break;
                case ObstacleTypeEnum.EnergyBlock:
                    scaleVector.x = GetRandomNumber(_maxEnergyCubeScale.x, _minEnergyCubeScale.x) * _smallestLengthUnit;
                    scaleVector.y = GetRandomNumber(_maxEnergyCubeScale.y, _minEnergyCubeScale.y) * _smallestLengthUnit;
                    scaleVector.z = GetRandomNumber(_maxEnergyCubeScale.z, _minEnergyCubeScale.z) * _smallestLengthUnit;
                    break;
            }

            return scaleVector;
        }
        /// <summary>
        /// Returns a pseudo-random number from the provided range. Detects if max is lower than min.
        /// If max and min are of same value - returns max.
        /// </summary>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        private int GetRandomNumber(float max, float min)
        {
            var intMax = (int)max;
            var intMin = (int)min;
            if (intMin > intMax)
            {
                var temp = intMin;
                intMin = intMax;
                intMax = temp;
            }
            else if (intMin == intMax)
            {
                return intMax;
            }

            return _randomGenerator.Next(intMin, intMax);
        }
        /// <summary>
        /// Returns randomized, accordingly to provided in editor configuration, obstacle data.
        /// </summary>
        /// <returns></returns>
        public ObstacleIniData GetRandomizedObstacleData(ObstacleTypeEnum obstacleType)
        {
            var obstacleIniData = new ObstacleIniData();
            var positionVector = new Vector3();
            
            positionVector.x = GetRandomNumber(_maxPosition.x, _minPosition.x) * _smallestLengthUnit;

            positionVector.y = GetRandomNumber(_maxPosition.y, _minPosition.y) * _smallestLengthUnit;

            positionVector.z = GetRandomNumber(_maxPosition.z, _minPosition.z) * _smallestLengthUnit;

            var scaleVector = GetScaleVector(obstacleType);

            obstacleIniData.Position = positionVector;
            obstacleIniData.Scale = scaleVector;
            obstacleIniData.ParentTransform = _parentTransform;
            obstacleIniData.Mass = CalculateMass(obstacleType, scaleVector);
            
            return obstacleIniData;
        }

    }
}