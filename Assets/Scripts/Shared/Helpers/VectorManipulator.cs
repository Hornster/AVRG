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
        /// <summary>
        /// Returns a dot product of two provided vectors.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float CalcDotProduct(Vector3 v1, Vector3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }
        /// <summary>
        /// Returns cosinus of angle between two vectors.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float CalcAngleBetweenVectors(Vector3 v1, Vector3 v2)
        {
            return CalcDotProduct(v1.normalized, v2.normalized);
        }
    }
}
