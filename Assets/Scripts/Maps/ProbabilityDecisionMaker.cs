using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Shared.Enums;

namespace Assets.Scripts.Maps
{/// <summary>
/// Author: Karol Kozuch
///
/// Performs decisions basing on random generator output.
/// </summary>
    public class ProbabilityDecisionMaker : IDisposable
    {
        private static ProbabilityDecisionMaker _instance;
        /// <summary>
        /// Stores ranges of probabilities for all obstacles (min, max).
        /// </summary>
        private static Dictionary<ObstacleTypeEnum, Tuple<float, float>> _obstacleTypeSpawnChance = new Dictionary<ObstacleTypeEnum, Tuple<float, float>>();

        public static ProbabilityDecisionMaker GetInstance()
        {
            return _instance ?? (_instance = new ProbabilityDecisionMaker());
        }


        /// <summary>
        /// Random generator used for decision what object shall be spawned.
        /// </summary>
        private Random _randomGenerator = new Random();

        public ObstacleTypeEnum SpawnWhatObstacle()
        {
            float value = (float)_randomGenerator.NextDouble();
            var probabilities = _obstacleTypeSpawnChance.Values;
            var keys = _obstacleTypeSpawnChance.Keys;
            for (int i = 0; i < probabilities.Count; i++)
            {
                var currentRange = probabilities.ElementAt(i);
                if (value >= currentRange.Item1 && value < currentRange.Item2)
                {
                    return keys.ElementAt(i);
                }
            }

            return keys.ElementAt(0);
        }
        /// <summary>
        /// Sets the probabilities of spawns for ALL obstacle types at once. Needs value for all types.
        /// </summary>
        /// <param name="spawnProbabilities">Key: Type of obstacle, Value: probability of spawning [0:1]. Sum should be lower or equal to 1.</param>
        public void SetObstacleSpawnProbabilities(Dictionary<ObstacleTypeEnum, float> spawnProbabilities)
        {
            var obstacleTypes = (ObstacleTypeEnum[])Enum.GetValues(typeof(ObstacleTypeEnum));
            ObstacleTypeEnum lastModifiedObstacle = ObstacleTypeEnum.EnergyBlock;
            float overallProbability = 0.0f;
            float lowerBorderProbability = 0.0f;
            float upperBorderProbability = 0.0f;
            foreach (var obstacleType in obstacleTypes)
            {
                float newProbability;
                if (spawnProbabilities.TryGetValue(obstacleType, out newProbability) == false)
                {
                    throw new Exception($"Probabilities concerning ALL types of obstacles must be provided! {obstacleType} was not found!");
                }

                upperBorderProbability = lowerBorderProbability + newProbability;
                _obstacleTypeSpawnChance.Remove(obstacleType);
                _obstacleTypeSpawnChance.Add(obstacleType, Tuple.Create(lowerBorderProbability, upperBorderProbability));
                lowerBorderProbability = upperBorderProbability;

                overallProbability += newProbability;
                lastModifiedObstacle = obstacleType;
            }

            if (overallProbability > 1.0f)
            {
                throw new Exception(
                    "Sum of provided probabilities in obstacle spawn probability cannot be more than 1!");
            }
            //If the probability is lower than 1.0f - add the remaining amount to the last range.
            if (overallProbability < 1.0f)
            {
                Tuple<float, float> probabilityRange;
                _obstacleTypeSpawnChance.TryGetValue(lastModifiedObstacle, out probabilityRange);
                _obstacleTypeSpawnChance.Remove(lastModifiedObstacle);
                var newRange = Tuple.Create(probabilityRange.Item1,
                    probabilityRange.Item2 + (1.0f - probabilityRange.Item2));
                _obstacleTypeSpawnChance.Add(lastModifiedObstacle, newRange);
            }
        }
        public void Dispose()
        {
            _instance = null;
        }
    }
}
