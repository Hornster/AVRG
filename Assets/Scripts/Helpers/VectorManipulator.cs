using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class VectorManipulator
    {
        public static Vector3 RotateVector(Quaternion rotation, Vector3 vector)
        {
            return rotation * vector;
        } 
    }
}
