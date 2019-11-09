using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Factories;
using Assets.Scripts.Maps.Interfaces;
using Assets.Scripts.Shared.Enums;
using UnityEngine;

namespace Assets.Scripts.Enemies.Interface
{
    /// <summary>
    /// Interface defining simple enemies, usually blocks or gates, that do not
    /// try to attack player on purpose and have no attacking AI.
    /// </summary>
    public interface IObstacle : IPoolableObstacle
    {
        /// <summary>
        /// Applies force to object.
        /// </summary>
        /// <param name="direction">Normalized force vector.</param>
        /// <param name="value">Value by which the vector shall be multiplied.</param>
        void ApplyForce(Vector3 direction, float value);
        /// <summary>
        /// Applies force to object.
        /// </summary>
        /// <param name="direction">Direction of force, including its value.</param>
        void ApplyForce(Vector3 direction);
        /// <summary>
        /// Selects the object, highlighting it characteristically.
        /// </summary>
        void SelectObject(TeamEnum team);
        /// <summary>
        /// Deselects the object, removing the highlight.
        /// </summary>
        void DeselectObject();
        /// <summary>
        /// Checks if type of object is the same as type of the glove that tried to hook it.
        /// </summary>
        /// <param name="projectileType">Type of hooking glove.</param>
        /// <returns>True if types match, false otherwise.</returns>
        bool ChkGloveType(ProjectileTypeEnum projectileType);
        /// <summary>
        /// Retrieves the position of the obstacle in global space.
        /// </summary>
        /// <returns></returns>
        Vector3 GetPosition();
        /// <summary>
        /// Returns current velocity of the object.
        /// </summary>
        /// <returns></returns>
        Vector3 GetVelocity();
    }
}
