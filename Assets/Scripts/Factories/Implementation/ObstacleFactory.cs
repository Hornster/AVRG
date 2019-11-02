using System;
using Assets.Scripts.Enemies.Implementation;
using Assets.Scripts.Enemies.Interface;
using Assets.Scripts.Factories.Interface;
using Assets.Scripts.Shared.Enums;
using UnityEngine;

namespace Assets.Scripts.Factories.Implementation
{
    public class ObstacleFactory : MonoBehaviour, IObstacleFactory
    {
        [SerializeField]
        private GameObject _physicalObstaclePrefab;
        [SerializeField]
        private GameObject _energyObstaclePrefab;

        private IObstacle CreatePhysicalObstacle(ObstacleIniData iniData)
        {
            var newObstacle = Instantiate(_physicalObstaclePrefab, iniData.Position, iniData.Rotation, iniData.ParentTransform);
            newObstacle.transform.localScale = iniData.Scale;

            IObstacle newObstacleController = newObstacle.GetComponent<PhysicalBlockController>();

            if (newObstacleController == null)
            {
                throw new Exception("Error - Physical obstacle controller not found!");
            }   

            return newObstacleController;
        }

        private IObstacle CreateEnergyObstacle(ObstacleIniData iniData)
        {
            var newObstacle = Instantiate(_energyObstaclePrefab, iniData.Position, iniData.Rotation);
            newObstacle.transform.localScale = iniData.Scale;

            IObstacle newObstacleController = newObstacle.GetComponent<EnergyBlockController>();

            if (newObstacleController == null)
            {
                throw new Exception("Error - Energy obstacle controller not found!");
            }

            return newObstacleController;
        }

        public IObstacle CreateObstacle(ObstacleTypeEnum type, ObstacleIniData iniData)
        {
            switch (type)
            {
                case ObstacleTypeEnum.PhysicalBlock:
                    return CreatePhysicalObstacle(iniData);
                case ObstacleTypeEnum.EnergyBlock:
                    return CreateEnergyObstacle(iniData);
            }

            throw new Exception("Error - Not recognized obstacle type.");
        }
    }
}