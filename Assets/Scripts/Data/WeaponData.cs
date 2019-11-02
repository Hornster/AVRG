using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Shared.Enums;

namespace Assets.Scripts.Data
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Class that contains data concerning single player weapon.
    /// </summary>
    public class WeaponData
    {
        /// <summary>
        /// Type of weapon.
        /// </summary>
        public GloveType GloveType { get; set; }
        /// <summary>
        /// Type of weapon projectile.
        /// </summary>
        public ProjectileTypeEnum ProjectileType { get; set; }
    }
}
