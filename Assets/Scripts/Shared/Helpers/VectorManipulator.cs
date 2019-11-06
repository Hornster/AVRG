using UnityEngine;

namespace Assets.Scripts.Shared.Helpers
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Performs various manipulations to provided vectors.
    /// </summary>
    public static class VectorManipulator
    {
        /// <summary>
        /// Rotates vector by quaternion.
        /// </summary>
        /// <param name="rotation">Quaternion defining rotation.</param>
        /// <param name="vector">Vector to rotate.</param>
        /// <returns></returns>
        public static Vector3 RotateVector(Quaternion rotation, Vector3 vector)
        {
            return rotation * vector;
        }
    }
}
