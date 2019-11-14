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

        public void Dispose()
        {
            _calculations = null;
        }
    }
}
