using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Enemies.Interface;
using Assets.Scripts.Shared.Enums;
using UnityEngine;

namespace Assets.Scripts.Player.Interface
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Interface for player gloves.
    /// </summary>
    public interface IWeapon
    {
        /// <summary>
        /// The currently hooked obstacle by this glove. If none is hooked - null.
        /// </summary>
        IObstacle HookedObstacle { get; }
        /// <summary>
        /// Type of the glove.
        /// </summary>
        ProjectileTypeEnum ProjectileType { get; }

        /// <summary>
        /// Casts a ray. If any objects are on the way - tries to hook the ray to one of these, starting
        /// from the closest one.
        /// </summary>
        /// <param name="direction">Direction in global coords of the raycast.</param>
        /// <param name="team">Team of the using player.</param>
        /// <param name="gloveType">Type of glove that tries to hook the object.</param>
        HookingResultEnum UseWeapon(Vector3 direction, TeamEnum team);

        /// <summary>
        /// Called when the player stops holding the object.
        /// </summary>
        void StopUsingWeapon();
    }
}
