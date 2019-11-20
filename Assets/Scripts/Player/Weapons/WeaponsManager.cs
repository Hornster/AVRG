using System;
using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.Factories.Interface;
using Assets.Scripts.Player.Interface;
using UnityEngine;

namespace Assets.Scripts.Player.Weapons
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Manages gloves that are available to the player.
    /// </summary>
    public class WeaponsManager : MonoBehaviour
    {
        /// <summary>
        /// Stores te index of currently selected weapon.
        /// </summary>
        public int CurrentWeaponIndex { get; private set; }
        /// <summary>
        /// Stories all available weapons for the player.
        /// </summary>
        private List<IWeapon> _availableWeapons = new List<IWeapon>();
        [SerializeField]
        private IWeaponsFactory _weaponsFactory;

        void Awake()
        {
            _weaponsFactory = gameObject.GetComponentInChildren<IWeaponsFactory>();
            if (_weaponsFactory == null)
            {
                _weaponsFactory = gameObject.GetComponentInParent<IWeaponsFactory>();
                if (_weaponsFactory == null)
                {
                    throw new Exception("Weapons factory not found!");
                }
            }
        }
        /// <summary>
        /// Checks if the CurrentWeaponIndex has gone outside of _availableWeapons list.
        /// If yes - sets it (index) to 0.
        /// </summary>
        private void ChkWeaponIndex()
        {
            if (CurrentWeaponIndex >= _availableWeapons.Count
                || CurrentWeaponIndex < 0)
            {
                CurrentWeaponIndex = 0;
            }
        }
        /// <summary>
        /// Recreates list of weapons available for the player, then returns the first weapon.
        /// </summary>
        /// <param name="weapons"></param>
        public IWeapon CreateWeapons(List<WeaponData> weapons)
        {
            if (weapons.Count <= 0)
            {
                throw new Exception("Error: player has to have at least one weapon (glove)!");
            }

            _availableWeapons = new List<IWeapon>(weapons.Count);
            foreach (var weapon in weapons)
            {
                _availableWeapons.Add(_weaponsFactory.CreateWeapon(weapon, transform));
            }

            CurrentWeaponIndex = 0;
            return _availableWeapons[CurrentWeaponIndex];
        }
        /// <summary>
        /// Gets next weapon available for player.
        /// </summary>
        /// <returns></returns>
        public IWeapon GetNextWeapon()
        {
            CurrentWeaponIndex++;
            ChkWeaponIndex();

            return _availableWeapons[CurrentWeaponIndex];
        }
        /// <summary>
        /// Gets current weapon available for the player.
        /// </summary>
        /// <returns></returns>
        public IWeapon GetCurrentWeapon()
        {
            return _availableWeapons[CurrentWeaponIndex];
        }
    }
}
