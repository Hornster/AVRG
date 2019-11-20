using System;
using UnityEngine;

namespace Assets.Scripts.Shared.Helpers
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Calculates physics stuff.
    /// </summary>
    public class Calculations : IDisposable
    {
        private static Calculations _calculations;
        /// <summary>
        /// Returns instance of the singleton.
        /// </summary>
        /// <returns></returns>
        public static Calculations GetInstance()
        {
            return _calculations ?? (_calculations = new Calculations());
        }
        /// <summary>
        /// Calculates kinetic energy described by velocity vector and mass.
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="mass"></param>
        /// <returns></returns>
        public float CalcKineticEnergy(Vector3 velocity, float mass)
        {
            float velocityMagnitude = velocity.magnitude;
            return velocityMagnitude * velocityMagnitude * mass * 0.5f;
        }
        /// <summary>
        /// Calculates volume of a cuboid.
        /// </summary>
        /// <param name="size">Dimensions of the cuboid.</param>
        /// <returns></returns>
        public float CalcCuboidVolume(Vector3 size)
        {
            return size.x * size.y * size.z;
        }
        /// <summary>
        /// Divides provided volume by area, returns the third dimension value.
        /// Note that the axes names are only called like they are to distinguish one from the other.
        /// It doesn't matter whether areaX will be the Z axis, areaY the X axis, etc.
        /// If surface size will be 0 - returns 0.
        /// </summary>
        /// <param name="areaX">X dimension of the area.</param>
        /// <param name="areaY">Y dimension of the area.</param>
        /// <param name="volume">Volume to divide.</param>
        /// <returns></returns>
        public float DivideVolumeByArea(float areaX, float areaY, float volume)
        {
            float areaValue = areaX * areaY;
            if (areaValue == 0.0f)
            {
                return 0.0f;
            }
            return volume / areaValue;
        }

        public void Dispose()
        {
            _calculations = null;
        }
    }
}
