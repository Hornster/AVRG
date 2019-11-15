using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Match.Entities
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// A decorator class that searches for script that inherits IDamageReceiver in gameobject hierarchy - parent and children.
    /// Can be used as a connector between the collider containing object and IDamageReceiver
    /// located on different gameobject that will be used upon collision detection.
    /// </summary>
    class DamageReceiver : MonoBehaviour, IDamageReceiver
    {
        /// <summary>
        /// Reference to the IDamageReceiver that will be called.
        /// </summary>
        private IDamageReceiver _damageReceiver;

        void Start()
        {
            _damageReceiver = GetComponentInParent<IDamageReceiver>();
            if (_damageReceiver != null)
            {
                return;
            }

            _damageReceiver = GetComponentInChildren<IDamageReceiver>();
            if (_damageReceiver == null)
            {
                throw new Exception("Error - Damage Receiver not found in parent, nor in children!");
            }
        }

        /// <summary>
        /// Applies damage to the _damageReceiver.
        /// </summary>
        /// <param name="damage">Amount of damage to be received.</param>
        public void ReceiveDamage(float damage)
        {
            _damageReceiver.ReceiveDamage(damage);
        }

        /// <summary>
        /// Returns kinetic data of the _damageReceiver, like velocity and mass.
        /// </summary>
        /// <returns></returns>
        public KineticObjectData GetKineticData()
        {
            return _damageReceiver.GetKineticData();
        }
    }
}
//TODO add to collider gameobject on player and check if can find the Player script.