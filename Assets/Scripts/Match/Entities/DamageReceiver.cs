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
    /// A proxy class that searches for script that inherits IDamageReceiver in gameobject hierarchy - parent and children.
    /// Can be used as a connector between the collider containing object and IDamageReceiver
    /// located on different gameobject that will be used upon collision detection.
    /// </summary>
    public class DamageReceiver : MonoBehaviour, IDamageReceiver
    {
        /// <summary>
        /// Reference to the IDamageReceiver that will be called.
        /// </summary>
        private IDamageReceiver _damageReceiver;

        private void ChkPossibilities(ref IDamageReceiver[] possibleReceivers)
        {
            foreach (var receiver in possibleReceivers)
            {
                if (receiver == this)
                {
                    // This receiver does not count as it's only a form of a proxy.
                    continue;
                }
                else
                {
                    _damageReceiver = receiver;
                }
            }
        }

        void Start()
        {
            var possibleDamageReceiversInParent = GetComponentsInParent<IDamageReceiver>();

            ChkPossibilities(ref possibleDamageReceiversInParent);
            if (_damageReceiver != null)
            {
                return;
            }
            //Get all possible receivers.
            var possibleReceivers = GetComponentsInChildren<IDamageReceiver>();
            ChkPossibilities(ref possibleReceivers);

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
        /// Deals given percent of entire hp as damage to the player.
        /// </summary>
        /// <param name="percent">Percentage of full hp dealt as damage.</param>
        public void ReceivePercentalDamage(float percent)
        {
            _damageReceiver.ReceivePercentalDamage(percent);
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