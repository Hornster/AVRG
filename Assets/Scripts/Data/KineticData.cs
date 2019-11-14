

using UnityEngine;

namespace Assets.Scripts.Data
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Used to pass kinetic object data.
    /// </summary>
    public class KineticObjectData
    {
        /// <summary>
        /// Current velocity of the object. 
        /// </summary>
        public Vector3 Velocity { get; set; }
        /// <summary>
        /// Mass of the object.
        /// </summary>
        public float Mass { get; set; }
    }
}
