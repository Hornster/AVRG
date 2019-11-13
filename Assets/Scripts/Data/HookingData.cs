using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Enemies.Interface;
using UnityEngine;

namespace Assets.Scripts.Data
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Focuses data gathered by glove during obstacle hooking process.
    /// </summary>
    public class HookingData
    {
        /// <summary>
        /// The currently hooked obstacle. If none is hooked - null.
        /// </summary>
        public IObstacle HookedObstacle { get; }

        /// <summary>
        /// Offset of the object towards the glove upon hooking it up.
        /// </summary>
        public Vector3 HookingRefDistance { get; }
        /// <summary>
        /// Stores the magnitude (length) of the reference distance.
        /// </summary>
        public float HookingDistMagnitude { get; }

        public HookingData(IObstacle hookedObstacle, Vector3 hookingRefDistance)
        {
            HookingDistMagnitude = hookingRefDistance.magnitude;
            HookingRefDistance = hookingRefDistance;
            HookedObstacle = hookedObstacle;
        }
    }
}
