﻿using UnityEngine;

namespace Assets.Scripts.Factories
{
    /// <summary>
    /// Used to pass initialization info to obstacle factory.
    /// </summary>
    public class ObstacleIniData
    {
        /// <summary>
        /// Position of the object accordingly to the parent transform. If transform is null - global position.
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// Rotation of the object accordingly to the parent transform. If transform is null - global rotation.
        /// </summary>
        public Quaternion Rotation { get; set; }
        /// <summary>
        /// Scale of the object.
        /// </summary>
        public Vector3 Scale { get; set; }
        /// <summary>
        /// Transform component of the parent. Can be null.
        /// </summary>
        public Transform ParentTransform { get; set; }
    }
}
