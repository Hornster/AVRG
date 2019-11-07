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
        /// <summary>
        /// Rotates a vector by provided rotation and sets it to provided length.
        /// </summary>
        /// <param name="normalizedRotationReference">Rotation reference vector. Must be NORMALIZED.</param>
        /// <param name="rotation">Rotation applied to reference vector.</param>
        /// <param name="length">Length of the resulting vector.</param>
        /// <returns></returns>
        public static Vector3 CreateVectorFromRotation(Vector3 normalizedRotationReference, Quaternion rotation, float length)
        {
            return RotateVector(rotation, normalizedRotationReference) * length;
        }
    }
}
