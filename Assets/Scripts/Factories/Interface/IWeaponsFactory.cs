using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Data;
using Assets.Scripts.Player.Interface;
using Assets.Scripts.Shared.Enums;
using UnityEngine;

namespace Assets.Scripts.Factories.Interface
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Interface for player weapons factory.
    /// </summary>
    public interface IWeaponsFactory
    {
        /// <summary>
        /// Creates a weapon, basing on provided arguments.
        /// </summary>
        /// <param name="gloveType">Type of the weapon (what is the weapon doing).</param>
        /// <param name="projectileType">Type of projectiles for the weapon (energy/physical/etc.).</param>
        /// <param name="parentTransform">Transform of the parent object. Can be null.</param>
        /// <returns></returns>
        IWeapon CreateWeapon(GloveType gloveType, ProjectileTypeEnum projectileType, Transform parentTransform);
        /// <summary>
        /// Creates a weapon, basing on provided arguments.
        /// </summary>
        /// <param name="weaponData">Data of the weapon to be created.</param>
        /// <param name="parentTransform">Transform of the parent object. Can be null.</param>
        /// <returns></returns>
        IWeapon CreateWeapon(WeaponData weaponData, Transform parentTransform);
    }
}
