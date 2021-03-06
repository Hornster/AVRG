﻿using Assets.Scripts.Factories.Interface;
using Assets.Scripts.Player.Interface;
using Assets.Scripts.Shared.Enums;
using System;
using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Factories.Implementation
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Factory that creates weapons for the player.
    /// </summary>
    public class WeaponsFactory : MonoBehaviour, IWeaponsFactory
    {
        [SerializeField]
        private GameObject _energyGlovePrefab;
        [SerializeField]
        private GameObject _physicsGlovePrefab;
        /// <summary>
        /// Factor defining the strength of the glove.
        /// Used to multiply the difference vectors created when the player moves around with
        /// the object hooked to the glove.
        /// </summary>
        [SerializeField] private float _gloveStrength = 10.0f;

        /// <summary>
        /// Creates a tool glove - weapon that is capable of hooking objects of the same projectile type and allows
        /// for influence of their movement.
        /// </summary>
        /// <param name="projectileType">Type of glove (electric/physics).</param>
        /// <param name="parentTransform">Transform of parent object. Can be null.</param>
        /// <returns></returns>
        private IWeapon CreateToolGlove(ProjectileTypeEnum projectileType, Transform parentTransform)
        {
            IWeapon glove = null;
            switch (projectileType)
            {
                case ProjectileTypeEnum.Physical:
                    glove = Instantiate(_physicsGlovePrefab, parentTransform).GetComponent<IWeapon>();
                    break;
                case ProjectileTypeEnum.Energy:
                    glove = Instantiate(_energyGlovePrefab, parentTransform).GetComponent<IWeapon>();
                    break; 
            }

            if (glove == null)
            {
                throw new Exception($"Tool glove with projectiles of type {projectileType} is unknown.");
            }

            glove.SetStrength(_gloveStrength);
            return glove;
        }

        /// <summary>
        /// Creates a weapon, basing on provided arguments.
        /// </summary>
        /// <param name="gloveType">Type of the weapon (what is the weapon doing).</param>
        /// <param name="projectileType">Type of projectiles for the weapon (energy/physical/etc.).</param>
        /// <param name="parentTransform">Transform of the parent object. Can be null.</param>
        /// <returns></returns>
        public IWeapon CreateWeapon(GloveType gloveType, ProjectileTypeEnum projectileType, Transform parentTransform)
        {
            switch (gloveType)
            {
                case GloveType.Glove:
                    return CreateToolGlove(projectileType, parentTransform);
            }

            throw new Exception($"Weapon of type {gloveType} with projectiles of type {projectileType} is unknown.");
        }
        /// <summary>
        /// Creates a weapon, basing on provided arguments.
        /// </summary>
        /// <param name="weaponData">Data of the weapon to be created.</param>
        /// <param name="parentTransform">Transform of the parent object. Can be null.</param>
        /// <returns></returns>
        public IWeapon CreateWeapon(WeaponData weaponData, Transform parentTransform)
        {
            return CreateWeapon(weaponData.GloveType, weaponData.ProjectileType, parentTransform);
        }
    }
}