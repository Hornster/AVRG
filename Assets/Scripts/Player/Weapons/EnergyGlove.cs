using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Player.Interface;
using Assets.Scripts.Shared.Enums;
using UnityEngine;

namespace Assets.Scripts.Player.Weapons
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Energy glove which is used by the player to interact with the world.
    /// </summary>
    public class EnergyGlove : Glove
    {
        /// <summary>
        /// Type of the glove.
        /// </summary>
        public new ProjectileTypeEnum ProjectileType { get; } = ProjectileTypeEnum.Energy;
    }
}
