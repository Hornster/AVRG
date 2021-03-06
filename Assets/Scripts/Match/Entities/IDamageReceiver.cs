﻿using Assets.Scripts.Data;

namespace Assets.Scripts.Match.Entities
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Defines what a killable object has to have.
    /// </summary>
    public interface IDamageReceiver
    {
        /// <summary>
        /// Applies damage to the object.
        /// </summary>
        /// <param name="damage">Amount of damage to be received.</param>
        void ReceiveDamage(float damage);
        /// <summary>
        /// Deals given percent of entire hp as damage to the player.
        /// </summary>
        /// <param name="percent">Percentage of full hp dealt as damage.</param>
        void ReceivePercentalDamage(float percent);
        /// <summary>
        /// Returns kinetic data of the object, like velocity and mass.
        /// </summary>
        /// <returns></returns>
        KineticObjectData GetKineticData();
    }
}
