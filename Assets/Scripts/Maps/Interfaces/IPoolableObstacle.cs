using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Factories;
using Assets.Scripts.Shared.Enums;
using UnityEngine.Events;

namespace Assets.Scripts.Maps.Interfaces
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Interface for objects that can be used within a pool.
    /// </summary>
    public interface IPoolableObstacle
    {
        /// <summary>
        /// Index assigned to poolable obstacle upon activation. Used by the owner pool to recognize the object in collection.
        /// </summary>
        uint ActivationIndex { get; }
        /// <summary>
        /// Reference to callback that can be used by the obstacle to call its pool when the obstacle has to be
        /// deactivated. UINT argument is the activation index.
        /// </summary>
        UnityAction<uint> DeactivationCallback { get; set; }
        /// <summary>
        /// Applies new data to obstacle.
        /// </summary>
        /// <param name="newData">Data to be applied to obstacle.</param>
        void Mutate(ObstacleIniData newData);
        /// <summary>
        /// Enables the gameobject of the obstacle.
        /// </summary>
        void Activate(uint activationIndex);
        /// <summary>
        /// Disables the gameobject of the obstacle.
        /// </summary>
        void Deactivate();
    }
}
