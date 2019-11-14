using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Match.Entities
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Describes entities capable of dealing damage to other entities.
    /// </summary>
    public interface IDamageDealer
    {
        /// <summary>
        /// Deals damage to the receiver.
        /// </summary>
        /// <param name="receiver">Damage receiving entity.</param>
        void DealDamage(IDamageReceiver receiver);
    }
}
