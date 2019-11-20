using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Data
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Stores spawner connected constants.
    /// </summary>
    public class GameConstants
    {
        /////////////////////
        ///////Coords////////
        /////////////////////
        public const string X = "x";
        public const string Y = "y";
        public const string Z = "z";
        /////////////////////
        //////Spawners///////
        /////////////////////
        public const float SpawnerCooldown = 2;
        public const int MaxPoolObstacles = 30;
        public const int StartingPoolObstacles = 10;
        /////////////////////
        ///////Player////////
        /////////////////////
        public static readonly Vector3 PlayerRotationRefVector = Vector3.forward;
        /////////////////////
        /////Formatting//////
        /////////////////////
        public const string TimeFormat = @"hh\:mm\:ss\.fff";
        public const string TimeTemplate = "00:00:00.000";
    }
}
