using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Enemies.Interface;
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
        public ProjectileTypeEnum ProjectileType { get; } = ProjectileTypeEnum.Energy;

        /// <summary>
        /// Seeks for any hookable obstacles/enemies and if found - hooks them to the glove, if possible.
        /// </summary>
        /// <param name="direction">Direction of the seeking raycast.</param>
        /// <param name="team">Team of the seeking caster.</param>
        /// <returns></returns>
        protected override HookingResultEnum TryHookingObject(Vector3 direction, TeamEnum team)
        {
            if (Physics.Raycast(gameObject.transform.position, direction, out _raycastHit))
            {
                var hookedObstacle = _raycastHit.transform.gameObject.GetComponent<IObstacle>();

                if (hookedObstacle == null)
                {
                    return HookingResultEnum.NoObjectFound;
                }

                if (hookedObstacle.ChkGloveType(ProjectileType) == false)
                {
                    return HookingResultEnum.WrongType;
                }

                hookedObstacle.SelectObject(team);
                SaveHookedObjectData(hookedObstacle);
                DrawHookingLine();

                return HookingResultEnum.ObjectHooked;
            }

            return HookingResultEnum.NoObjectFound;
        }

        public override ProjectileTypeEnum GetProjectileType()
        {
            return ProjectileType;
        }
    }
}
