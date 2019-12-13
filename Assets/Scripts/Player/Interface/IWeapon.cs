using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Data;
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
        /// Defines the strength of the weapon (ex. dealt damage per shot).
        /// </summary>
        float Strength { get; }

        /// <summary>
        /// Stores data about the currently hooked obstacle by this glove. If none is hooked - null.
        /// </summary>
        HookingData HookingData { get; }

        /// <summary>
        /// Type of the glove.
        /// </summary>
        ProjectileTypeEnum GetProjectileType();

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
        /// <summary>
        /// Sets new value of the glove strength.
        /// </summary>
        /// <param name="gloveStrength">New glove strength.</param>
        void SetStrength(float gloveStrength);
        /// <summary>
        /// Deactivates the gameobject of the weapon.
        /// </summary>
        void DeactivateWeapon();
        /// <summary>
        /// Activates the gameobject of the weapon.
        /// </summary>
        void ActivateWeapon();
    }
}
