using Assets.Scripts.Data;
using Assets.Scripts.Enemies.Interface;
using Assets.Scripts.Player.Interface;
using Assets.Scripts.Player.Weapons.Aesthetics;
using Assets.Scripts.Shared.Enums;
using Assets.Scripts.Player.Helpers;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;
using HookingResultEnum = Assets.Scripts.Shared.Enums.HookingResultEnum;

namespace Assets.Scripts.Player.Weapons
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Physical glove which is used by the player to interact with the world.
    /// </summary>
    public class PhysicsGlove : Glove
    {
        /// <summary>
        /// Type of the glove.
        /// </summary>
        public new ProjectileTypeEnum ProjectileType { get; } = ProjectileTypeEnum.Physical;
    }
}
